using System.Collections.Generic;
using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 活动分组配置：定义一个 Tab 页签下包含哪些活动
    /// 策划可在 Inspector 中拖入多个 ActivityConfig
    /// </summary>
    [CreateAssetMenu(fileName = "ActivityGroupConfig", menuName = "ActivityFramework/Activity Group Config", order = 1)]
    public class ActivityGroupConfig : ScriptableObject
    {
        [SerializeField] private ActivityTabType tabType;
        [SerializeField] private string tabDisplayName;
        [SerializeField] private Sprite tabIcon;
        [SerializeField] private int sortOrder;
        [SerializeField] private List<ActivityConfig> activities = new List<ActivityConfig>();

        public ActivityTabType TabType => tabType;
        public string TabDisplayName => tabDisplayName;
        public Sprite TabIcon => tabIcon;
        public int SortOrder => sortOrder;
        public List<ActivityConfig> Activities => activities;
    }
}
