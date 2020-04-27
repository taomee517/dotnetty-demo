namespace gk_common.beans
{
    public class BaseHeader
    {
        public byte Head { get; set; }
        public string Dst { get; set; }

        public string Src { get; set; }
        public int Ver { get; set; }
        public Control Control { get; set; }
        public int Send { get; set; }
        public int Receive { get; set; }
        public ushort Length { get; set; }
    }
}