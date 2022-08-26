using PHVN_WS_CORE.SHARED.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PHVN_WS_CORE.SHARED.Configurations
{
    public class AppSettings
    {
        public string AppId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RollingInterval { get; set; }
        public string SleepingInterval { get; set; }

        public DateTime StartTimestamp {
            get
            {
                var splitStr = !string.IsNullOrEmpty(StartTime) ? StartTime : null;

                if (splitStr == null)
                    return DateTime.Now;

                return DateTime.Parse(splitStr + ":00");
            }
        }
        public DateTime EndTimestamp
        {
            get
            {
                var splitStr = !string.IsNullOrEmpty(EndTime) ? EndTime : null;

                if (splitStr == null)
                    return  DateTime.Now;

                return DateTime.Parse(splitStr + ":00");
            }
        }

        public TimeSpan RollingInvervalConvert
        {
            get
            {
                return RollingInterval.ToTimeSpan();
            }
        }

        public TimeSpan SleepingIntervalConvert
        {
            get
            {
                return SleepingInterval.ToTimeSpan();
            }
        }
    }
}
