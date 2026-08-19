using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 活动系统核心管理器（单例）
    /// 负责：初始化、事件分发、状态刷新、奖励领取、存档管理
    /// </summary>
    public class ActivityManager : MonoBehaviour
    {
        public static ActivityManager Instance { get; private set; }

        [Header("Config")]
        [SerializeField] private ActivityGroupConfig[] groupConfigs;

        [Header("Settings")]
        [SerializeField] private float statusRefreshInterval = 30f;
        [SerializeField] private bool useLocalStorage = true;

        // --- 内部状态 ---
        private readonly Dictionary<int, ActivityInstance> _instances = new Dictionary<int, ActivityInstance>();
        private ActivitySaveData _saveData;
        private IActivityStorage _storage;
        private IActivityService _service;
        private float _refreshTimer;
        private bool _initialized;

        // --- 事件回调（UI 层订阅） ---
        public event Action OnActivityListChanged;
        public event Action<ActivityInstance, TaskHandler> OnTaskProgressChanged;
        public event Action<ActivityInstance> OnActivityExpired;

        // ==================== 生命周期 ====================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (!_initialized) return;

            _refreshTimer += Time.deltaTime;
            if (_refreshTimer >= statusRefreshInterval)
            {
                _refreshTimer = 0;
                RefreshActivityStates();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            EventBus.ClearAll();
        }

        // ==================== 初始化 ====================

        public void Initialize()
        {
            if (_initialized) return;

            // 存储层
            _storage = useLocalStorage ? new ActivityLocalStorage() : null;
            _saveData = _storage?.Load() ?? new ActivitySaveData();

            // 登录天数计算
            UpdateLoginDays();

            // 从分组配置创建活动实例
            if (groupConfigs != null)
            {
                foreach (var groupConfig in groupConfigs)
                {
                    if (groupConfig?.Activities == null) continue;
                    foreach (var activityConfig in groupConfig.Activities)
                    {
                        CreateActivityInstance(activityConfig);
                    }
                }
            }

            // 订阅所有任务事件
            SubscribeGameEvents();

            _initialized = true;
            OnActivityListChanged?.Invoke();

            Debug.Log($"[ActivityManager] 初始化完成，共 {_instances.Count} 个活动");
        }

        void CreateActivityInstance(ActivityConfig config)
        {
            if (config == null) return;
            if (_instances.ContainsKey(config.ActivityId))
            {
                Debug.LogWarning($"[ActivityManager] 活动 ID {config.ActivityId} 重复，跳过");
                return;
            }

            var runtimeData = _saveData.GetOrCreateActivity(config.ActivityId);
            var instance = new ActivityInstance(config, runtimeData);
            instance.OnTaskCompleted += (handler, inst) =>
            {
                OnTaskProgressChanged?.Invoke(inst, handler);
                OnActivityListChanged?.Invoke();
            };
            _instances[config.ActivityId] = instance;
        }

        // ==================== 事件分发 ====================

        void SubscribeGameEvents()
        {
            EventBus.Subscribe(GameEventID.Login, OnGameEvent);
            EventBus.Subscribe(GameEventID.KillMonster, OnGameEvent);
            EventBus.Subscribe(GameEventID.ClearStage, OnGameEvent);
            EventBus.Subscribe(GameEventID.SpendCurrency, OnGameEvent);
            EventBus.Subscribe(GameEventID.ObtainItem, OnGameEvent);
            EventBus.Subscribe(GameEventID.ReachLevel, OnGameEvent);
            EventBus.Subscribe(GameEventID.InviteFriend, OnGameEvent);
            EventBus.Subscribe(GameEventID.ShareGame, OnGameEvent);
            EventBus.Subscribe(GameEventID.WatchAd, OnGameEvent);
            EventBus.Subscribe(GameEventID.PVPWin, OnGameEvent);
            EventBus.Subscribe(GameEventID.Custom, OnGameEvent);
        }

        void OnGameEvent(GameEvent evt)
        {
            foreach (var kvp in _instances)
            {
                kvp.Value.HandleEvent(evt);
            }
            SaveData();
        }

        // ==================== 状态刷新 ====================

        void RefreshActivityStates()
        {
            DateTime now = DateTime.UtcNow;
            var expiredIds = new List<int>();

            foreach (var kvp in _instances)
            {
                var state = kvp.Value.GetState(now);
                if (state == ActivityState.Expired)
                {
                    expiredIds.Add(kvp.Key);
                    OnActivityExpired?.Invoke(kvp.Value);
                }
            }

            if (expiredIds.Count > 0)
            {
                OnActivityListChanged?.Invoke();
                Debug.Log($"[ActivityManager] {expiredIds.Count} 个活动已过期");
            }
        }

        // ==================== 登录天数 ====================

        void UpdateLoginDays()
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var today = DateTimeOffset.FromUnixTimeSeconds(now).LocalDateTime.Date;
            var lastDate = _saveData.lastLoginDateUnix > 0
                ? DateTimeOffset.FromUnixTimeSeconds(_saveData.lastLoginDateUnix).LocalDateTime.Date
                : DateTime.MinValue;

            if (lastDate != today)
            {
                if (lastDate == DateTime.MinValue || (today - lastDate).Days >= 1)
                {
                    _saveData.loginDayCount++;
                    _saveData.lastLoginDateUnix = now;
                    EventBus.Emit(GameEvent.Login());
                    Debug.Log($"[ActivityManager] 登录天数: {_saveData.loginDayCount}");
                }
            }
        }

        // ==================== 奖励领取 ====================

        /// <summary>
        /// 领取单个任务奖励
        /// </summary>
        public bool ClaimReward(int activityId, int taskId)
        {
            if (!_instances.TryGetValue(activityId, out var instance))
            {
                Debug.LogWarning($"[ActivityManager] 活动 {activityId} 不存在");
                return false;
            }

            bool success = instance.ClaimReward(taskId);
            if (success)
            {
                SaveData();
                OnActivityListChanged?.Invoke();
            }
            return success;
        }

        /// <summary>
        /// 一键领取指定活动的所有可领取奖励
        /// </summary>
        public int ClaimAllRewards(int activityId)
        {
            if (!_instances.TryGetValue(activityId, out var instance))
                return 0;

            int count = instance.ClaimAllRewards();
            if (count > 0)
            {
                SaveData();
                OnActivityListChanged?.Invoke();
            }
            return count;
        }

        /// <summary>
        /// 一键领取所有活动的所有可领取奖励
        /// </summary>
        public int ClaimAllRewardsAllActivities()
        {
            int total = 0;
            foreach (var kvp in _instances)
            {
                total += kvp.Value.ClaimAllRewards();
            }
            if (total > 0)
            {
                SaveData();
                OnActivityListChanged?.Invoke();
                Debug.Log($"[ActivityManager] 全部领取完成，共 {total} 个奖励");
            }
            return total;
        }

        // ==================== UI 查询接口 ====================

        /// <summary>
        /// 获取指定 Tab 下的所有活动（已排序）
        /// </summary>
        public List<ActivityInstance> GetActivitiesByTab(ActivityTabType tabType)
        {
            DateTime now = DateTime.UtcNow;
            var result = new List<ActivityInstance>();

            foreach (var kvp in _instances)
            {
                if (kvp.Value.Config.TabType == tabType)
                {
                    var state = kvp.Value.GetState(now);
                    // 过滤掉已过期和未解锁的
                    if (state != ActivityState.Expired && state != ActivityState.Locked)
                        result.Add(kvp.Value);
                }
            }

            // 按 sortOrder 排序
            result.Sort((a, b) => a.Config.SortOrder.CompareTo(b.Config.SortOrder));
            return result;
        }

        /// <summary>
        /// 获取所有 Tab 分组信息
        /// </summary>
        public List<ActivityGroupConfig> GetTabGroups()
        {
            if (groupConfigs == null) return new List<ActivityGroupConfig>();
            return groupConfigs.ToList();
        }

        /// <summary>
        /// 获取单个活动
        /// </summary>
        public ActivityInstance GetActivity(int activityId)
        {
            _instances.TryGetValue(activityId, out var instance);
            return instance;
        }

        /// <summary>
        /// 是否有可领取的奖励（主界面红点）
        /// </summary>
        public bool HasAnyClaimableReward()
        {
            foreach (var kvp in _instances)
            {
                if (kvp.Value.HasClaimableTask())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定 Tab 是否有可领取奖励
        /// </summary>
        public bool HasClaimableRewardInTab(ActivityTabType tabType)
        {
            foreach (var kvp in _instances)
            {
                if (kvp.Value.Config.TabType == tabType && kvp.Value.HasClaimableTask())
                    return true;
            }
            return false;
        }

        // ==================== 存档 ====================

        void SaveData()
        {
            _storage?.Save(_saveData);
        }

        /// <summary>
        /// 手动保存存档
        /// </summary>
        public void Save()
        {
            SaveData();
        }

        // ==================== 外部触发 ====================

        /// <summary>
        /// 外部触发自定义事件（供游戏玩法侧调用）
        /// </summary>
        public static void TriggerEvent(GameEvent evt)
        {
            EventBus.Emit(evt);
        }

        /// <summary>
        /// 设置网络服务（联机模式下替换为真实网络实现）
        /// </summary>
        public void SetService(IActivityService service)
        {
            _service = service;
        }
    }
}
