using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Dongle.System.IO;

namespace Dongle.System.Networking.Tcp
{
    /// <summary>
    /// Classe base para cliente e servidor TCP
    /// </summary>
    public class TcpCommunication
    {
        /// <summary>
        /// Tamanho padrão do buffer de transferência do socket
        /// </summary>
        public const int DefaultBufferSize = 65536;

        /// <summary>
        /// Handler de arquivo enviado
        /// </summary>
        public delegate void FileSentEventHandler();
        
        /// <summary>
        /// Handler de comando enviado
        /// </summary>
        /// <param name="command"></param>
        /// <param name="bytesSent"></param>
        public delegate void CommandSentEventHandler(string command, long bytesSent);

        /// <summary>
        /// Handler de comando recebido
        /// </summary>
        /// <param name="command"></param>
        public delegate void CommandReceivedEventHandler(string command);

        /// <summary>
        /// Handler de arquivo recebido
        /// </summary>
        /// <param name="fileData"></param>
        public delegate void FileReceivedEventHandler(byte[] fileData);

        /// <summary>
        /// Handler de socket disconectado
        /// </summary>
        /// <param name="socket"></param>
        public delegate void SocketDisconnectedEventHandler(Socket socket);

        /// <summary>
        /// Handler de erro
        /// </summary>
        public delegate void ErrorOcurredEventHandler(Exception exception);

        /// <summary>
        /// Evento executado na ocorrência de algum erro
        /// </summary>
        public event ErrorOcurredEventHandler OnErrorOcurred;

        /// <summary>
        /// Evento executado quando o host é desconectado
        /// </summary>
        public event SocketDisconnectedEventHandler OnSocketDisconnected;

        /// <summary>
        /// Está enviando dados para o host
        /// </summary>
        public bool IsSending { get; private set; }

        protected void SendCommand(Socket socket, string command, CommandSentEventHandler callback, bool closeConnection = false)
        {
            try
            {
                Wait();
                var byteData = DongleEncoding.Default.GetBytes(command);
                socket.BeginSend(byteData, 0, byteData.Length, 0, asyncResult =>
                {
                    int bytesSent;
                    try
                    {
                        bytesSent = socket.EndSend(asyncResult);
                    }
                    catch (Exception exception)
                    {
                        if (OnErrorOcurred != null)
                        {
                            OnErrorOcurred(exception);
                        }
                        if (OnSocketDisconnected != null)
                        {
                            OnSocketDisconnected(socket);
                        }
                        return;
                    }

                    IsSending = false;

                    if (callback != null)
                    {
                        callback(command, bytesSent);
                    }
                    if (closeConnection)
                    {
                        try
                        {
                            socket.Disconnect(false);
                            if (OnSocketDisconnected != null)
                            {
                                OnSocketDisconnected(socket);
                            }
                        }
                        catch (Exception exception)
                        {
                            if (OnErrorOcurred != null)
                            {
                                OnErrorOcurred(exception);
                            }
                        }                        
                    }
                }, null);
            }
            catch (Exception)
            {                
                if (OnSocketDisconnected != null)
                {
                    OnSocketDisconnected(socket);
                }
            }            
        }

        protected void SendFile(Socket socket, FileSentEventHandler callback, byte[] fileData, int offset = 0, int bufferSize = DefaultBufferSize)
        {
            Wait();
            var sendingLength = bufferSize;
            if (offset + bufferSize > fileData.Length)
            {
                sendingLength = fileData.Length - offset;
            }
            socket.BeginSend(fileData, offset, sendingLength, 0, asyncResult =>
            {
                try
                {
                    socket.EndSend(asyncResult);
                }
                catch (Exception exception)
                {
                    if (OnErrorOcurred != null)
                    {
                        OnErrorOcurred(exception);
                    }
                    if (OnSocketDisconnected != null)
                    {
                        OnSocketDisconnected(socket);
                    }
                    return;
                }                

                if (offset + sendingLength < fileData.Length)
                {
                    SendFile(socket, callback, fileData, offset + bufferSize, bufferSize);
                    return;
                }

                IsSending = false;

                if (callback != null)
                {
                    callback();
                }
            }, null);
        }

        private void Wait()
        {
            var i = 0;
            while (IsSending && i < 20)
            {
                i++;
                Thread.CurrentThread.Join(100);
            }
            IsSending = true;
        }

