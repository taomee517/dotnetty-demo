// 创建人：taomee
// 创建时间：2020/06/02 15:46

using System;

namespace Lora_Common.Util
{
    public class TimestampUtil
    {
        public static DateTime ConvertSeconds2DateTime(long d)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(d + "0000000");
            var toNow = new TimeSpan(lTime);
            var dtResult = dtStart.Add(toNow);
            return dtResult;
        }
        
        
        public static int ConvertDateTime2Seconds(DateTime dateTime)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var offset = dateTime - dtStart;
            return Convert.ToInt32(offset.Ticks / 10000000);
        }

        public static int GetUtcSecondsStamp()
        {
            var utc = DateTime.Now.AddHours(-8);
            return ConvertDateTime2Seconds(utc);
        }
    }
}