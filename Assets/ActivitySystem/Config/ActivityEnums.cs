using System;

namespace ActivityFramework
{
    /// <summary>
    /// 活动分组类型，对应 UI 上的 Tab 页签
    /// </summary>
    public enum ActivityTabType
    {
        Daily,      // 日常活动
        Weekly,     // 周常活动
        Event,      // 限时活动
        Achievement // 成就
    }

    /// <summary>
    /// 活动整体运行时状态
    /// </summary>
    public enum ActivityState
    {
        Locked,     // 未解锁
        Available,  // 可参与
        InProgress,  // 进行中（有未完成目标）
        Completed,  // 已完成（可领奖）
        Rewarded,   // 已领奖
        Expired     // 已过期
    }

    /// <summary>
    /// 单个任务目标的运行时状态
    /// </summary>
    public enum TaskState
    {
        NotStarted, // 未开始
        InProgress,  // 进行中
        Completed,   // 已完成
        Claimed      // 已领取
    }

    /// <summary>
    /// 任务目标类型，决定如何触发进度
    /// </summary>
    public enum TaskType
    {
        LoginDays,       // 累计登录 N 天
        KillMonster,     // 击杀怪物 N 只
        ClearStage,      // 通关副本 N 次
        SpendCurrency,   // 消耗货币 N 数量
        ObtainItem,      // 获得道具 N 个
        ReachLevel,      // 达到等级 N
        InviteFriend,    // 邀请好友 N 人
        ShareGame,       // 分享游戏 1 次
        WatchAd,         // 观看广告 N 次
        PVPWin,          // PVP 胜利 N 次
        Custom           // 自定义（由外部代码触发）
    }

    /// <summary>
    /// 奖励物品类型
    /// </summary>
    public enum RewardType
    {
        Currency,    // 货币（金币/钻石等）
        Item,        // 道具
        Hero,        // 角色
        Skin,        // 皮肤
        Exp,         // 经验
        Title        // 称号
    }

    /// <summary>
    /// 游戏事件 ID，用于事件总线触发任务进度
    /// </summary>
    public static class GameEventID
    {
        public const string Login = "evt_login";
        public const string KillMonster = "evt_kill_monster";
        public const string ClearStage = "evt_clear_stage";
        public const string SpendCurrency = "evt_spend_currency";
        public const string ObtainItem = "evt_obtain_item";
        public const string ReachLevel = "evt_reach_level";
        public const string InviteFriend = "evt_invite_friend";
        public const string ShareGame = "evt_share_game";
        public const string WatchAd = "evt_watch_ad";
        public const string PVPWin = "evt_pvp_win";
        public const string Custom = "evt_custom";

        /// <summary>
        /// 根据 TaskType 获取对应的 EventID
        /// </summary>
        public static string GetEventID(TaskType taskType)
        {
            switch (taskType)
            {
                case TaskType.LoginDays: return Login;
                case TaskType.KillMonster: return KillMonster;
                case TaskType.ClearStage: return ClearStage;
                case TaskType.SpendCurrency: return SpendCurrency;
                case TaskType.ObtainItem: return ObtainItem;
                case TaskType.ReachLevel: return ReachLevel;
                case TaskType.InviteFriend: return InviteFriend;
                case TaskType.ShareGame: return ShareGame;
                case TaskType.WatchAd: return WatchAd;
                case TaskType.PVPWin: return PVPWin;
                default: return Custom;
            }
        }
    }
}
