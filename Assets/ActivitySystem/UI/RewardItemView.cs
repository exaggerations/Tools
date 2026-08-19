using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// 奖励项视图：显示单个奖励的图标、名称、数量
    /// </summary>
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private GameObject claimedTag; // "已领取"标签

        public void Refresh(ActivityRewardConfig reward)
        {
            if (reward == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            if (iconImage != null)
            {
                iconImage.sprite = reward.Icon;
                iconImage.gameObject.SetActive(reward.Icon != null);
            }

            if (nameText != null)
                nameText.text = string.IsNullOrEmpty(reward.DisplayName) ? reward.RewardType.ToString() : reward.DisplayName;

            if (amountText != null)
                amountText.text = $"x{reward.Amount}";
        }

        public void SetClaimed(bool claimed)
        {
            if (claimedTag != null)
                claimedTag.SetActive(claimed);
        }
    }
}
