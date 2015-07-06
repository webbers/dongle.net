using System;
using System.IO.Pipes;
using System.Threading;

namespace Dongle.System.Networking.NamedPipes
{
    /// <summary>
    /// Servidor multi-thread para conexões via pipe nomeado
    /// </summary>
    public class PipeServer
    {
        public delegate void ErrorOcurredEventHandler(Exception exception);

        public delegate void ClientConnectedEventHandler(StringStream stream);

        public event ErrorOcurredEventHandler OnErrorOcurred;

        public event ClientConnectedEventHandler OnClientConnected;        
        
        public string PipeName { get; private set; }
        
        private bool _stopRequired;
        private Thread _runningThread;
        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        
        public PipeServer(string pipeName)
        {
            PipeName = pipeName;
        }

        public void Start()
        {
            _stopRequired = true;
            _runningThread = new Thread(ServerLoop) { Name = "PipeServer." + PipeName };
            _runningThread.Start();
        }

        public void Stop()
        {
            _stopRequired = false;
            _allDone.Set();
        }

        public void Pause()
        {
            Stop();
        }

        public void Continue()
        {
            Start();
        }

        private void ServerLoop()
        {
            while (_stopRequired)
            {
                _allDone.Reset();
                var pipeStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, -1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);                
                pipeStream.BeginWaitForConnection(asyncResult =>
                                                      {                                                          
                                                          pipeStream.EndWaitForConnection(asyncResult);
                                                          _allDone.Set();
                                                          var thread = new Thread(ProcessClientThread);
                                                          thread.Start(pipeStream);
                                                      }, null);
                _allDone.WaitOne();
            }
        }

        private void ProcessClientThread(object o)
        {
            using (var pipeStream = (NamedPipeServerStream)o)
            {
                try
                {
                    using (var stringStream = new StringStream(pipeStream))
                    {
                        if (OnClientConnected != null)
                        {
                            OnClientConnected(stringStream);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (OnErrorOcurred != null)
                    {
                        OnErrorOcurred(exception);
                    }
                }
                finally
                {
                    if (pipeStream != null && pipeStream.IsConnected)
                    {
                        pipeStream.Flush();
                        pipeStream.Close();
                    }
                }
            }
        }
    }
}
