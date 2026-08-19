using System;

namespace ActivityFramework
{
    /// <summary>
    /// 游戏事件：由游戏玩法触发，驱动活动任务进度
    /// </summary>
    public struct GameEvent
    {
        public string EventID;
        public int IntParam;      // 数量/等级/怪物ID 等
        public string StrParam;  // 额外字符串参数

        public GameEvent(string eventID, int intParam = 1, string strParam = null)
        {
            EventID = eventID;
            IntParam = intParam;
            StrParam = strParam;
        }

        public static GameEvent Login()
        {
            return new GameEvent(GameEventID.Login, 1);
        }

        public static GameEvent KillMonster(int monsterId, int count = 1)
        {
            return new GameEvent(GameEventID.KillMonster, count, monsterId.ToString());
        }

        public static GameEvent ClearStage(int stageId, int count = 1)
        {
            return new GameEvent(GameEventID.ClearStage, count, stageId.ToString());
        }

        public static GameEvent SpendCurrency(int amount)
        {
            return new GameEvent(GameEventID.SpendCurrency, amount);
        }

        public static GameEvent ObtainItem(int itemId, int count = 1)
        {
            return new GameEvent(GameEventID.ObtainItem, count, itemId.ToString());
        }

        public static GameEvent ReachLevel(int level)
        {
            return new GameEvent(GameEventID.ReachLevel, level);
        }

        public static GameEvent PVPWin(int count = 1)
        {
            return new GameEvent(GameEventID.PVPWin, count);
        }

        public static GameEvent Custom(string eventId, int intParam = 1, string strParam = null)
        {
            return new GameEvent(eventId, intParam, strParam);
        }
    }
}
