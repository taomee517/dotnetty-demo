using NUnit.Framework;

namespace gk_test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            //基康心跳消息
            var msg = "fe-00-00-00-01-00-02-3e-76-10-98-80-00-ff-00-1c-7f-fe-00-4a-26-38-a4-bd-00-03-00-0a-00-00-00-0c-00-00-00-00-00-00-00-02-00-00-00-1e-71-0f";
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}