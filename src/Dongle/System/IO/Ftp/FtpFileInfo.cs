using System;
using System.IO;
using System.Net;

namespace Dongle.System.IO.Ftp
{
    public class FtpFileInfo
    {
        private readonly string _fileName;
        private readonly NetworkCredential _networkCredential;

        public FtpFileInfo(string fileName, NetworkCredential networkCredential)
        {
            _fileName = fileName;
            _networkCredential = networkCredential;
        }

        public string FullName
        {
            get
            {
                return _fileName;
            }
        }

        public string Name
        {
            get
            {
                return _fileName.LastIndexOf("/") > 0 ? _fileName.Substring(_fileName.LastIndexOf("/") + 1) : _fileName;
            }
        }

        public FileInfo CopyTo(string destFilename)
        {
            var request = (FtpWebRequest)WebRequest.Create(new Uri(_fileName));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = _networkCredential;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (response != null)
                {
                    using (var reader = response.GetResponseStream())
                    {
                        if (reader != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                var buffer = new byte[1024];

                                while (true)
                                {
                                    var bytesRead = reader.Read(buffer, 0, buffer.Length);
                                    if (bytesRead == 0)
                                    {
                                        break;
                                    }
                                    memoryStream.Write(buffer, 0, bytesRead);
                                }

                                var downloadedData = memoryStream.ToArray();
                                using (var newFile = new FileStream(destFilename, FileMode.Create))
                                {
                                    newFile.Write(downloadedData, 0, downloadedData.Length);
                                }
                            }
                        }
                    }
                }
            }
            return new FileInfo(destFilename);
        }

        public FileInfo MoveTo(string destFilename)
        {
            var fileInfo = CopyTo(destFilename);
            Delete();
            return fileInfo;
        }

        public void Delete()
        {
            var request = (FtpWebRequest)WebRequest.Create(new Uri(_fileName));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = _networkCredential;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            var response = (FtpWebResponse)request.GetResponse();
            if (response != null)
            {
                response.Close();
            }
        }
    }
}