        protected void WaitForCommand(Socket socket, CommandReceivedEventHandler callback, byte[] commandBuffer, int commandBufferSize = 1024)
        {
            if (commandBuffer == null)
            {
                commandBuffer = new byte[commandBufferSize];
            }
            try
            {
                socket.BeginReceive(commandBuffer, 0, commandBufferSize, 0,
                                        asyncResult =>
                                        {
                                            int bytesRead;
                                            try
                                            {
                                                bytesRead = socket.EndReceive(asyncResult);                                                
                                            }
                                            catch (Exception exception)
                                            {
                                                if (OnErrorOcurred != null)
                                                {
                                                    OnErrorOcurred(exception);
                                                }
                                                if (OnSocketDisconnected != null)
                                                {
                                                    OnSocketDisconnected(socket);
                                                }
                                                return;
                                            }
                                            if (bytesRead <= 0)
                                            {
                                                if (OnSocketDisconnected != null)
                                                {
                                                    OnSocketDisconnected(socket);
                                                }
                                                return;
                                            }
                                            var command = DongleEncoding.Default.GetString(commandBuffer, 0, bytesRead);
                                            if (callback != null)
                                            {
                                                try
                                                {
                                                    callback(command);
                                                }
                                                catch (ThreadAbortException)
                                                {
                                                    //Treadh abortada. Nada a fazer
                                                }
                                                catch (Exception exception)
                                                {
                                                    if (OnErrorOcurred != null)
                                                    {
                                                        OnErrorOcurred(exception);
                                                    }
                                                    else
                                                    {
                                                        throw;
                                                    }
                                                }                                                
                                            }
                                        },
                                     null);
            }
            catch (Exception)
            {
                if (OnSocketDisconnected != null)
                {
                    OnSocketDisconnected(socket);
                }
            }
            
        }

        protected void WaitForFile(Socket socket, FileReceivedEventHandler fileReceivedCallback, int fileSize, int bufferSize = DefaultBufferSize, byte[] previousPart = null, int currentPart = 0, int totalParts = 1)
        {
            if (currentPart == 0)
            {
                totalParts = (int)Math.Ceiling(Convert.ToDouble(fileSize) / Convert.ToDouble(bufferSize));
            }
            var buffer = new byte[bufferSize];            
            try
            {
                SocketError errorCode;
                socket.BeginReceive(buffer, 0, bufferSize, 0, out errorCode, asyncResult =>
                {
                    byte[] newPart;
                    Debug.WriteLine("RECEIVING FILE " + currentPart + "/" + totalParts);
                    var bufferRealSize = buffer.Length;
                    if(fileSize <= bufferSize)
                    {
                        bufferRealSize = fileSize;
                    }                    
                    else if(currentPart == totalParts - 1)
                    {
                        bufferRealSize = fileSize - (currentPart*bufferSize);
                    }
                    Array.Resize(ref buffer, bufferRealSize);

                    if (buffer.Length == 0 && previousPart == null)
                    {
                        newPart = null;
                    }
                    else if (buffer.Length == 0 && previousPart != null)
                    {
                        newPart = previousPart;
                    }
                    else if (previousPart != null)
                    {
                        var newPartLength = previousPart.Length + buffer.Length;
                        var bufferLength = buffer.Length;
                        if (newPartLength > fileSize)
                        {
                            newPartLength = fileSize;
                            bufferLength = fileSize - previousPart.Length;
                        }
                        newPart = new byte[newPartLength];
                        Buffer.BlockCopy(previousPart, 0, newPart, 0,
                                         previousPart.Length);
                        Buffer.BlockCopy(buffer, 0, newPart, previousPart.Length,
                                         bufferLength);
                    }
                    else
                    {
                        newPart = buffer;
                    }

                    if (newPart == null || newPart.Length < fileSize)
                    {
                        WaitForFile(socket, fileReceivedCallback, fileSize, bufferSize, newPart, currentPart + 1, totalParts);
                        return;
                    }

                    try
                    {
                        socket.EndReceive(asyncResult);
                    }
                    catch (Exception exception)
                    {
                        if (OnErrorOcurred != null)
                        {
                            OnErrorOcurred(exception);
                        }
                        if (OnSocketDisconnected != null)
                        {
                            OnSocketDisconnected(socket);
                        }
                        return;
                    }

                    if (fileReceivedCallback != null)
                    {
                        fileReceivedCallback(newPart);
                    }
                }, null);
            }
            catch (Exception exception)
            {
                if (OnErrorOcurred != null)
                {
                    OnErrorOcurred(exception);
                }
                if (OnSocketDisconnected != null)
                {
                    OnSocketDisconnected(socket);
                }
            }
            
        }
    }
}
