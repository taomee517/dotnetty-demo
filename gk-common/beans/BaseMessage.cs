using DotNetty.Buffers;

namespace gk_common.beans
{
    public class BaseMessage
    {
        public byte Head { get; set; }
        public string Dst { get; set; }

        public string Src { get; set; }
        public int Ver { get; set; }
        public string Control { get; set; }
        public int Send { get; set; }
        public int Receive { get; set; }
        public ushort Length { get; set; }

        public IByteBuffer Content { get; set; }
    }
}