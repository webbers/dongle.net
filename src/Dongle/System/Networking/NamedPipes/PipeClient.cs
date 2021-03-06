﻿using System;
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
            NamedPipeClientStream pipeStream = null;

            try
            {
                pipeStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut,
                                                       PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation);
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
                if (pipeStream != null && pipeStream.IsConnected)
                {
                    pipeStream.Flush();
                    pipeStream.Close();
                }
            }
        }
    }
}
