using System;
using DotNetty.Buffers;
using dotnetty_server.beans;
using dotnetty_server.constants;

namespace dotnetty_server.utils
{
    public class ParseUtil
    {
        public static byte[] Split(IByteBuffer buffer)
        {
            var readableSize = buffer.ReadableBytes;
            if(readableSize==0){
                return null;
            }
            var starter = new byte[2];
            for(var i=0; i<readableSize-1; i++){
                starter[0] = buffer.GetByte(i);
                starter[1] = buffer.GetByte(i+1);
                if (!BytesUtil.ByteArrayEquals(starter, LaserDefault.Starter)) return null;
                    buffer.SetReaderIndex(i);
                    break;
            }
            //至少得有13个字节 帧头（2) + 长度(2) + 硬件类型(1) + MAC地址(6) + CRC校验(2)
            if(readableSize<13){
                return null;
            }
            byte[] srcLength = new byte[2];
            buffer.GetBytes(2, srcLength);
            int length = BytesUtil.Bytes2Int16(srcLength);

            //真正的包长度 = 帧头（2) + 长度(2) + 【长度值 硬件类型(1) + MAC地址(6) + 业务数据(n)】 + CRC校验(2)
            byte[] frame = null;
            int frameLength = length + 6; 
            if (readableSize >= frameLength)
            {
                frame = new byte[frameLength];
                buffer.ReadBytes(frame);
            }
            return frame;
        }



        public static BaseMessage parse(byte[] bytes)
        {
            try
            {
                IByteBuffer byteBuf = Unpooled.WrappedBuffer(bytes);
                //帧头
                var startSign = new byte[2];
                byteBuf.ReadBytes(startSign);

                //长度 = 硬件类型 + mac + 消息内容
                var srcLength = new byte[2];
                byteBuf.ReadBytes(srcLength);

                var length = BytesUtil.Bytes2Int16(srcLength);
                var contentLength = length - LaserDefault.HardwareTypeLength - LaserDefault.MacLength;

                var hardwareTypeCode = byteBuf.ReadByte();

                var srcMac = new byte[LaserDefault.MacLength];
                byteBuf.ReadBytes(srcMac);
                var mac = BytesUtil.BytesToHex(srcMac);
                var sn = String.Join("CL", mac);

                var srcContent = new byte[contentLength];
                byteBuf.ReadBytes(srcContent);
                var crc = new byte[2];
                byteBuf.ReadBytes(crc);
                byteBuf.Release();

                var buffer = Unpooled.WrappedBuffer(srcContent);
                byte protocolVersion = buffer.ReadByte();

                var srcSerial = new byte[2];
                buffer.ReadBytes(srcSerial);
                int serial = BytesUtil.Bytes2Int16(srcSerial);

                var srcFunction = new byte[2];
                buffer.ReadBytes(srcFunction);
                var function = BytesUtil.Bytes2Int16(srcFunction);
                var bizCode = function >> 12;
                var funcCode = function & 0xfff;
               

                var coreContent = new byte[srcContent.Length - 5];
                buffer.ReadBytes(coreContent);

                buffer.Release();

                // baseMessage.
                // baseMessage.SetFunctionType();
                // baseMessage.SetMac(mac);
                // baseMessage.SetSn(sn);
                // baseMessage.SetContent(coreContent);
                // baseMessage.SetCrc(crc);
                // baseMessage.SetRaw(bytes);
                // baseMessage.SetProtocolVersion((String.valueOf(protocolVersion)));
                // baseMessage.SetSerial(serial);
                // baseMessage.SetBusinessType(businessType);
                // baseMessage.SetFunctionType(functionType);
                return new BaseMessage()
                {
                    Mac = mac,
                    Sn = sn,
                    Content = coreContent,
                    Crc = crc,
                    Raw =  bytes,
                    Serial = serial,
                    FunctionType = funcCode,
                    BusinessType = bizCode,
                    ProtocolVersion = Convert.ToString(protocolVersion)
                    
                };
            }
            catch (Exception e)
            {
                // log.error("解析异常： bytes = {}", BytesUtil.bytes2HexWithBlank(bytes, true));
                // e.printStackTrace();
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}