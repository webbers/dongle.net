using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Dongle.System.IO
{
    /// <summary>
    /// A fast enumerator of files in a directory.  Use this if you need to get attributes for 
    /// all files in a directory.
    /// </summary>
    /// <remarks>
    /// This enumerator is substantially faster than using <see cref="Directory.GetFiles(string)"/>
    /// and then creating a new FileInfo object for each path.  Use this version when you 
    /// will need to look at the attibutes of each file returned (for example, you need
    /// to check each file in a directory to see if it was modified after a specific date).
    /// </remarks>
    public static class FastDirectoryEnumerator
    {
        /// <summary>
        /// Gets all the files in a directory that 
        /// match a specific filter, optionally including all sub directories.
        /// </summary>
        public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (searchPattern == null)
            {
                throw new ArgumentNullException("searchPattern");
            }
            if ((searchOption != SearchOption.TopDirectoryOnly) && (searchOption != SearchOption.AllDirectories))
            {
                throw new ArgumentOutOfRangeException("searchOption");
            }
            try
            {
                return new FileEnumerable(path, searchPattern, searchOption);
            }
            catch (Exception)
            {
                return Directory.GetFiles(path, searchPattern);
            }
        }

        /// <summary>
        /// Provides the implementation of the interface
        /// </summary>
        private class FileEnumerable : IEnumerable<string>
        {
            private readonly string _path;
            private readonly string _filter;
            private readonly SearchOption _searchOption;

            /// <summary>
            /// Initializes a new instance of the FileEnumerable class.
            /// </summary>
            public FileEnumerable(string path, string filter, SearchOption searchOption)
            {
                _path = path;
                _filter = filter;
                _searchOption = searchOption;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            public IEnumerator<string> GetEnumerator()
            {
                return new FileEnumerator(_path, _filter, _searchOption);
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new FileEnumerator(_path, _filter, _searchOption);
            }
        }

        /// <summary>
        /// Provides the implementation of the 
        /// <see cref="T:System.Collections.Generic.IEnumerator`1"/> interface
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private class FileEnumerator : IEnumerator<string>
        {
            /// <summary>
            /// Wraps a FindFirstFile handle.
            /// </summary>
            private sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
            {
                [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
                [DllImport("kernel32.dll")]
                private static extern bool FindClose(IntPtr handle);

                [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
                internal SafeFindHandle() : base(true)
                {
                }

                protected override bool ReleaseHandle()
                {
                    return FindClose(handle);
                }
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern SafeFindHandle FindFirstFile(string fileName, [In, Out] Win32FindData data);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool FindNextFile(SafeFindHandle hndFindFile, [In, Out, MarshalAs(UnmanagedType.LPStruct)] Win32FindData lpFindFileData);

            /// <summary>
            /// Hold context information about where we current are in the directory search.
            /// </summary>
            private class SearchContext
            {
                public readonly string Path;
                public Stack<string> SubdirectoriesToProcess;

                public SearchContext(string path)
                {
                    Path = path;
                }
            }

            private string _path;
            private readonly string _filter;
            private readonly SearchOption _searchOption;
            private readonly Stack<SearchContext> _contextStack;
            private SearchContext _currentContext;
            private SafeFindHandle _hndFindFile;
            private readonly Win32FindData _mWinFindData = new Win32FindData();

            /// <summary>
            /// Initializes a new instance of the FileEnumerator class.
            /// </summary>
            public FileEnumerator(string path, string filter, SearchOption searchOption)
            {
                _path = path;
                _filter = filter;
                _searchOption = searchOption;
                _currentContext = new SearchContext(path);

                if (_searchOption == SearchOption.AllDirectories)
                {
                    _contextStack = new Stack<SearchContext>();
                }
            }
            
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public string Current
            {
                get { return Path.Combine(_path, _mWinFindData.cFileName); }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, 
            /// or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_hndFindFile != null)
                {
                    _hndFindFile.Dispose();
                }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current
            {
                get { return _mWinFindData; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            public bool MoveNext()
            {
                var retval = false;

                //If the handle is null, this is first call to MoveNext in the current 
                // directory.  In that case, start a new search.
                if (_currentContext.SubdirectoriesToProcess == null)
                {
                    if (_hndFindFile == null)
                    {
                        new FileIOPermission(FileIOPermissionAccess.PathDiscovery, _path).Demand();

                        var searchPath = Path.Combine(_path, _filter);
                        _hndFindFile = FindFirstFile(searchPath, _mWinFindData);
                        retval = !_hndFindFile.IsInvalid;
                    }
                    else
                    {
                        //Otherwise, find the next item.
                        retval = FindNextFile(_hndFindFile, _mWinFindData);
                    }
                }

                //If the call to FindNextFile or FindFirstFile succeeded...
                if (retval)
                {
                    if ((_mWinFindData.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        //Ignore folders for now.   We call MoveNext recursively here to 
                        // move to the next item that FindNextFile will return.
                        return MoveNext();
                    }
                }
                else if (_searchOption == SearchOption.AllDirectories)
                {
                    if (_currentContext.SubdirectoriesToProcess == null)
                    {
                        var subDirectories = Directory.GetDirectories(_path);
                        _currentContext.SubdirectoriesToProcess = new Stack<string>(subDirectories);
                    }

                    if (_currentContext.SubdirectoriesToProcess.Count > 0)
                    {
                        var subDir = _currentContext.SubdirectoriesToProcess.Pop();
                        _contextStack.Push(_currentContext);
                        _path = subDir;
                        _hndFindFile = null;
                        _currentContext = new SearchContext(_path);
                        return MoveNext();
                    }

                    //If there are no more files in this directory and we are 
                    // in a sub directory, pop back up to the parent directory and
                    // continue the search from there.
                    if (_contextStack.Count > 0)
                    {
                        _currentContext = _contextStack.Pop();
                        _path = _currentContext.Path;
                        if (_hndFindFile != null)
                        {
                            _hndFindFile.Close();
                            _hndFindFile = null;
                        }

                        return MoveNext();
                    }
                }

                return retval;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _hndFindFile = null;
            }

        }

        /// <summary>
        /// Contains information about the file that is found 
        /// by the FindFirstFile or FindNextFile functions.
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto), BestFitMapping(false)]
        internal class Win32FindData
        {
            public FileAttributes dwFileAttributes;
            public uint ftCreationTime_dwLowDateTime;
            public uint ftCreationTime_dwHighDateTime;
            public uint ftLastAccessTime_dwLowDateTime;
            public uint ftLastAccessTime_dwHighDateTime;
            public uint ftLastWriteTime_dwLowDateTime;
            public uint ftLastWriteTime_dwHighDateTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            #pragma warning disable 169
            private int dwReserved0;
            private int dwReserved1;
            #pragma warning restore 169
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            #pragma warning disable 169
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            private string cAlternateFileName;
            #pragma warning restore 169

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            public override string ToString()
            {
                return "File name=" + cFileName;
            }
        }
    }
}
