// 创建人：taomee
// 创建时间：2020/06/03 10:20

using System;
using Lora_Common.Constant;

namespace Lora_Common.Beans
{
    public class Header
    {
        public int TaskId { get; set; }
        public int MsgSn { get; set; }
        public int Ttl { get; set; }
        public TransportType TransportType { get; set; }
        public DateTime Time { get; set; }
        public FunType FunType { get; set; }
        public string SrcMac { get; set; }
        public string DstMac { get; set; }
        public int Length { get; set; }
        public byte[] Content { get; set; }
        public byte[] crc { get; set; }
        
    }
}