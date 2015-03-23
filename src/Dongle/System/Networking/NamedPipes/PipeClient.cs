using System;
using System.IO.Pipes;
using System.Security.Principal;

namespace Dongle.System.Networking.NamedPipes
{
    public class PipeClient
    {
        public delegate void ErrorOcurredEventHandler(Exception exception);

        public delegate void ConnectedToServerEventHandler(StringStream stream);

        public event ErrorOcurredEventHandler OnErrorOcurred;

        public event ConnectedToServerEventHandler OnConnected;

        public void Connect(string pipeName, string serverName = ".", int timeout = 2000)
        {
            using (var pipeStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation))
            {
                try
                {
                    pipeStream.Connect(timeout);
                    using (var stringStream = new StringStream(pipeStream))
                    {
                        if (OnConnected != null)
                        {
                            OnConnected(stringStream);
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
                    pipeStream.Flush();
                    pipeStream.Close();
                }
            }
        }
    }
}
