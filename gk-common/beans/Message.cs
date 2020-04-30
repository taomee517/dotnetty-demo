using System.Collections.Generic;

namespace gk_common.beans
{
    public class Message
    {
        public BaseHeader Header { get; set; }
        public List<BaseBody> Bodies { get; set; }
    }
}