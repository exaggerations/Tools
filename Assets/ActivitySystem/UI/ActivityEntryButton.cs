using UnityEngine;
using UnityEngine.UI;

namespace ActivityFramework
{
    /// <summary>
    /// 主界面活动入口按钮：红点提示 + 打开面板
    /// </summary>
    public class ActivityEntryButton : MonoBehaviour
    {
        [SerializeField] private Button entryButton;
        [SerializeField] private GameObject redDot;
        [SerializeField] private ActivityPanel activityPanel;

        private void Awake()
        {
            if (entryButton == null)
                entryButton = GetComponent<Button>();
            if (entryButton != null)
                entryButton.onClick.AddListener(OnEntryClicked);
        }

        private void OnEnable()
        {
            if (ActivityManager.Instance != null)
            {
                ActivityManager.Instance.OnActivityListChanged += UpdateRedDot;
                UpdateRedDot();
            }
        }

        private void OnDisable()
        {
            if (ActivityManager.Instance != null)
                ActivityManager.Instance.OnActivityListChanged -= UpdateRedDot;
        }

        void UpdateRedDot()
        {
            if (redDot == null || ActivityManager.Instance == null) return;
            redDot.SetActive(ActivityManager.Instance.HasAnyClaimableReward());
        }

        void OnEntryClicked()
        {
            if (activityPanel != null)
            {
                activityPanel.Open();
            }
            else
            {
                Debug.LogWarning("[ActivityEntryButton] ActivityPanel 未设置");
            }
        }
    }
}
