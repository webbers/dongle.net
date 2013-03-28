using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dongle.Resources;

namespace Dongle.System.Networking.Tcp
{
    public class TcpClientSync : IDisposable
    {
        private readonly ManualResetEvent _connectionResetEvent;
        private readonly ManualResetEvent _connectionTimeoutResetEvent;
        private IPAddress _remoteAddress;
        private int _remotePort;
        private int _timeout;
        private Socket _socket;
        private global::System.Net.Sockets.TcpClient _tcpClient;

        /// <summary>
        /// Handler de exception ocorrida
        /// </summary>
        public delegate void ErrorEventHandler(Socket socket, Exception exception);

        /// <summary>
        /// Evento disparado ao ocorrer uma exceção
        /// </summary>
        public event ErrorEventHandler OnErrorOcurred;

        public TcpClientSync()
        {
            _connectionResetEvent = new ManualResetEvent(false);
            _connectionTimeoutResetEvent = new ManualResetEvent(false);            
        }

        public void Connect(IPAddress remoteAddress, int remotePort, int connectionTimeout)
        {
            _remoteAddress = remoteAddress;
            _remotePort = remotePort;
            _timeout = connectionTimeout;

            if (!Connect())
            {
                throw new IOException(string.Format(DongleResource.TcpConnectionError, remoteAddress, remotePort));
            }

            _socket.Blocking = true;
            _tcpClient = new global::System.Net.Sockets.TcpClient { Client = _socket, SendTimeout = connectionTimeout, ReceiveTimeout = connectionTimeout };
        }

        public void Dispose()
        {
            Disconnect();
        }

        private bool Connect()
        {
            var result = false;
            IPEndPoint remoteEndPoint;
            try
            {
                remoteEndPoint = new IPEndPoint(_remoteAddress, _remotePort);
            }
            catch
            {
                var ipHostInfo = Dns.GetHostEntry(_remoteAddress);
                var ipAddress = ipHostInfo.AddressList[0];
                remoteEndPoint = new IPEndPoint(ipAddress, _remotePort);
            }

            _socket = _remoteAddress.AddressFamily == AddressFamily.InterNetworkV6 ?
                        new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) { Blocking = false } :
                        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { Blocking = false };
            _socket.BeginConnect(remoteEndPoint, OnConnect, _socket);
            if (!_connectionResetEvent.WaitOne(_timeout, false))
            {
                _connectionTimeoutResetEvent.Set();
                _socket.Close();
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Conexão efetuada
        /// </summary>        
        private void OnConnect(IAsyncResult ar) 
        {
            try
            {
                var client = (Socket)ar.AsyncState;
                if (client != null)
                {
                    client.EndConnect(ar);
                    _connectionResetEvent.Set();
                }
                else
                {
                    Disconnect();
                }                
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception exception)
            {
                if (_connectionTimeoutResetEvent.WaitOne(0, false))
                {
                    if (OnErrorOcurred != null)
                    {
                        OnErrorOcurred(_socket, exception);
                    }                   
                }
            }
        }

        /// <summary>
        /// Encerra a conexão
        /// </summary>
        private void Disconnect()
        {
            try
            {
                if(_tcpClient != null)
                {
                    _tcpClient.Close();
                }
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
            }
            _tcpClient = null;
            _socket = null;
        }

        /// <summary>
        /// Recebe uma linha do servidor
        /// </summary>
        /// <param name="maxEndDate"> </param>
        public string ReceiveCommandLine(DateTime maxEndDate)
        {
            try
            {
                while (!_tcpClient.Connected && DateTime.Now < maxEndDate)
                {
                    Connect();
                }
                if (!_tcpClient.Connected)
                {
                    return "NOT_CONNECTED";
                }
                var networkStream = _tcpClient.GetStream();
                var streamReader = new StreamReader(networkStream);
                return streamReader.ReadLine();
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
                return "";
            }                        
        }

        /// <summary>
        /// Envia uma linha de texto para o servidor
        /// </summary>        
        public void SendCommandLine(string commandLine)
        {
            var networkStream = _tcpClient.GetStream();
            var streamWriter = new StreamWriter(networkStream);
            streamWriter.WriteLine(commandLine);
            streamWriter.Flush();
        }
    }
}
