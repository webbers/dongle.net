using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using Dongle.System.IO;
using Dongle.System.Networking.Tcp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System.Networking
{
    [TestClass]
    public class TcpSystemTest : TcpFunctionsTest
    {
        public Thread ServerThread;
        public Thread ClientThread;
        public const string Server = "Server";
        public const string Client = "Client";

        [TestMethod]
        public void SystemTestTcp()
        {
            ServerThread = new Thread(StartServer);
            ServerThread.Start();
            AssertLastLog(Server, "SERVER STARTED");
            
            ClientThread = new Thread(StartClient);
            ClientThread.Start();
            AssertAnyOf(Client, "CONNECTED true");
            AssertAnyOf(Server, "CONNECTED WITH CLIENT");
            var connectionId = TcpServer.ConnectionList.First().Key;
        
            //Testa enviar comando do cliente para o servidor
            TcpClient.SendCommand("TEST");
            AssertAnyOf(Client, "COMMAND SENT TEST");
            AssertAnyOf(Server, "COMMAND RECEIVED TEST");
            TcpClient.SendCommand("TEST2");
            AssertAnyOf(Client, "COMMAND SENT TEST2");
            AssertAnyOf(Server, "COMMAND RECEIVED TEST2");
 
            //Testa enviar comando do servidor para o cliente
            TcpClient.SendCommand("HELLO");
            TcpClient.WaitForCommand();
            AssertAnyOf(Client, "COMMAND SENT HELLO", 4);
            AssertAnyOf(Server, "COMMAND RECEIVED HELLO", 4);
            AssertAnyOf(Server, "COMMAND SENT HELLO CLIENT", 4);
            AssertAnyOf(Client, "COMMAND RECEIVED HELLO CLIENT", 4);

            //Testa enviar arquivo para o servidor
            var fileData = new byte[] {72, 69, 76, 76, 79, 32, 83, 62, 67};
            TcpClient.SendCommand("FILE:9");
            TcpClient.SendFile(fileData, 0, 4);
            AssertAnyOf(Client, "FILE SENT", 4);            
            AssertAnyOf(Server, "FILE RECEIVED", 4);
            Assert.IsTrue(fileData.SequenceEqual(File.ReadAllBytes(ApplicationPaths.CurrentPath + @"Server.txt")));
          
            //Testa enviar arquivo para o cliente
            TcpClient.SendCommand("GETFILE");
            TcpClient.WaitForFile(9, 4);
            AssertAnyOf(Server, "FILE SEND STARTED", 4);
            AssertAnyOf(Client, "FILE RECEIVED", 4);
            Assert.IsTrue(fileData.SequenceEqual(File.ReadAllBytes(ApplicationPaths.CurrentPath + @"Client.txt")));

            //Testa desconectar
            TcpClient.Disconnect();
            AssertAnyOf(Server, "DISCONNECTED OF CLIENT");
            Assert.IsFalse(TcpClient.IsConnected);
            Assert.AreEqual(0, TcpServer.ConnectionList.Count);            
            TcpServer.Stop();

            //Tenta enviar comandos após desconexão. Não deve gerar exceptions.
            TcpServer.SendCommand(connectionId, "Alow");
            TcpServer.SendFile(connectionId, new byte[]{1,2,3,4,5,6});
            TcpServer.WaitForCommand(connectionId);
            TcpServer.WaitForFile(connectionId, 6);
            TcpServer.CloseConnection(connectionId);
    
            TcpServer.Stop();
        }

        [TestMethod]
        public void TestTwoServerSamePort()
        {
            var randomPort = 56000 + new Random().Next(1, 999);
            var tcpServer1 = new TcpServer();                        
            tcpServer1.Start(randomPort, IPAddress.Loopback);
            AssertIsTrue(() => tcpServer1.Running);

            var tcpServer2 = new TcpServer();
            tcpServer2.OnErrorOcurred += delegate { };
            tcpServer2.Start(randomPort, IPAddress.Loopback);
            AssertIsTrue(() => !tcpServer2.Running);

            var tcpClient1 = new TcpClient();
            tcpClient1.Connect(IPAddress.Loopback, randomPort);
            tcpClient1.Disconnect();

            tcpServer1.Stop();
        }
    }
}
