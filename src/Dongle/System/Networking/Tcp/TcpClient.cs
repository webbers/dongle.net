using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dongle.Resources;

namespace Dongle.System.Networking.Tcp
{
    /// <summary>
    /// Cliente TCP
    /// </summary>
    public class TcpClient : TcpCommunication, IDisposable
    {
        private readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
        
        /// <summary>
        /// Handler utilizado ao conectar com um host
        /// </summary>
        public delegate void ConnectEventHandler(bool successfull);

        /// <summary>
        /// Evento disparado ao conectar com um host
        /// </summary>
        public event ConnectEventHandler OnConnect;

        /// <summary>
        /// Evento disparado ao receber um comando
        /// </summary>
        public event CommandReceivedEventHandler OnCommandReceived;

        /// <summary>
        /// Evento disparado ao enviar um comando
        /// </summary>
        public event CommandSentEventHandler OnCommandSent;

        /// <summary>
        /// Evento disparado ao receber um arquivo
        /// </summary>
        public event FileReceivedEventHandler OnFileReceived;

        /// <summary>
        /// Evento disparado ao enviar um arquivo
        /// </summary>
        public event FileSentEventHandler OnFileSent;

        /// <summary>
        /// Handler utilizado ao disconectar do client
        /// </summary>
        public delegate void ClientDisconnectedEventHandler();

        /// <summary>
        /// Evento disparado ao disconectar do client
        /// </summary>
        public event ClientDisconnectedEventHandler OnClientDisconnected;

        /// <summary>
        /// Handler de exception ocorrida
        /// </summary>
        public delegate void ErrorEventHandler(Socket socket, Exception exception);

        /// <summary>
        /// Evento disparado ao ocorrer uma exceção
        /// </summary>
        public new event ErrorEventHandler OnErrorOcurred;

        /// <summary>
        /// Endereço do host remoto
        /// </summary>
        public IPAddress RemoteAdress { get; private set; }

        /// <summary>
        /// Porta do host remoto
        /// </summary>
        public int RemotePort { get; private set; }

        private Socket _socket;
        
        /// <summary>
        /// Informa se o socket está conectado
        /// </summary>
        public bool IsConnected
        {
            get { return _socket != null && _socket.Connected; }
        }

        private bool _closing;

        /// <summary>
        /// Construtor
        /// </summary>
        public TcpClient()
        {
            RemoteAdress = IPAddress.Loopback;
            RemotePort = 56000;

            OnSocketDisconnected += socket =>
                                        {
                                            Disconnect();
                                            if (OnClientDisconnected != null)
                                            {
                                                OnClientDisconnected();
                                            }
                                        };
        }

        /// <summary>
        /// Verifica se a conexão está ativa. Caso não esteja, tenta reconectar.
        /// </summary>
        private void CheckConnection()
        {
            if (!IsConnected)
            {
                Connect(RemoteAdress, RemotePort);
                if (!IsConnected)
                {
                    throw new Exception(DongleResource.TcpConnectionClosedError);
                }
            }
        }

        /// <summary>
        /// Conecta ao servidor especificado pelo IP e porta fornecidos
        /// </summary>
        public void Connect(IPAddress remoteAdress, int remotePort, int timeout = -1)
        {
            if (_socket!= null && IsConnected)
            {
                Disconnect();
            }
            RemoteAdress = remoteAdress;
            RemotePort = remotePort;
            _socket = remoteAdress.AddressFamily == AddressFamily.InterNetworkV6 ? 
                        new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) : 
                        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (timeout != -1)
            {
                _socket.ReceiveTimeout = timeout;
                _socket.SendTimeout = timeout;
            }
            try
            {
                _socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);            
            }
            catch (Exception)
            {
                Debug.WriteLine("TcpClient: Maquina provavelmente nao tem IPV6 ativado...");
            }              
            try
            {
                IPEndPoint remoteEndPoint;
                try
                {
                    remoteEndPoint = new IPEndPoint(remoteAdress, remotePort);
                }
                catch
                {
                    var ipHostInfo = Dns.GetHostEntry(remoteAdress);
                    var ipAddress = ipHostInfo.AddressList[0];
                    remoteEndPoint = new IPEndPoint(ipAddress, remotePort);
                }
                _socket.BeginConnect(remoteEndPoint, asyncResult =>
                {
                    var success = true;
                    try
                    {
                        if (_socket.Connected)
                        {
                            _socket.EndConnect(asyncResult);   
                        }
                        else
                        {
                            Disconnect();
                        }
                    }
                    catch (Exception exception)
                    {
                        success = false;
                        if (OnErrorOcurred != null)
                        {
                            OnErrorOcurred(_socket, exception);
                        }
                    }
                    if (OnConnect != null)
                    {
                        OnConnect(success);
                        if (!success)
                        {
                            return;
                        }
                    }
                    _connectDone.Set();
                }, null);
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
            }
            
            if (_closing)
                return;
            _connectDone.WaitOne();
        }

        /// <summary>
        /// Encerra a conexão
        /// </summary>
        /// <param name="reuseSocket">True para reutilizar o socket</param>
        public void Disconnect(bool reuseSocket = true)
        {
            try
            {
                if (IsConnected)
                {
                    try
                    {
                        _socket.Disconnect(reuseSocket);
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch
                    {
                        //Nada a fazer
                    }
                    // ReSharper restore EmptyGeneralCatchClause
                }
                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected();
                }
                if (!reuseSocket)
                {
                    _closing = true;                    
                    _socket.Dispose();
                    _socket = null;
                    _connectDone.Set();
                }                
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
            }  
        }

        /// <summary>
        /// Envia um comando para o servidor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="closeConnection"></param>
        public void SendCommand(string command, bool closeConnection = false)
        {
            CheckConnection();
            SendCommand(_socket, command, OnCommandSent, closeConnection);
        }
        
        /// <summary>
        /// Envia um arquivo para o servidor
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="offset"></param>
        /// <param name="bufferSize"></param>
        public void SendFile(byte[] fileData, int offset = 0, int bufferSize = DefaultBufferSize)
        {
            CheckConnection();
            SendFile(_socket, OnFileSent, fileData, offset, bufferSize);
        }

        /// <summary>
        /// Aguarda por um comando do servidor
        /// </summary>
        public void WaitForCommand()
        {
            CheckConnection();
            try
            {
                WaitForCommand(_socket, OnCommandReceived, null);            
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
                Disconnect();
            }            
        }

        /// <summary>
        /// Aguarda por um comando do servidor
        /// </summary>
        public void WaitForCommand(int size)
        {
            CheckConnection();
            try
            {
                WaitForCommand(_socket, OnCommandReceived, null, size);
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(_socket, exception);
                }
                Disconnect();
            }
        }

        /// <summary>
        /// Aguarda por um arquivo do servidor
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="bufferSize"></param>
        /// <param name="previousPart"></param>
        public void WaitForFile(int fileSize, int bufferSize = DefaultBufferSize, byte[] previousPart = null)
        {
            CheckConnection();
            WaitForFile(_socket, fileData => OnFileReceived(fileData), fileSize, bufferSize, previousPart);            
        }

        public void Dispose()
        {
            _connectDone.Dispose();
            _socket.Dispose();
        }
    }
}
