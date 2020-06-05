// 创建人：taomee
// 创建时间：2020/06/02 15:19

using System.Collections.Concurrent;
using System.Threading;

namespace Lora_Common.Util
{
    public class SearialUtil
    {
        private static ConcurrentDictionary<string,int> SerialMap = new ConcurrentDictionary<string, int>();

        public static int GetSerial(string key)
        {
            var serial = 0;
            if (!SerialMap.ContainsKey(key))
            {
                SerialMap[key] = serial;
                return serial;
            }
            var temp = SerialMap[key];
            serial = Interlocked.Increment(ref temp)% 0x100;
            SerialMap[key] = serial;
            return serial;
        }
    }
}