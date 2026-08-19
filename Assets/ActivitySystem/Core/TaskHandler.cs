using System;

namespace ActivityFramework
{
    /// <summary>
    /// 任务处理器基类：负责处理特定任务类型的进度计算逻辑
    /// 不同任务类型有不同的进度策略（累加/取最大值/按条件过滤）
    /// </summary>
    public abstract class TaskHandler
    {
        protected readonly ActivityTaskConfig config;
        protected readonly TaskProgressData progress;

        public event Action<TaskHandler> OnProgressChanged;
        public event Action<TaskHandler> OnTaskCompleted;

        public int TaskId => config.TaskId;
        public TaskType TaskType => config.TaskType;
        public int CurrentProgress => progress.currentProgress;
        public int TargetAmount => config.TargetAmount;
        public bool IsCompleted => progress.currentProgress >= config.TargetAmount;
        public bool IsClaimed => progress.IsClaimed;
        public TaskState State => progress.state;
        public ActivityTaskConfig Config => config;

        public float ProgressNormalized =>
            config.TargetAmount > 0
                ? (float)progress.currentProgress / config.TargetAmount
                : 0f;

        protected TaskHandler(ActivityTaskConfig config, TaskProgressData progress)
        {
            this.config = config;
            this.progress = progress ?? new TaskProgressData
            {
                taskId = config.TaskId,
                currentProgress = 0,
                state = TaskState.NotStarted
            };
        }

        /// <summary>
        /// 由 ActivityInstance 调用，将事件传递给处理器
        /// </summary>
        public abstract void HandleEvent(GameEvent evt);

        /// <summary>
        /// 外部直接设置进度值（如服务器同步）
        /// </summary>
        public virtual void SetProgress(int value)
        {
            if (value <= progress.currentProgress && config.TaskType != TaskType.ReachLevel)
                return;

            progress.currentProgress = value;
            CheckCompletion();
            OnProgressChanged?.Invoke(this);
        }

        /// <summary>
        /// 子类通过此方法触发进度变更事件（event 不能在子类中直接 Invoke）
        /// </summary>
        protected void RaiseProgressChanged() => OnProgressChanged?.Invoke(this);

        protected void AddProgress(int amount)
        {
            progress.currentProgress += amount;
            CheckCompletion();
            RaiseProgressChanged();
        }

       public void CheckCompletion()
        {
            if (progress.currentProgress >= config.TargetAmount)
            {
                if (progress.state == TaskState.NotStarted || progress.state == TaskState.InProgress)
                {
                    progress.state = TaskState.Completed;
                    OnTaskCompleted?.Invoke(this);
                }
            }
            else if (progress.currentProgress > 0)
            {
                if (progress.state == TaskState.NotStarted)
                    progress.state = TaskState.InProgress;
            }
        }

        public void MarkClaimed()
        {
            progress.state = TaskState.Claimed;
            progress.claimTimeUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    /// <summary>
    /// 累加型任务：击杀、通关、消费、获取道具等（最常见）
    /// </summary>
    public class AccumulateTaskHandler : TaskHandler
    {
        public AccumulateTaskHandler(ActivityTaskConfig config, TaskProgressData progress)
            : base(config, progress) { }

        public override void HandleEvent(GameEvent evt)
        {
            if (IsCompleted) return;

            // 如果有自定义参数过滤（如特定怪物ID），检查是否匹配
            if (config.CustomEventParam != 0)
            {
                if (evt.StrParam != null && int.TryParse(evt.StrParam, out int paramValue))
                {
                    if (paramValue != config.CustomEventParam) return;
                }
            }

            AddProgress(evt.IntParam);
        }
    }

    /// <summary>
    /// 取最大值型任务：达到等级 N（等级只能升不能降）
    /// </summary>
    public class MaxValueTaskHandler : TaskHandler
    {
        public MaxValueTaskHandler(ActivityTaskConfig config, TaskProgressData progress)
            : base(config, progress) { }

        public override void HandleEvent(GameEvent evt)
        {
            if (IsCompleted) return;
            SetProgress(evt.IntParam);
        }

        public override void SetProgress(int value)
        {
            if (value <= progress.currentProgress) return;
            progress.currentProgress = value;
            CheckCompletion();
            RaiseProgressChanged();
        }
    }

    /// <summary>
    /// 一次性任务：分享、邀请等只需做1次
    /// </summary>
    public class OnceTaskHandler : TaskHandler
    {
        public OnceTaskHandler(ActivityTaskConfig config, TaskProgressData progress)
            : base(config, progress) { }

        public override void HandleEvent(GameEvent evt)
        {
            if (IsCompleted) return;
            SetProgress(1);
        }

        public override void SetProgress(int value)
        {
            if (progress.currentProgress >= 1) return;
            progress.currentProgress = 1;
            CheckCompletion();
            RaiseProgressChanged();
        }
    }
}
