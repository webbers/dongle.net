using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Dongle.System.IO
{
    public static class ApplicationPaths
    {
        private static string _rootDirectory;
        /// <summary>
        /// Retorna o diretório da aplicação
        /// </summary>
        public static string RootDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_rootDirectory))
                {
                    _rootDirectory = GetTypeAssemblyPath(typeof(ApplicationPaths));
                }
                return _rootDirectory;
            }
        }

        /// <summary>
        /// Retorna o caminho completo da pasta APP_DATA
        /// </summary>
        public static string DataDirectory
        {
            get { return RootDirectory + @"App_Data\"; }
        }

        private static string _currentPath;
        /// <summary>
        /// Retorna o caminho atual da dll em execução. Caso a dll esteja na pasta bin, volta dois niveis
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                if (string.IsNullOrEmpty(_currentPath))
                {
                    _currentPath = GetTypeAssemblyPath(typeof(ApplicationPaths));
                    var directoryInfo = new DirectoryInfo(_currentPath);
                    if (directoryInfo.Name.ToLower() == "bin")
                    {
                        if (directoryInfo.Parent != null && directoryInfo.Parent.Parent != null)
                        {
                            _currentPath = directoryInfo.Parent.Parent.FullName;
                        }
                    }
                    if (!_currentPath.EndsWith(@"\"))
                    {
                        _currentPath += @"\";
                    }
                }
                return _currentPath;
            }
        }

        /// <summary>
        /// Equivalente a Path.Combine, mas já combina com o CurrentPath
        /// </summary>
        public static string CurrentPathCombine(string subpath)
        {
            return Path.Combine(CurrentPath, subpath);
        }

        /// <summary>
        /// Equivalente a Path.Combine, mas já combina com o CurrentPath
        /// </summary>
        public static string CurrentPathCombine(params string[] subPaths)
        {
            return subPaths.Aggregate(CurrentPath, Path.Combine);
        }

        /// <summary>
        /// Retorna o caminho atual da dll em execução.
        /// </summary>
        public static string GetTypeAssemblyPath(Type type)
        {
            var directory = Path.GetDirectoryName(Assembly.GetAssembly(type).CodeBase);
            var path = directory != null ? directory.ToLower().Replace("file:\\\\", "").Replace("file:\\", "") : "";
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
            return path;
        }

        /// <summary>
        /// Retorna o caminho atual da dll em execução.
        /// </summary>
        public static string GetTypeAssemblyFullPath(Type type)
        {
            return Path.GetFullPath(Assembly.GetAssembly(type).CodeBase.ToLower().Replace("file:///", "").Replace("file://", "").Replace("file:/", "").Replace("file:\\\\", "").Replace("file:\\", ""));
        }        

        /// <summary>
        /// Equivalente a Path.Combine, mas já combina com o CurrentPath
        /// </summary>
        public static string AssemblyPathCombine(params string[] subPaths)
        {
            return subPaths.Aggregate(RootDirectory, Path.Combine);
        }        

        /// <summary>
        /// Cria uma pasta recursivamente
        /// </summary>  
        public static string CreatePath(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(CurrentPath, path);
            new DirectoryInfo(path).CreatePath();
            return path;
        }

        /// <summary>
        /// Cria uma pasta recursivamente. Se já existir, apaga antes de criar
        /// </summary>     
        public static void RecreatePath(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            CreatePath(path);
        }

        /// <summary>
        /// Apaga todos os arquivos de um diretório
        /// </summary>     
        public static void RemoveAllFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            foreach (var file in Directory.EnumerateFiles(path))
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Retorna um nome de arquivo com base na data atual, no formato yyyy-MM-ddTHH-mm-ss.FFFFFF_RANDOM
        /// </summary>
        public static string GetDateBasedFileName(string extension)
        {
            if (extension != "" && !extension.StartsWith("."))
                extension = "." + extension;
            return DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.FFFFFF_") + new Random().Next() + "." + extension;
        }

        /// <summary>
        /// Copia um diretório recursivamente
        /// </summary>
        public static void CopyDirectory(string source, string destination, bool replaceFiles = true, bool recursive = true, string fileFilter = "*.*", bool throwException = false)
        {
            if (!Path.IsPathRooted(source))
            {
                source = CurrentPathCombine(source);
            }
            if (!Path.IsPathRooted(destination))
            {
                destination = CurrentPathCombine(destination);
            }
            if (!Directory.Exists(source))
            {
                return;
            }
            if (!destination.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destination += Path.DirectorySeparatorChar;
            }
            CreatePath(destination);
            foreach (var entry in Directory.EnumerateFileSystemEntries(source, fileFilter))
            {
                var dest = Path.Combine(destination, Path.GetFileName(entry) ?? "");
                if (Directory.Exists(entry))
                {
                    if (recursive)
                    {
                        CopyDirectory(entry, dest);
                    }
                }
                else
                {
                    try
                    {
                        File.Copy(entry, dest, replaceFiles);
                    }
                    catch (IOException)
                    {
                        if (throwException)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Move os arquivos de um diretório recursivamente
        /// </summary>
        public static void MoveDirectoryFiles(string source, string destination, bool replaceFiles = true, bool recursive = true, string fileFilter = "*.*", bool throwException = false)
        {
            if (!Path.IsPathRooted(source))
            {
                source = CurrentPathCombine(source);
            }
            if (!Path.IsPathRooted(destination))
            {
                destination = CurrentPathCombine(destination);
            }
            if (!Directory.Exists(source))
            {
                return;
            }
            CreatePath(destination);
            foreach (var entry in Directory.EnumerateFileSystemEntries(source, fileFilter))
            {
                var dest = Path.Combine(destination, Path.GetFileName(entry) ?? "");
                if (Directory.Exists(entry))
                {
                    if (recursive)
                    {
                        MoveDirectoryFiles(entry, dest, replaceFiles, true, fileFilter);
                    }
                }
                else
                {
                    try
                    {
                        if (File.Exists(dest))
                        {
                            DeleteFiles(dest);
                        }
                        File.Move(entry, dest);
                    }
                    catch (IOException)
                    {
                        if (throwException)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Procura o diretório mais próximo do diretório corrente
        /// </summary>
        public static DirectoryInfo ClosestDir(string dirname)
        {
            var directory = new DirectoryInfo(RootDirectory);
            while (directory.GetDirectories(dirname).Length == 0)
            {
                directory = directory.Parent;
                if (directory == null)
                    return null;
            }
            return new DirectoryInfo(Path.Combine(directory.FullName, dirname));
        }

        /// <summary>
        /// Deleta os arquivos especificados. Aceita caminho relativo ou absoluto. Tenta apagar o arquivo várias vezes, caso ele esteja sendo utilizado.
        /// </summary>
        public static void DeleteFiles(params string[] files)
        {
            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                if (!Path.IsPathRooted(file))
                {
                    file = CurrentPathCombine(file);
                }
                if (!File.Exists(file))
                {
                    continue;
                }
                var j = 0;
                while (j < 20)
                {
                    try
                    {
                        j++;
                        File.Delete(file);
                        break;
                    }
                    catch (Exception)
                    {
                        Thread.CurrentThread.Join(50);
                    }
                }
            }
        }

        /// <summary>
        /// Atalho para obter a ultima data de escrita do arquivo
        /// </summary>
        public static DateTime GetFileLastWriteTime(string filepath)
        {
            if (!Path.IsPathRooted(filepath))
            {
                filepath = Path.Combine(CurrentPath, filepath);
            }
            if (!File.Exists(filepath))
            {
                return new DateTime();
            }
            return new FileInfo(filepath).LastWriteTime;
        }
    }
}