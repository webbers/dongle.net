using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dongle.Resources;

namespace Dongle.System.Networking.Tcp
{
    /// <summary>
    /// Servidor TCP
    /// </summary>
    public class TcpServer : TcpCommunication, IDisposable
    {
        /// <summary>
        /// Connexão
        /// </summary>
        public class Connection
        {
            /// <summary>
            /// Tamanho do buffer de comandos
            /// </summary>
            public const int CommandBufferSize = 4096;            

            /// <summary>
            /// Socket da conexão
            /// </summary>
            public Socket WorkSocket;

            /// <summary>
            /// Buffer para comandos
            /// </summary>
            public byte[] CommandBuffer = new byte[CommandBufferSize];
            
            /// <summary>
            /// Último comando recebido
            /// </summary>
            public string PreviousCommand;
        }

        private bool _forceStop;
        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        private int _portNumber;
        private IPAddress _ipAddress;
        private int _backlogSize;
        private Thread _serverThread;

        /// <summary>
        /// Servidor está em execução
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Lista de conexões
        /// </summary>
        public readonly ConcurrentDictionary<Guid, Connection> ConnectionList = new ConcurrentDictionary<Guid, Connection>();

        /// <summary>
        /// Evento executado ao enviar um comando
        /// </summary>
        public event CommandSentEventHandler OnCommandSent;

        /// <summary>
        /// Handler de arquivo enviado
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="fileData"></param>
        public delegate void FileSentToClientEventHandler(Guid connectionId, byte[] fileData);

        /// <summary>
        /// Evento disparado ao término do envio de um arquivo
        /// </summary>
        public event FileSentToClientEventHandler OnFileSent;

        /// <summary>
        /// Handler de comando recebido
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="command"></param>
        public delegate void CommandReceivedFromClientEventHandler(Guid connectionId, string command);

        /// <summary>
        /// Evento disparado ao receber um comando
        /// </summary>
        public event CommandReceivedFromClientEventHandler OnCommandReceived;

        /// <summary>
        /// Handler de arquivo recebido
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="fileData"></param>
        public delegate void FileReceivedFromClientEventHandler(Guid connectionId, byte[] fileData);

        /// <summary>
        /// Evento disparado ao receber um arquivo do servidor
        /// </summary>
        public event FileReceivedFromClientEventHandler OnFileReceived;

        /// <summary>
        /// Handler de cliente conectado
        /// </summary>
        /// <param name="connectionId"></param>
        public delegate void ClientConnectedEventHandler(Guid connectionId);

        /// <summary>
        /// Evento disparado ao conectar a um cliente
        /// </summary>
        public event ClientConnectedEventHandler OnClientConnected;

        /// <summary>
        /// Handler de cliente conectado
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="endPoint"></param>
        public delegate void ClientDisconnectedEventHandler(Guid connectionId, EndPoint endPoint);

        /// <summary>
        /// Evento disparado ao disconectar de um cliente
        /// </summary>
        public event ClientDisconnectedEventHandler OnClientDisconnected;

        /// <summary>
        /// Handler de exception ocorrida
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="exception"></param>
        public delegate void ErrorEventHandler(Socket socket, Exception exception);

        /// <summary>
        /// Evento disparado ao ocorrer uma exceção
        /// </summary>
        public new event ErrorEventHandler OnErrorOcurred;

        /// <summary>
        /// Handler de servidor iniciado
        /// </summary>
        public delegate void ServerStartedEventHandler();

        /// <summary>
        /// Evento disparado ao iniciar o servidor
        /// </summary>
        public event ServerStartedEventHandler OnStart;
       
        /// <summary>
        /// Construtor
        /// </summary>
        public TcpServer()
        {
            OnSocketDisconnected += ClientDisconnected;
        }

        private void ClientDisconnected(Socket socket)
        {
            foreach (var connection in ConnectionList)
            {
                if (connection.Value.WorkSocket == socket)
                {
                    CloseConnection(connection.Key);
                }
            }
        }        

        /// <summary>
        /// Inicia o servidor no IP e porta fornecidos
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="ipAddress"></param>
        /// <param name="backlogSize"></param>
        public void Start(int portNumber = 56000, IPAddress ipAddress = null, int backlogSize = 100)
        {
            if (ipAddress == null)
            {
                ipAddress = IPAddress.IPv6Any;
            }

            _portNumber = portNumber;
            _ipAddress = ipAddress;
            _backlogSize = backlogSize;

            _serverThread = new Thread(StartServerThread) {Name = "TcpServer." + _ipAddress + ".At." + _portNumber};
            _serverThread.Start();
        }

