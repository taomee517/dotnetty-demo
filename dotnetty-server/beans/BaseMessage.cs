using dotnetty_server.constants;

namespace dotnetty_server.beans
{
    public class BaseMessage
    {
        public int HardwareType { get; set; }
        public string Mac { get; set; }
        public string Sn { get; set; }
        public string ProtocolVersion { get; set; }
        public int Serial { get; set; }
        public int BusinessType { get; set; }
        public int FunctionType { get; set; }
        public byte[] Content { get; set; }
        public byte[] Crc { get; set; }
        public byte[] Raw { get; set; }
    }
}