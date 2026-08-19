using System;
using System.Collections.Generic;

namespace ActivityFramework
{
    /// <summary>
    /// 活动网络服务接口：联机游戏可对接服务器实现
    /// 单机游戏可使用 ActivityMockService 本地模拟
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// 拉取当前有效的活动配置列表
        /// </summary>
        void FetchActivities(Action<List<ActivityConfig>> onSuccess, Action<string> onError);

        /// <summary>
        /// 提交任务进度到服务器
        /// </summary>
        void SubmitProgress(int activityId, int taskId, int progress, Action onSuccess, Action<string> onError);

        /// <summary>
        /// 领取奖励请求
        /// </summary>
        void ClaimReward(int activityId, int taskId, Action<bool> onSuccess, Action<string> onError);

        /// <summary>
        /// 拉取玩家活动存档
        /// </summary>
        void FetchPlayerData(Action<ActivitySaveData> onSuccess, Action<string> onError);
    }
}
