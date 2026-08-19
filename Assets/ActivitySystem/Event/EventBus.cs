using System;
using System.Collections.Generic;

namespace ActivityFramework
{
    /// <summary>
    /// 全局事件总线：解耦游戏玩法与活动系统
    /// 玩法侧只需 Emit，活动侧 Subscribe
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<string, Action<GameEvent>> _handlers
            = new Dictionary<string, Action<GameEvent>>();

        /// <summary>
        /// 订阅指定事件
        /// </summary>
        public static void Subscribe(string eventID, Action<GameEvent> handler)
        {
            if (string.IsNullOrEmpty(eventID) || handler == null) return;

            if (!_handlers.ContainsKey(eventID))
                _handlers[eventID] = null;

            _handlers[eventID] += handler;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public static void Unsubscribe(string eventID, Action<GameEvent> handler)
        {
            if (string.IsNullOrEmpty(eventID) || handler == null) return;

            if (_handlers.ContainsKey(eventID))
                _handlers[eventID] -= handler;
        }

        /// <summary>
        /// 发送事件，触发所有订阅者
        /// </summary>
        public static void Emit(GameEvent evt)
        {
            if (string.IsNullOrEmpty(evt.EventID)) return;

            if (_handlers.TryGetValue(evt.EventID, out var handler))
                handler?.Invoke(evt);
        }

        /// <summary>
        /// 清空所有订阅（场景切换/测试时调用）
        /// </summary>
        public static void ClearAll()
        {
            _handlers.Clear();
        }
    }
}