        private void StartServerThread()
        {           
            var socket = _ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ?
                        new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) :
                        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
            }
            catch (Exception)
            {
                Debug.WriteLine("TcpServer: Maquina provavelmente nao tem IPV6 ativado...");
            }            
            try
            {
                socket.Bind(new IPEndPoint(_ipAddress, _portNumber));
                socket.Listen(_backlogSize);
                Running = true;
            }
            catch (Exception exception)
            {
                Running = false;
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(socket, new Exception(DongleResource.TcpServerStartError, exception));   
                }                
                return;
            }
            if (OnStart != null)
            {
                OnStart();
            }

            while (!_forceStop)
            {
                _allDone.Reset();
                socket.BeginAccept(asyncResult =>
                {
                    _allDone.Set();
                    var connectionId = Guid.NewGuid();
                    try
                    {
                        var workSocket = socket.EndAccept(asyncResult);
                        ConnectionList[connectionId] = new Connection { WorkSocket = workSocket };
                        if (OnClientConnected != null)                        
                            OnClientConnected(connectionId);                        
                        WaitForCommand(connectionId);
                    }
                    catch (ObjectDisposedException)
                    {
                        if (OnClientDisconnected != null)
                            OnClientDisconnected(connectionId, null);
                    }
                    catch (Exception exception)
                    {
                        if (OnErrorOcurred != null)
                            OnErrorOcurred(socket, exception);
                    }
                }, null);
                _allDone.WaitOne();
            }
            socket.Close();
        }

        /// <summary>
        /// Para o servidor
        /// </summary>
        public void Stop()
        {
            _forceStop = true;
            Running = false;
            _allDone.Set();
        }

        /// <summary>
        /// Encerra uma conexão
        /// </summary>
        /// <param name="connectionId"></param>
        public void CloseConnection(Guid connectionId)
        {
            if (!ConnectionList.ContainsKey(connectionId))
            {
                return;
            }
            var connection = ConnectionList[connectionId];            
            var endPoint = connection.WorkSocket.RemoteEndPoint;
            connection.WorkSocket.Close();            
            bool removed;
            do
            {
                removed = ConnectionList.TryRemove(connectionId, out connection);
            } while (!removed);

            if (OnClientDisconnected != null)
            {
                OnClientDisconnected(connectionId, endPoint);
            }            
        }

        /// <summary>
        /// Envia um comando ao cliente
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="command"></param>
        /// <param name="closeConnection"></param>
        /// <returns>False caso a conexão especificada não seja encontrada</returns>
        public bool SendCommand(Guid connectionId, string command, bool closeConnection = false)
        {
            if (!ConnectionList.ContainsKey(connectionId))
            {
                return false;
            }
            SendCommand(ConnectionList[connectionId].WorkSocket, command, OnCommandSent, closeConnection);
            return true;
        }

        /// <summary>
        /// Envia um arquivo ao cliente
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="fileData"></param>
        /// <param name="offset"></param>
        /// <param name="bufferSize"></param>
        /// <returns>False caso a conexão especificada não seja encontrada</returns>
        public bool SendFile(Guid connectionId, byte[] fileData, int offset = 0, int bufferSize = DefaultBufferSize)
        {
            if (!ConnectionList.ContainsKey(connectionId))
            {
                return false;
            }
            SendFile(ConnectionList[connectionId].WorkSocket, () =>
                                                                  {
                                                                      if (OnFileSent != null)
                                                                      {
                                                                          OnFileSent(connectionId, fileData);
                                                                      }
                                                                  }, fileData, offset, bufferSize);
            return true;
        }

        /// <summary>
        /// Aguarda por um comando do cliente
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>False caso a conexão especificada não seja encontrada</returns>
        public bool WaitForCommand(Guid connectionId)
        {
            if (!ConnectionList.ContainsKey(connectionId))
            {
                return false;
            }

            var connection = ConnectionList[connectionId];
            connection.CommandBuffer = new byte[Connection.CommandBufferSize];
            WaitForCommand(connection.WorkSocket, command =>
            {
                if (OnCommandReceived != null)
                {
                    try
                    {
                        OnCommandReceived(connectionId, command);                    
                    }
                    catch (ThreadAbortException)
                    {
                        //Tread abortada. Nada a fazer
                    }
                    catch (Exception exception)
                    {
                        if (OnErrorOcurred != null)
                        {
                            OnErrorOcurred(connection.WorkSocket, exception);
                        }                        
                    }                    
                }                
                
                if (!ConnectionList.ContainsKey(connectionId))
                {
                    return;
                }
                ConnectionList[connectionId].PreviousCommand = command;
            }, connection.CommandBuffer);
            return true;
        }

        /// <summary>
        /// Aguarda por um arquivo do cliente
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="fileSize"></param>
        /// <param name="bufferSize"></param>
        /// <param name="previousPart"></param>
        /// <returns>False caso a conexão especificada não seja encontrada</returns>
        public bool WaitForFile(Guid connectionId, int fileSize, int bufferSize = DefaultBufferSize, byte[] previousPart = null)
        {
            if (!ConnectionList.ContainsKey(connectionId))
            {
                return false;
            }
            WaitForFile(ConnectionList[connectionId].WorkSocket, fileData => OnFileReceived(connectionId, fileData), fileSize, bufferSize, previousPart);
            return true;
        }

        public void Dispose()
        {
            _allDone.Dispose();
        }
    }
}
