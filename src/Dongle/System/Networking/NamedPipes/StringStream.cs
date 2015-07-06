using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Dongle.System.IO;

namespace Dongle.System.Networking.NamedPipes
{
    public class StringStream : IDisposable
    {
        private readonly PipeStream _ioStream;
        private readonly Encoding _streamEncoding = DongleEncoding.Default;

        public StringStream(PipeStream ioStream, int timeout = -1)
        {
            _ioStream = ioStream;
            _ioStream.ReadMode = PipeTransmissionMode.Message;
            Timeout = timeout;
        }

        public int Timeout { get; set; }

        public string ReadLine()
        {
            using (var reader = new StreamReader(_ioStream, _streamEncoding))
            {
                return reader.ReadLine();
            }
        }

        public void WriteLine(string outString)
        {
            using (var writer = new StreamWriter(_ioStream, _streamEncoding))
            {
                writer.WriteLine(outString);
                writer.Flush();
            }
        }

        public string ReadStringAsync()
        {
            var readTask = Task.Factory.StartNew(() => ReadString());

            if (readTask.Wait(Timeout))
            {
                return readTask.Result;
            }
            throw new TimeoutException();
        }

        public void WriteStringAsync(string outString)
        {
            var writeTask = Task.Factory.StartNew(() => WriteString(outString));

            if (!writeTask.Wait(Timeout))
            {
                throw new TimeoutException();
            }
        }

        public string ReadString()
        {
            var memoryStream = new MemoryStream();
            var buffer = new byte[1024];
            do
            {
                var bytesRead = _ioStream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, bytesRead);
            } while (!_ioStream.IsMessageComplete);
            return _streamEncoding.GetString(memoryStream.ToArray());
        }

        public void WriteString(string outString)
        {
            var outBuffer = _streamEncoding.GetBytes(outString);
            _ioStream.Write(outBuffer, 0, outBuffer.Length);
            _ioStream.Flush();
        }

        public void Dispose()
        {
            _ioStream.Close();
            _ioStream.Dispose();
        }
    }
}
