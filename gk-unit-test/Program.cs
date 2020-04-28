using System;
using DotNetty.Buffers;


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

            var bytes = new byte[] {254, 150, 118, 153};
            var fv = BytesToDouble(bytes);
            var fvStr = fv.ToString("0.00");
            Console.WriteLine("转成小数结果：{0}", fvStr);
        }


        private static float BytesToDouble(byte[] bytes)
        {
            var buffer = Unpooled.WrappedBuffer(bytes);
            return buffer.ReadFloat();
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