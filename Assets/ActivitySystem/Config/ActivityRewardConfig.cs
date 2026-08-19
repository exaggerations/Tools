using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 奖励配置（可序列化，嵌入 ActivityConfig 的列表中）
    /// </summary>
    [System.Serializable]
    public class ActivityRewardConfig
    {
        [SerializeField] private RewardType rewardType;
        [SerializeField] private int rewardId;      // 道具/角色/皮肤的 ID
        [SerializeField] private int amount;
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;

        public RewardType RewardType => rewardType;
        public int RewardId => rewardId;
        public int Amount => amount;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
    }
}
