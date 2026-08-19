using System;
using System.Collections.Generic;

namespace ActivityFramework
{
    /// <summary>
    /// 单个任务的运行时进度（可序列化存档）
    /// </summary>
    [Serializable]
    public class TaskProgressData
    {
        public int taskId;
        public int currentProgress;
        public TaskState state;
        public long claimTimeUnix; // 领奖时间戳

        public bool IsCompleted => state == TaskState.Completed || state == TaskState.Claimed;
        public bool IsClaimed => state == TaskState.Claimed;
    }

    /// <summary>
    /// 单个活动的运行时数据（可序列化存档）
    /// </summary>
    [Serializable]
    public class ActivityRuntimeData
    {
        public int activityId;
        public List<TaskProgressData> taskProgresses = new List<TaskProgressData>();
        public ActivityState activityState;
        public long firstJoinTimeUnix; // 首次参与时间

        /// <summary>
        /// 获取指定任务的进度，不存在则返回 null
        /// </summary>
        public TaskProgressData GetTaskProgress(int taskId)
        {
            for (int i = 0; i < taskProgresses.Count; i++)
            {
                if (taskProgresses[i].taskId == taskId)
                    return taskProgresses[i];
            }
            return null;
        }

        /// <summary>
        /// 获取或创建任务进度
        /// </summary>
        public TaskProgressData GetOrCreateTaskProgress(int taskId)
        {
            var progress = GetTaskProgress(taskId);
            if (progress == null)
            {
                progress = new TaskProgressData
                {
                    taskId = taskId,
                    currentProgress = 0,
                    state = TaskState.NotStarted
                };
                taskProgresses.Add(progress);
            }
            return progress;
        }

        /// <summary>
        /// 是否所有任务都已完成
        /// </summary>
        public bool AllTasksCompleted(int totalTaskCount)
        {
            if (taskProgresses.Count < totalTaskCount) return false;
            foreach (var tp in taskProgresses)
            {
                if (!tp.IsCompleted) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 整体存档数据容器（顶层可序列化）
    /// </summary>
    [Serializable]
    public class ActivitySaveData
    {
        public List<ActivityRuntimeData> activities = new List<ActivityRuntimeData>();
        public int loginDayCount;        // 累计登录天数
        public long lastLoginDateUnix;   // 上次登录日期

        /// <summary>
        /// 获取活动的运行时数据，不存在则创建
        /// </summary>
        public ActivityRuntimeData GetOrCreateActivity(int activityId)
        {
            for (int i = 0; i < activities.Count; i++)
            {
                if (activities[i].activityId == activityId)
                    return activities[i];
            }

            var data = new ActivityRuntimeData { activityId = activityId };
            activities.Add(data);
            return data;
        }
    }
}
