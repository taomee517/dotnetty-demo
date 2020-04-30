namespace gk_common.constants
{
    public enum IdType
    {
        //读设备状态
        ReadDeviceStatus = 0x7ffe0041,
        
        //报设备状态
        ReportDeviceStatus = 0x7ffe005e,
        
        //蓄电池信息
        BatteryInfo = 0x7ffe0042,
        
        //读自记状态
        ReadCacheStatus = 0x7ff30043,
        
        //读设备基本信息
        ReadDeviceInfo = 0x7ffe0048,
        
        //上下线，心跳
        HeartBeat = 0x7ffe004a,
        
        //设备唯一识别码
        DeviceUniqueCode = 0x7ffe0047,
        
        //时间校准
        TimeAdjust = 0x7ffe0044,
        
        //应用服务器地址（1-主）
        ApplyMasterServerAddress = 0x7ffe0800,
        
        //应用服务器地址（2-副）
        ApplySlaveServerAddress = 0x7ffe0801,
        
        //承载服务器地址（1-主）
        PayloadMasterServerAddress = 0x7ffe0810,
        
        //承载服务器地址（2-副）
        PayloadSlaveServerAddress = 0x7ffe0811,
        
        //传感器数据
        SensorData = 0x7ffe1d00,
        
        //历史数据-自记数据
        HistoryData = 0x7ffe0059,
        
        //测量命令
        MeasureOrder = 0x7ffe0050,
        
        //文件下载
        FileDownload = 0x7ffe0055,
        
        //设备工作策略
        DeviceWorkingPolicy = 0x7ffe2ff0,
        
        //异常信息
        DeviceException = 0x7ffefffa
        
        
        
        
        
        
    }
}