using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 任务目标配置：定义单个任务的目标类型、目标值和对应奖励
    /// </summary>
    [System.Serializable]
    public class ActivityTaskConfig
    {
        [SerializeField] private int taskId;
        [SerializeField] private string taskName;
        [SerializeField] private string description;
        [SerializeField] private TaskType taskType;
        [SerializeField] private int targetAmount = 1;
        [SerializeField] private int customEventParam; // 自定义参数（如击杀特定怪物ID）
        [SerializeField] private ActivityRewardConfig[] rewards;

        public int TaskId => taskId;
        public string TaskName => taskName;
        public string Description => description;
        public TaskType TaskType => taskType;
        public int TargetAmount => targetAmount;
        public int CustomEventParam => customEventParam;
        public ActivityRewardConfig[] Rewards => rewards;
    }
}
