using System;

namespace gk_unit_test
{
    class Program
    {
        static void Main(string[] args)
        {
            var timestamp = GetTimeStamp();
            Console.WriteLine(timestamp);
            var target = BuildTime(timestamp);
            Console.WriteLine(target);
        }


        public static long GetTimeStamp()
        {
            var timeBase = new DateTime(2000,1,1);
            var localTimeBase = TimeZoneInfo.ConvertTime(timeBase, TimeZoneInfo.Local);
            var timespan = DateTime.Now - localTimeBase;
            var timestamp = Convert.ToInt32(timespan.TotalSeconds);
            return timestamp;
        }


        public static DateTime BuildTime(long gkTimeStamp)
        {
            var timeBase = TimeZoneInfo.ConvertTime(new DateTime(2000, 1, 1), TimeZoneInfo.Local);
            var timestamp = (gkTimeStamp * 10000000L);
            var timeOffset = new TimeSpan(timestamp);
            var targetTime = timeBase.Add(timeOffset);
            return targetTime;
        }
    }
}