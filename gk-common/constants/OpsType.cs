namespace gk_common.constants
{
    public enum OpsType
    {
        ReadCommand = 0x04,
        ReadResponse = 0x05,
        WriteCommand = 0x06,
        WriteResponse = 0x07,
        ReportCommand = 0x0a,
        ReportResponse = 0x0b,
        ExeCommand = 0x14,
        ExeResponse = 0x15,
        NotifyCommand = 0x16
    }
}