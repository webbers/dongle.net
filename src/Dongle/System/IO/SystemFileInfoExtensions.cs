using System;
using System.IO;
using System.Linq;
using System.Text;

using Dongle.Resources;

namespace Dongle.System.IO
{
    public static class SystemFileInfoExtensions
    {
        public static bool BruteMove(this FileInfo fileInfo, DirectoryInfo destDirectory)
        {
            return fileInfo.BruteMove(destDirectory.FullName + @"\" + fileInfo.Name);
        }

        public static bool BruteMove(this FileInfo fileInfo, string destFileName)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("fileInfo");
            }

            if (!File.Exists(destFileName))
            {
                var newDirectory = new FileInfo(destFileName).Directory;
                newDirectory.CreateRecursively();
            }
            File.Move(fileInfo.FullName, destFileName);
            return true;
        }

        public static FileInfo ChangeExtension(this FileInfo fileInfo, string extension)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("fileInfo");
            }

            if (extension == null)
            {
                throw new ArgumentException("extension");
            }

            var fullNameWithoutExtension = fileInfo.FullName;

            var dotPos = fileInfo.FullName.LastIndexOf(".");
            if (dotPos > 0)
            {
                fullNameWithoutExtension = fileInfo.FullName.Substring(0, dotPos);
            }
            var newFullName = fullNameWithoutExtension + extension;

            fileInfo.MoveTo(newFullName);
            return new FileInfo(newFullName);
        }

        public static byte[] ReadBytes(this FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("fileInfo");
            }

            byte[] buffer;
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                var binaryReader = new BinaryReader(fileStream);
                var numBytes = fileInfo.Length;
                buffer = binaryReader.ReadBytes((int)numBytes);
            }
            return buffer;
        }

        public static string GetContent(this FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("fileInfo");
            }
            using (var streamReader = new StreamReader(fileInfo.FullName, DongleEncoding.Default))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static void PutContent(this FileInfo fileInfo, string content)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("fileInfo");
            }

            if (content == null)
            {
                throw new ArgumentException("content");
            }
            fileInfo.Directory.CreateRecursively();
            using (var writer = new StreamWriter(fileInfo.FullName, false, DongleEncoding.Default))
            {
                writer.Write(content);
            }
        }
        public static void CreateRecursively(this DirectoryInfo dirInfo)
        {
            if (dirInfo == null)
            {
                throw new ArgumentException("dirInfo");
            }

            if (dirInfo.Parent != null && dirInfo.Parent.Exists == false)
            {
                CreateRecursively(dirInfo.Parent);
            }
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }

        public static DirectoryInfo Closest(this DirectoryInfo dirInfo, string name)
        {
            var parent = dirInfo.Parent;

            if (parent == null)
            {
                return null;
            }

            if (parent.Name.Replace("/", "").Equals(name.Replace("/", ""), StringComparison.CurrentCultureIgnoreCase))
            {
                return parent;
            }
            return parent.Closest(name);
        }

        public static void CopyTo(this DirectoryInfo srcDir, string destDir, bool copySubDirs = true)
        {
            srcDir.Refresh();

            if (!srcDir.Exists)
            {
                throw new DirectoryNotFoundException(DongleResource.SourceDirectoryNotFound + ": " + srcDir.FullName);
            }

            if (!Directory.Exists(destDir))
            {
                new DirectoryInfo(destDir).CreateRecursively();
            }

            var files = srcDir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDir, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                var dirs = srcDir.GetDirectories();
                foreach (var dir in dirs)
                {
                    var newDir = new DirectoryInfo(Path.Combine(destDir, dir.Name));
                    dir.CopyTo(newDir.FullName);
                }
            }
        }

        /// <summary>
        /// Cria uma pasta recursivamente
        /// </summary>
        public static void CreatePath(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Parent != null && !directoryInfo.Exists)
            {
                CreatePath(directoryInfo.Parent);
            }
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        /// <summary>
        /// Obtém o tamanho total em bytes de um diretório e de seus subdiretórios
        /// </summary>
        /// <returns>Retorna o tamanho total em bytes. Retorna o valor 0 caso o diretório não exista</returns>
        public static long GetFolderSize(this DirectoryInfo directoryInfo, bool includeSubdirectories = true)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
            {
                return 0;
            }
            var totalSize = directoryInfo.EnumerateFiles().Sum(file => file.Length);

            if (includeSubdirectories)
            {
                totalSize += directoryInfo.EnumerateDirectories().Sum(dir => GetFolderSize(dir));
            }

            return totalSize;
        }
    }
}