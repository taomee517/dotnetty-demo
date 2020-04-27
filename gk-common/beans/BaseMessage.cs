using DotNetty.Buffers;

namespace gk_common.beans
{
    public class BaseMessage
    {
        public BaseHeader Header { get; set; }

        public IByteBuffer Content { get; set; }
    }
}