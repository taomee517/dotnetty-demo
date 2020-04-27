using System;
using NUnit.Framework;

namespace gk_test
{
    public class Tests
    {
        private Tests _tests;
        
        [SetUp]
        public void Setup()
        {
            //基康心跳消息
            // var msg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
            _tests = new Tests();
        }

        [Test]
        public void Test1()
        {
            var timestamp = _tests.GetTimeStamp();
            Assert.IsTrue(timestamp>0);
            // Assert.Pass(timestamp>0);
        }


        public long GetTimeStamp()
        {
            var timeBase = new DateTime(2000,1,1);
            var localTimeBase = TimeZoneInfo.ConvertTime(timeBase, TimeZoneInfo.Local);
            var timespan = DateTime.Now - localTimeBase;
            var timestamp = Convert.ToInt32(timespan.TotalSeconds);
            Console.WriteLine(timestamp);
            return timestamp;
        }
    }
}