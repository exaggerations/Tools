using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 活动运行时实例：绑定配置与存档，管理任务进度，响应游戏事件
    /// </summary>
    public class ActivityInstance
    {
        public ActivityConfig Config { get; }
        public ActivityRuntimeData RuntimeData { get; }

        readonly List<TaskHandler> taskHandlers = new List<TaskHandler>();

        public IReadOnlyList<TaskHandler> TaskHandlers => taskHandlers;

        public event Action<ActivityInstance> OnActivityStateChanged;
        public event Action<TaskHandler, ActivityInstance> OnTaskCompleted;

        public ActivityInstance(ActivityConfig config, ActivityRuntimeData runtimeData)
        {
            Config = config;
            RuntimeData = runtimeData;

            if (Config.Tasks == null) return;

            // 为每个任务配置创建 Handler
            foreach (var taskConfig in Config.Tasks)
            {
                if (taskConfig == null) continue;
                var progress = runtimeData.GetOrCreateTaskProgress(taskConfig.TaskId);
                var handler = TaskHandlerFactory.Create(taskConfig, progress);
                if (handler != null)
                {
                    handler.OnTaskCompleted += h => OnTaskCompleted?.Invoke(h, this);
                    taskHandlers.Add(handler);
                }
            }
        }

        /// <summary>
        /// 获取活动当前运行时状态
        /// </summary>
        public ActivityState GetState(DateTime now)
        {
            // 已过期
            if (now > Config.EndTime && Config.EndTime != DateTime.MinValue)
                return ActivityState.Expired;

            // 未开始
            if (now < Config.StartTime && Config.StartTime != DateTime.MinValue)
                return ActivityState.Locked;

            // 所有任务都已领取
            bool allClaimed = true;
            foreach (var handler in taskHandlers)
            {
                if (!handler.IsClaimed)
                {
                    allClaimed = false;
                    break;
                }
            }
            if (allClaimed && taskHandlers.Count > 0)
                return ActivityState.Rewarded;

            // 有已完成的任务可领取
            foreach (var handler in taskHandlers)
            {
                if (handler.IsCompleted && !handler.IsClaimed)
                    return ActivityState.Completed;
            }

            // 有未完成的任务
            return ActivityState.InProgress;
        }

        /// <summary>
        /// 是否有可领取奖励的任务（红点判定）
        /// </summary>
        public bool HasClaimableTask()
        {
            foreach (var handler in taskHandlers)
            {
                if (handler.IsCompleted && !handler.IsClaimed)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 所有任务是否已完成
        /// </summary>
        public bool AllTasksCompleted()
        {
            if (taskHandlers.Count == 0) return false;
            foreach (var handler in taskHandlers)
            {
                if (!handler.IsCompleted) return false;
            }
            return true;
        }

        /// <summary>
        /// 处理游戏事件：分发给匹配的任务处理器
        /// </summary>
        public void HandleEvent(GameEvent evt)
        {
            foreach (var handler in taskHandlers)
            {
                string handlerEventID = GameEventID.GetEventID(handler.TaskType);
                if (handlerEventID == evt.EventID || handler.TaskType == TaskType.Custom)
                {
                    handler.HandleEvent(evt);
                }
            }
        }

        /// <summary>
        /// 领取指定任务的奖励
        /// </summary>
        public bool ClaimReward(int taskId)
        {
            foreach (var handler in taskHandlers)
            {
                if (handler.TaskId == taskId)
                {
                    if (handler.IsCompleted && !handler.IsClaimed)
                    {
                        handler.MarkClaimed();
                        GrantRewards(handler.Config);
                        OnActivityStateChanged?.Invoke(this);
                        Debug.Log($"[ActivityInstance] 领取奖励: 活动={Config.ActivityName}, 任务={handler.Config.TaskName}");
                        return true;
                    }
                }
            }
            return false;
        }

        void GrantRewards(ActivityTaskConfig taskConfig)
        {
            if (taskConfig.Rewards == null) return;
            foreach (var reward in taskConfig.Rewards)
            {
                Debug.Log($"[ActivityInstance] 发放奖励: {reward.DisplayName} x{reward.Amount} (类型={reward.RewardType})");
            }
        }

        /// <summary>
        /// 一键领取所有可领取的奖励
        /// </summary>
        public int ClaimAllRewards()
        {
            int claimed = 0;
            foreach (var handler in taskHandlers)
            {
                if (handler.IsCompleted && !handler.IsClaimed)
                {
                    handler.MarkClaimed();
                    GrantRewards(handler.Config);
                    claimed++;
                }
            }
            if (claimed > 0)
            {
                OnActivityStateChanged?.Invoke(this);
                Debug.Log($"[ActivityInstance] 一键领取 {claimed} 个奖励: 活动={Config.ActivityName}");
            }
            return claimed;
        }
    }
}
