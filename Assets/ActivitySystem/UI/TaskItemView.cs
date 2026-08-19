using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// 任务项视图：显示单个任务目标、进度、奖励、领取按钮
    /// </summary>
    public class TaskItemView : MonoBehaviour
    {
        [Header("Task Info")]
        [SerializeField] private TMP_Text taskNameText;
        [SerializeField] private TMP_Text descText;

        [Header("Progress")]
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TMP_Text progressText;

        [Header("Rewards")]
        [SerializeField] private Transform rewardContainer;
        [SerializeField] private RewardItemView rewardItemPrefab;

        [Header("Action")]
        [SerializeField] private Button claimButton;
        [SerializeField] private TMP_Text claimButtonText;
        [SerializeField] private GameObject completedTag;

        private TaskHandler _handler;
        private int _activityId;
        private System.Action<int, int> _onClaim; // (activityId, taskId)

        public void Initialize(int activityId, System.Action<int, int> onClaim)
        {
            _activityId = activityId;
            _onClaim = onClaim;

            if (claimButton != null)
            {
                claimButton.onClick.RemoveAllListeners();
                claimButton.onClick.AddListener(OnClaimPressed);
            }
        }

        public void Refresh(TaskHandler handler)
        {
            _handler = handler;
            if (_handler == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            var config = handler.Config;
            if (taskNameText != null)
                taskNameText.text = config.TaskName;

            if (descText != null)
                descText.text = config.Description;

            // 进度
            if (progressSlider != null)
                progressSlider.value = handler.ProgressNormalized;

            if (progressText != null)
                progressText.text = $"{handler.CurrentProgress}/{handler.TargetAmount}";

            // 奖励列表
            RefreshRewards(config);

            // 按钮状态
            UpdateButtonState();
        }

        void RefreshRewards(ActivityTaskConfig config)
        {
            if (rewardContainer == null || rewardItemPrefab == null || config.Rewards == null) return;

            // 简单策略：每次刷新时清除并重建
            for (int i = rewardContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(rewardContainer.GetChild(i).gameObject);
            }

            foreach (var reward in config.Rewards)
            {
                var item = Instantiate(rewardItemPrefab, rewardContainer);
                item.Refresh(reward);
                item.SetClaimed(_handler.IsClaimed);
            }
        }

        void UpdateButtonState()
        {
            if (claimButton != null)
                claimButton.interactable = _handler.IsCompleted && !_handler.IsClaimed;

            if (claimButtonText != null)
            {
                if (_handler.IsClaimed)
                    claimButtonText.text = "已领取";
                else if (_handler.IsCompleted)
                    claimButtonText.text = "领取";
                else
                    claimButtonText.text = "未完成";
            }

            if (completedTag != null)
                completedTag.SetActive(_handler.IsClaimed);
        }

        void OnClaimPressed()
        {
            if (_handler == null) return;
            _onClaim?.Invoke(_activityId, _handler.TaskId);
        }
    }
}
