// 创建人：李鸢
// 创建时间：2020/05/12 11:05

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NewLife.Data;
using NewLife.Net;

namespace dotnetty_echo.client
{
    public class TcpForward
    {
        private IPEndPoint _endPoint;

        private NetUri _netUri;
        private ISocketClient _socketClient;
//        private readonly ILogger<TcpForward> _logger;
//
//        public TcpForward(ILogger<TcpForward> logger)
//        {
//            _logger = logger;
//        }

        public bool Active()
        {
            return (_socketClient == null)?false:_socketClient.Active;
        }
        
        public void Dispose()
        {
            _socketClient.Dispose();
        }

        public Task SetEndPoint(IPEndPoint endPoint)
        {
            var reBind = false;
            if (endPoint != null)
            {
                Console.WriteLine($"设置连接点为 {endPoint.Address} {endPoint.Port}");
                _endPoint = endPoint;
                reBind = true;
            }

            if (endPoint != null && !Equals(_endPoint, endPoint))
            {
                _endPoint = endPoint;
                reBind = true;
            }

            if (_netUri == null || reBind)
            {
                _netUri = new NetUri(NetType.Tcp, _endPoint);
            }

            if (_socketClient == null || reBind)
            {
                _socketClient = _netUri.CreateRemote();
            }

//            if (_socketClient.Disposed)
//            {
//                _socketClient = new TcpSession(new Socket(SocketType.Stream,ProtocolType.Tcp));
//                _socketClient.Remote = _netUri;
//            }

            return Task.CompletedTask;
        }
        private DateTime _lastOpenTime = DateTime.MinValue;

        public Task Start(IPEndPoint endPoint)
        {
            if (_socketClient != null && _socketClient.Active && _lastOpenTime > DateTime.Now.AddMinutes(-1))
            {
                Console.WriteLine("数据转发连接在线不进行重连");
                return Task.CompletedTask;
            }

            try
            {
                // || _socketClient.Disposed
                if (_socketClient == null)
                {
                    SetEndPoint(endPoint);
                }

                _socketClient?.Open();
                _lastOpenTime = DateTime.Now;
                Console.WriteLine("数据转发重新连接");
            }
            catch (Exception e)
            {
                Console.WriteLine($"数据转发重新连接时发生错误:{endPoint.Address}:{endPoint.Port}{e.StackTrace}");
            }
            finally
            {

            }

            return Task.CompletedTask;
        }

        public Task Forward(byte[] data)
        {
            try
            {
                if (_socketClient != null && _socketClient.Active && _lastOpenTime > DateTime.Now.AddMinutes(-1))
                {
                    if (data != null && data.Any())
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Console.WriteLine("发送数据{0}", Encoding.Default.GetString(data));
                            _socketClient.Send(new Packet(data));
//                            _socketClient.SendMessage(data);
                        });
                    }
                }
                else
                {
                    Task.Factory.StartNew(() => { Start(_endPoint); });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"数据转发时发生错误:{e.StackTrace}");
            }

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _socketClient?.Close("关闭");
            return Task.CompletedTask;
        }
    }
}