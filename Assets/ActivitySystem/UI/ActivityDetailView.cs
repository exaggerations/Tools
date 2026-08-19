using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// 活动详情视图：展示活动信息、任务列表、一键领取
    /// </summary>
    public class ActivityDetailView : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private Image bannerImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private TMP_Text timerText;

        [Header("Tasks")]
        [SerializeField] private Transform taskContainer;
        [SerializeField] private TaskItemView taskItemPrefab;

        [Header("Actions")]
        [SerializeField] private Button claimAllButton;
        [SerializeField] private TMP_Text claimAllButtonText;
        [SerializeField] private Button closeButton;

        private ActivityInstance _instance;
        private readonly List<TaskItemView> _taskViews = new List<TaskItemView>();

        void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));

            if (claimAllButton != null)
                claimAllButton.onClick.AddListener(OnClaimAllPressed);
        }

        void Update()
        {
            if (_instance != null && timerText != null)
            {
                timerText.text = ActivityTimer.GetActivityTimeText(_instance.Config, System.DateTime.UtcNow);
            }
        }

        public void Show(ActivityInstance instance)
        {
            _instance = instance;
            if (_instance == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            Refresh();
        }

        void Refresh()
        {
            var config = _instance.Config;

            if (bannerImage != null)
            {
                bannerImage.sprite = config.BannerIcon;
                bannerImage.gameObject.SetActive(config.BannerIcon != null);
            }

            if (titleText != null)
                titleText.text = config.ActivityName;

            if (descText != null)
                descText.text = config.Description;

            RefreshTasks();
            UpdateClaimAllButton();
        }

        void RefreshTasks()
        {
            if (taskContainer == null || taskItemPrefab == null) return;

            // 清除旧的
            for (int i = taskContainer.childCount - 1; i >= 0; i--)
                Destroy(taskContainer.GetChild(i).gameObject);
            _taskViews.Clear();

            // 创建新的
            foreach (var handler in _instance.TaskHandlers)
            {
                var item = Instantiate(taskItemPrefab, taskContainer);
                item.Initialize(_instance.Config.ActivityId, OnClaimSingleReward);
                item.Refresh(handler);
                _taskViews.Add(item);
            }
        }

        void UpdateClaimAllButton()
        {
            bool canClaim = _instance.HasClaimableTask();
            if (claimAllButton != null)
                claimAllButton.interactable = canClaim;
            if (claimAllButtonText != null)
                claimAllButtonText.text = canClaim ? "一键领取" : "无可领取";
        }

        void OnClaimSingleReward(int activityId, int taskId)
        {
            if (ActivityManager.Instance == null) return;
            ActivityManager.Instance.ClaimReward(activityId, taskId);
            // 刷新当前面板
            RefreshCurrentView();
        }

        void OnClaimAllPressed()
        {
            if (_instance == null) return;
            if (ActivityManager.Instance != null)
                ActivityManager.Instance.ClaimAllRewards(_instance.Config.ActivityId);
            RefreshCurrentView();
        }

        void RefreshCurrentView()
        {
            if (_instance == null) return;
            // 刷新任务项
            var handlers = _instance.TaskHandlers;
            for (int i = 0; i < _taskViews.Count && i < handlers.Count; i++)
            {
                _taskViews[i].Refresh(handlers[i]);
            }
            UpdateClaimAllButton();
        }
    }
}
