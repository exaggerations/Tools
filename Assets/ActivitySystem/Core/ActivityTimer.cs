using System;

namespace ActivityFramework
{
    /// <summary>
    /// 活动倒计时工具类
    /// </summary>
    public static class ActivityTimer
    {
        /// <summary>
        /// 格式化剩余时间为可读字符串
        /// </summary>
        public static string FormatRemaining(double seconds)
        {
            if (seconds <= 0) return "已结束";

            TimeSpan ts = TimeSpan.FromSeconds(seconds);

            if (ts.TotalDays >= 1)
                return $"{(int)ts.TotalDays}天{ts.Hours}时{ts.Minutes}分";
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}时{ts.Minutes}分{ts.Seconds}秒";
            if (ts.TotalMinutes >= 1)
                return $"{(int)ts.TotalMinutes}分{ts.Seconds}秒";
            return $"{ts.Seconds}秒";
        }

        /// <summary>
        /// 格式化开始倒计时
        /// </summary>
        public static string FormatCountdown(double seconds)
        {
            if (seconds <= 0) return "进行中";
            return "距开始 " + FormatRemaining(seconds);
        }

        /// <summary>
        /// 获取活动时间描述
        /// </summary>
        public static string GetActivityTimeText(ActivityConfig config, DateTime now)
        {
            if (now < config.StartTime && config.StartTime != DateTime.MinValue)
            {
                double secs = (config.StartTime - now).TotalSeconds;
                return FormatCountdown(secs);
            }

            if (now > config.EndTime && config.EndTime != DateTime.MinValue)
                return "已结束";

            double remaining = config.GetRemainingSeconds(now);
            return "剩余 " + FormatRemaining(remaining);
        }
    }
}
