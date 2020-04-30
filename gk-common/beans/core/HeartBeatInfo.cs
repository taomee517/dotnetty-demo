// 创建人：李鸢
// 创建时间：2020/04/30 11:03

using gk_common.constants;

namespace gk_common.beans
{
    public class HeartBeatInfo
    {
        public HeartBeatType HeartBeatType { get; set; }
        
        public int KeepAliveDuration { get; set; }
    }
}