using gk_common.constants;

namespace gk_common.beans
{
    public class Attribute
    {
        public bool AckSuccess { get; set; }
        public InfoValueType ValueType { get; set; }
        
        //传感器的维度(参数个数)
        public int DataSize { get; set; }
        public ReportType ReportType { get; set; }
        
        //测试标志
        public bool IsTest { get; set; }
        
        //自记标志
        public bool IsHistory { get; set; }

        //补发标志
        public bool IsRepeat { get; set; }

        public bool Force { get; set; }

        public DigitType DigitType { get; set; }
    }
}