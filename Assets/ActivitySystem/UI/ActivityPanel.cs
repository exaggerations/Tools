using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ActivityFramework
{
    /// <summary>
    /// 活动主面板：Tab 页签 + 活动列表 + 详情入口
    /// </summary>
    public class ActivityPanel : MonoBehaviour
    {
        [Header("Tabs")]
        [SerializeField] private Transform tabContainer;
        [SerializeField] private ActivityTabButton tabButtonPrefab;

        [Header("Activity List")]
        [SerializeField] private Transform cardContainer;
        [SerializeField] private ActivityCardView cardPrefab;
        [SerializeField] private GameObject emptyTip;

        [Header("Detail")]
        [SerializeField] private ActivityDetailView detailView;

        [Header("Close")]
        [SerializeField] private Button closeButton;

        private ActivityTabType _currentTab;
        private readonly List<ActivityTabButton> _tabButtons = new List<ActivityTabButton>();
        private readonly List<ActivityCardView> _cardViews = new List<ActivityCardView>();

        void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        void OnEnable()
        {
            if (ActivityManager.Instance != null)
                ActivityManager.Instance.OnActivityListChanged += RefreshAll;
            RefreshAll();
        }

        void OnDisable()
        {
            if (ActivityManager.Instance != null)
                ActivityManager.Instance.OnActivityListChanged -= RefreshAll;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _currentTab = GetFirstAvailableTab();
            RefreshAll();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        void RefreshAll()
        {
            RefreshTabs();
            RefreshCards();
        }

        // ==================== Tab ====================

        void RefreshTabs()
        {
            if (tabContainer == null || tabButtonPrefab == null) return;
            if (ActivityManager.Instance == null) return;

            var groups = ActivityManager.Instance.GetTabGroups();
            if (groups.Count == 0) return;

            // 增量创建
            for (int i = 0; i < groups.Count; i++)
            {
                if (i >= _tabButtons.Count)
                {
                    var btn = Instantiate(tabButtonPrefab, tabContainer);
                    btn.Initialize(OnTabSelected);
                    _tabButtons.Add(btn);
                }

                bool isActive = groups[i].TabType == _currentTab;
                bool hasRedDot = ActivityManager.Instance.HasClaimableRewardInTab(groups[i].TabType);
                _tabButtons[i].Refresh(groups[i], isActive, hasRedDot);
            }

            // 隐藏多余的
            for (int i = groups.Count; i < _tabButtons.Count; i++)
                _tabButtons[i].gameObject.SetActive(false);
        }

        void OnTabSelected(ActivityTabType tabType)
        {
            _currentTab = tabType;
            RefreshCards();
        }

        ActivityTabType GetFirstAvailableTab()
        {
            if (ActivityManager.Instance == null) return ActivityTabType.Daily;
            var groups = ActivityManager.Instance.GetTabGroups();
            return groups.Count > 0 ? groups[0].TabType : ActivityTabType.Daily;
        }

        // ==================== Activity Cards ====================

        void RefreshCards()
        {
            if (cardContainer == null || cardPrefab == null) return;
            if (ActivityManager.Instance == null) return;

            var activities = ActivityManager.Instance.GetActivitiesByTab(_currentTab);

            // 空提示
            if (emptyTip != null)
                emptyTip.SetActive(activities.Count == 0);

            // 增量创建/刷新
            for (int i = 0; i < activities.Count; i++)
            {
                if (i >= _cardViews.Count)
                {
                    var card = Instantiate(cardPrefab, cardContainer);
                    card.Initialize(OnCardClicked);
                    _cardViews.Add(card);
                }

                _cardViews[i].gameObject.SetActive(true);
                _cardViews[i].Refresh(activities[i]);
            }

            // 隐藏多余的
            for (int i = activities.Count; i < _cardViews.Count; i++)
                _cardViews[i].gameObject.SetActive(false);
        }

        void OnCardClicked(ActivityInstance instance)
        {
            if (detailView != null)
                detailView.Show(instance);
        }
    }
}
