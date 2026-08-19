using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 活动配置 ScriptableObject：策划在 Inspector 中配置单个活动的所有信息
    /// </summary>
    [CreateAssetMenu(fileName = "ActivityConfig", menuName = "ActivityFramework/Activity Config", order = 0)]
    public class ActivityConfig : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private int activityId;
        [SerializeField] private string activityName;
        [SerializeField] private string description;
        [SerializeField] private ActivityTabType tabType;
        [SerializeField] private Sprite bannerIcon;
        [SerializeField] private Sprite entryIcon;

        [Header("Schedule (UTC)")]
        [SerializeField] private string startTimeISO;   // ISO 8601 格式，如 "2026-07-18T00:00:00Z"
        [SerializeField] private string endTimeISO;

        [Header("Display")]
        [SerializeField] private int sortOrder;
        [SerializeField] private bool showRedDotWhenCompletable = true;

        [Header("Tasks")]
        [SerializeField] private List<ActivityTaskConfig> tasks = new List<ActivityTaskConfig>();

        // --- 运行时只读属性 ---
        public int ActivityId => activityId;
        public string ActivityName => activityName;
        public string Description => description;
        public ActivityTabType TabType => tabType;
        public Sprite BannerIcon => bannerIcon;
        public Sprite EntryIcon => entryIcon;
        public int SortOrder => sortOrder;
        public bool ShowRedDotWhenCompletable => showRedDotWhenCompletable;
        public List<ActivityTaskConfig> Tasks => tasks;

        public DateTime StartTime => ParseISO(startTimeISO);
        public DateTime EndTime => ParseISO(endTimeISO);

        /// <summary>
        /// 根据当前时间判断活动是否在有效期内
        /// </summary>
        public bool IsTimeValid(DateTime now)
        {
            return now >= StartTime && now <= EndTime;
        }

        /// <summary>
        /// 获取活动剩余时间（秒），已过期返回 0
        /// </summary>
        public double GetRemainingSeconds(DateTime now)
        {
            if (now >= EndTime) return 0;
            return (EndTime - now).TotalSeconds;
        }

        static DateTime ParseISO(string iso)
        {
            if (string.IsNullOrEmpty(iso))
                return DateTime.MinValue;
            if (DateTime.TryParse(iso, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
                return dt;
            return DateTime.MinValue;
        }
    }
}
