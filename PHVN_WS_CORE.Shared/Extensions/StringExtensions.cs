namespace PHVN_WS_CORE.SHARED.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert string to Time Span
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultTime">Default is 5 mins</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string str, TimeSpan? defaultTimeSpan = null)
        {
            defaultTimeSpan = defaultTimeSpan ?? TimeSpan.FromMinutes(5);

            //Default value is 5 mins.
            if (string.IsNullOrEmpty(str))
                return TimeSpan.FromMinutes(5);

            string dimension = str.Substring(str.Length - 1);
            int totalTime;

            if (int.TryParse(dimension, out totalTime))
            {
                return TimeSpan.FromMinutes(totalTime);
            }

            //Default value is 5 mins.
            TimeSpan timeSpan = defaultTimeSpan.Value;

            string lft = str.Substring(0, str.Length - 1);
            int number;
            if (int.TryParse(lft, out number))
            {
                switch (dimension.ToUpper())
                {
                    case "H":
                        timeSpan = TimeSpan.FromHours(number);
                        break;
                    case "M":
                        timeSpan = TimeSpan.FromMinutes(number);
                        break;
                    default:
                        timeSpan = TimeSpan.FromSeconds(number);
                        break;
                }
            }

            return timeSpan;
        }
    }
}
