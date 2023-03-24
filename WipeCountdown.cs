using System;
using System.Globalization;

namespace Oxide.Plugins
{
    [Info("Wipe Countdown", "Mist8kenGAS", "0.1.0")]
    [Description("Shows a countdown and when the next wipe will occur")]
    class WipeCountdown: RustPlugin {
        private PluginConfig config;

        private int WipeCycleDays;
        private DayOfWeek WipeDayOfWeek;
        private DateTime NextWipeDate;
        private TimeSpan NextWipeTime;

        private class PluginConfig
        {
            public string WipeCycle;
            public string WipeDay;
            public int WipeTimeHourUTC;
            public int WipeTimeMinuteUTC;
        }

        private void Init()
        {
            // setup plugin here
            config = Config.ReadObject<PluginConfig>();

            // config: WipeCycle
            if (config.WipeCycle == "monthly") WipeCycleDays = 31;
            if (config.WipeCycle == "biweekly") WipeCycleDays = 14;
            else WipeCycleDays = 7;

            // config: WipeDay
            switch (config.WipeDay)
            {
                case "sunday":
                    WipeDayOfWeek = DayOfWeek.Sunday;
                    break;
                case "monday":
                    WipeDayOfWeek = DayOfWeek.Monday;
                    break;
                case "tuesday":
                    WipeDayOfWeek = DayOfWeek.Tuesday;
                    break;
                case "wednesday":
                    WipeDayOfWeek = DayOfWeek.Wednesday;
                    break;
                case "thursday":
                    WipeDayOfWeek = DayOfWeek.Thursday;
                    break;
                case "friday":
                    WipeDayOfWeek = DayOfWeek.Friday;
                    break;
                case "saturday":
                    WipeDayOfWeek = DayOfWeek.Saturday;
                    break;
                default:
                    WipeDayOfWeek = DayOfWeek.Friday;
                    break;
            }
        }

        #region plugin setup
        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        private PluginConfig GetDefaultConfig()
        {
            return new PluginConfig
            {
                WipeCycle = "weekly",
                WipeDay = "friday",
                WipeTimeHourUTC = 14,
                WipeTimeMinuteUTC= 0
            };
        }

        private void SaveConfig()
        {
            Config.WriteObject(config, true);
        }
        #endregion

        #region next wipe functions
        private string GetNextWipeMsg()
        {
            // convert: ts (js) -> c#
            //
            // function getNextDay(day: number, date = new Date()) {
            //     const currentDate = date.getDate()
            //     const currentDay = date.getDay()
            //
            //     let nextDate = currentDate + (day - currentDay)
            //
            //     if (nextDate <= currentDate) nextDate += 7
            //     date.setDate(nextDate)
            //
            //     return date
            // }

            // calculate next wipe date
            var DateNow = DateTime.UtcNow;
            NextWipeDate = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, config.WipeTimeHourUTC, config.WipeTimeMinuteUTC, 0);
            
            var NextDay = (int)WipeDayOfWeek - (int)DateNow.DayOfWeek;
            var NextDate = DateNow.Day + NextDay;

            // offset date to next wipe date
            if (NextDate <= DateNow.Day)
            {
                NextDay += WipeCycleDays;
                NextDate += WipeCycleDays;
            }

            NextWipeDate = NextWipeDate.AddDays(NextDay);
            var NextWipeDateReadable = $"{NextWipeDate.ToShortDateString()} - {NextWipeDate.ToShortTimeString()} UTC";

            // calculate next wipe time
            var TimeNow = new TimeSpan(DateNow.Hour, DateNow.Minute, DateNow.Second);
            NextWipeTime = new TimeSpan(NextDay, config.WipeTimeHourUTC, config.WipeTimeMinuteUTC, 0);

            var TimeRemaining = NextWipeTime.Subtract(TimeNow);

            var TimeRemainingReadable = "";
            if (TimeRemaining.TotalHours < 24)
                TimeRemainingReadable = string.Format("{0}h {1:d2}m", Convert.ToInt32(TimeRemaining.TotalHours), TimeRemaining.Minutes);
            else TimeRemainingReadable = String.Format("{0}d {1:d2}h", TimeRemaining.Days, TimeRemaining.Hours);

            return string.Format("Next wipe will occur in {0} ({1})", TimeRemainingReadable, NextWipeDateReadable);
        }
        #endregion

        #region commands
        [ConsoleCommand("wipe")]
        private void ConsoleCmd(ConsoleSystem.Arg arg)
        {
            SendReply(arg, GetNextWipeMsg());
        }

        [ChatCommand("wipe")]
        private void ChatCmd(BasePlayer player, string command, string[] args)
        {
            SendReply(player, GetNextWipeMsg());
        }
        #endregion
    }
}