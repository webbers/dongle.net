using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Dongle.System.IO.Ftp
{
    public class FtpDirectoryInfo
    {
        private readonly string _path;
        private readonly NetworkCredential _networkCredential;

        public FtpDirectoryInfo(string path, NetworkCredential networkCredential)
        {
            if (!path.ToLower().StartsWith("ftp://"))
            {
                path = "ftp://" + path;
            }
            _path = path;
            _networkCredential = networkCredential;
        }

        public IEnumerable<FtpFileInfo> GetFiles(string searchPattern = "")
        {
            var files = new List<FtpFileInfo>();
            var request = (FtpWebRequest)WebRequest.Create(new Uri(_path + "/" + searchPattern));

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var networkCredential = _networkCredential;
            request.Credentials = networkCredential;
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (response != null)
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                files.Add(new FtpFileInfo(_path + "/" + reader.ReadLine(), networkCredential));
                            }
                        }
                    }
                }
            }
            return files;
        }
    }
}