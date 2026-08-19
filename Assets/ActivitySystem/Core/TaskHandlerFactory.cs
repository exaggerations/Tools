using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 任务处理器工厂：根据 TaskType 创建对应的处理器
    /// </summary>
    public static class TaskHandlerFactory
    {
        public static TaskHandler Create(ActivityTaskConfig config, TaskProgressData progress)
        {
            if (config == null)
            {
                Debug.LogError("[TaskHandlerFactory] 任务配置为 null");
                return null;
            }

            switch (config.TaskType)
            {
                case TaskType.ReachLevel:
                    return new MaxValueTaskHandler(config, progress);

                case TaskType.ShareGame:
                case TaskType.InviteFriend:
                    return new OnceTaskHandler(config, progress);

                // 以下均使用累加型
                case TaskType.KillMonster:
                case TaskType.ClearStage:
                case TaskType.SpendCurrency:
                case TaskType.ObtainItem:
                case TaskType.LoginDays:
                case TaskType.WatchAd:
                case TaskType.PVPWin:
                case TaskType.Custom:
                default:
                    return new AccumulateTaskHandler(config, progress);
            }
        }
    }
}
