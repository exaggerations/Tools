using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 本地模拟网络服务：所有请求立即返回本地数据
    /// 用于单机模式或开发调试
    /// </summary>
    public class ActivityMockService : IActivityService
    {
        readonly List<ActivityConfig> localConfigs;
        readonly ActivitySaveData localSaveData;

        public ActivityMockService(List<ActivityConfig> configs, ActivitySaveData saveData = null)
        {
            localConfigs = configs ?? new List<ActivityConfig>();
            localSaveData = saveData ?? new ActivitySaveData();
        }

        public void FetchActivities(Action<List<ActivityConfig>> onSuccess, Action<string> onError)
        {
            Debug.Log("[ActivityMockService] FetchActivities - 返回本地配置");
            onSuccess?.Invoke(new List<ActivityConfig>(localConfigs));
        }

        public void SubmitProgress(int activityId, int taskId, int progress, Action onSuccess, Action<string> onError)
        {
            Debug.Log($"[ActivityMockService] SubmitProgress - activity:{activityId} task:{taskId} progress:{progress}");
            onSuccess?.Invoke();
        }

        public void ClaimReward(int activityId, int taskId, Action<bool> onSuccess, Action<string> onError)
        {
            Debug.Log($"[ActivityMockService] ClaimReward - activity:{activityId} task:{taskId}");
            onSuccess?.Invoke(true);
        }

        public void FetchPlayerData(Action<ActivitySaveData> onSuccess, Action<string> onError)
        {
            Debug.Log("[ActivityMockService] FetchPlayerData - 返回本地存档");
            onSuccess?.Invoke(localSaveData);
        }
    }
}
