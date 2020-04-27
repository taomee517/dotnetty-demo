using System;
using gk_common.constants;

namespace gk_common.beans
{
    public class BaseBody
    {
        public IdType IdType  { get; set; }

        public DateTime Time { get; set; }

        public MsgType MsgType { get; set; }

        public OpsType OpsType { get; set; }

        public Attribute Attribute { get; set; }

        public ushort Length { get; set; }

        public int SerialNumber { get; set; }

        public byte[] CoreMsg { get; set; }

        public HeartBeatType HeartBeatType { get; set; }
        
        public int KeepAliveDuration { get; set; }


    }
}