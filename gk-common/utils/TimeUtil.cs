using System;

namespace gk_common.utils
{
    public class TimeUtil
    {
        private static DateTime GetTimeBase()
        {
            var timeBase = new DateTime(2000,1,1);
            var localTimeBase = TimeZoneInfo.ConvertTime(timeBase, TimeZoneInfo.Local);
            return localTimeBase;
        }
        
        
        public static Int32 GetTimeStamp()
        {
            var localTimeBase = GetTimeBase();
            var timespan = DateTime.Now - localTimeBase;
            var timestamp = Convert.ToInt32(timespan.TotalSeconds);
            return timestamp;
        }


        public static DateTime BuildTime(long gkTimeStamp)
        {
            var timeBase = GetTimeBase();
            var timestamp = (gkTimeStamp * 10000000L);
            var timeOffset = new TimeSpan(timestamp);
            var targetTime = timeBase.Add(timeOffset);
            return targetTime;
        }
    }
}