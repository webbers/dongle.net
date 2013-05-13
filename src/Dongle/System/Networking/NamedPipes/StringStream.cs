using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Dongle.System.Networking.NamedPipes
{
    public class StringStream : IDisposable
    {
        private readonly PipeStream _ioStream;
        private readonly Encoding _streamEncoding = Encoding.Default;

        public StringStream(PipeStream ioStream)
        {
            _ioStream = ioStream;
            _ioStream.ReadMode = PipeTransmissionMode.Message;
        }

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

        //public string ReadString()
        //{
        //    var lenBytes = new byte[20];
        //    _ioStream.Read(lenBytes, 0, 20);
        //    var len = Convert.ToInt32(_streamEncoding.GetString(lenBytes));            
        //    if (len <= 0)
        //        return "";
        //    var inBuffer = new byte[len];
        //    _ioStream.Read(inBuffer, 0, len);
        //    return _streamEncoding.GetString(inBuffer);
        //}

        //public int WriteString(string outString)
        //{
        //    var outBuffer = _streamEncoding.GetBytes(outString);
        //    var len = outBuffer.Length;
        //    var lenStr = outBuffer.Length.ToString().PadLeft(10, '0');
        //    var lenBuffer = _streamEncoding.GetBytes(lenStr);
        //    _ioStream.Write(lenBuffer, 0, lenBuffer.Length);
        //    _ioStream.Flush();
        //    _ioStream.Write(outBuffer, 0, len);
        //    _ioStream.Flush();
        //    return outBuffer.Length + 2;
        //}


        public string ReadStringAsync(int timeout = 2000)
        {
            var readTask = Task.Factory.StartNew(() => ReadString());

            if (readTask.Wait(timeout))
            {
                return readTask.Result;
            }
            throw new TimeoutException();
        }

        public void WriteStringAsync(string outString, int timeout = 2000)
        {
            var writeTask = Task.Factory.StartNew(() => WriteString(outString));

            if (!writeTask.Wait(timeout))
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
