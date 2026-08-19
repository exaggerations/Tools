using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// 活动卡片视图：在列表中展示活动入口
    /// </summary>
    public class ActivityCardView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text stateText;
        [SerializeField] private GameObject redDot;
        [SerializeField] private Button cardButton;

        private ActivityInstance _instance;
        private System.Action<ActivityInstance> _onClick;

        public void Initialize(System.Action<ActivityInstance> onClick)
        {
            _onClick = onClick;
            if (cardButton == null)
                cardButton = GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(OnClick);
            }
        }

        public void Refresh(ActivityInstance instance)
        {
            _instance = instance;
            if (_instance == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            var config = _instance.Config;
            if (iconImage != null)
            {
                iconImage.sprite = config.EntryIcon;
                iconImage.gameObject.SetActive(config.EntryIcon != null);
            }

            if (nameText != null)
                nameText.text = config.ActivityName;

            if (stateText != null)
            {
                var state = _instance.GetState(System.DateTime.UtcNow);
                stateText.text = GetStateText(state, _instance);
            }

            // 红点
            if (redDot != null)
                redDot.SetActive(_instance.HasClaimableTask());
        }

        void Update()
        {
            if (_instance != null && timerText != null)
            {
                timerText.text = ActivityTimer.GetActivityTimeText(_instance.Config, System.DateTime.UtcNow);
            }
        }

        string GetStateText(ActivityState state, ActivityInstance inst)
        {
            switch (state)
            {
                case ActivityState.InProgress:
                    // 显示进度
                    int completed = 0;
                    int total = inst.TaskHandlers.Count;
                    foreach (var h in inst.TaskHandlers)
                        if (h.IsCompleted) completed++;
                    return $"{completed}/{total}";
                case ActivityState.Completed:
                    return "可领取";
                case ActivityState.Rewarded:
                    return "已完成";
                default:
                    return state.ToString();
            }
        }

        void OnClick()
        {
            _onClick?.Invoke(_instance);
        }
    }
}
