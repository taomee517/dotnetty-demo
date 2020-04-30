// 创建人：李鸢
// 创建时间：2020/04/30 10:51

using System.Collections.Generic;

namespace gk_common.beans
{
    public class SensorData
    {
        public double sensorDetectData;
        public double temp;
        public List<double> UnparsedData  { get; set; }
    }
}