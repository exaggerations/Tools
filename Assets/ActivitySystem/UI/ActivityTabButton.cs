using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// Tab 页签按钮：显示分组名称/图标 + 红点
    /// </summary>
    public class ActivityTabButton : MonoBehaviour
    {
        [SerializeField] private Image tabIcon;
        [SerializeField] private TMP_Text tabNameText;
        [SerializeField] private GameObject activeHighlight;
        [SerializeField] private GameObject redDot;
        [SerializeField] private Button button;

        private System.Action<ActivityTabType> _onClick;
        private ActivityTabType _tabType;

        public void Initialize(System.Action<ActivityTabType> onClick)
        {
            _onClick = onClick;
            if (button == null)
                button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnClick);
            }
        }

        public void Refresh(ActivityGroupConfig group, bool isActive, bool showRedDot)
        {
            _tabType = group.TabType;

            if (tabNameText != null)
                tabNameText.text = string.IsNullOrEmpty(group.TabDisplayName) ? group.TabType.ToString() : group.TabDisplayName;

            if (tabIcon != null)
            {
                tabIcon.sprite = group.TabIcon;
                tabIcon.gameObject.SetActive(group.TabIcon != null);
            }

            if (activeHighlight != null)
                activeHighlight.SetActive(isActive);

            if (redDot != null)
                redDot.SetActive(showRedDot);
        }

        void OnClick()
        {
            _onClick?.Invoke(_tabType);
        }
    }
}
