using System;
using System.IO;
using Dongle.System;
using Dongle.System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System.IO
{
    [TestClass]
    public class FileInfoExtensionsTest
    {
        [TestMethod]
        public void BruteMoveIfAlreadyExists()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = directory + "\\foo.bar";
            CreateFoo();

            var fileInfo = new FileInfo(path1);
            Assert.IsTrue(fileInfo.BruteMove(path1));
            Assert.IsTrue(File.Exists(path1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BruteMoveException1()
        {
            SystemFileInfoExtensions.BruteMove(null, "foo");
        }

        [TestMethod]
        public void BruteMove1()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = directory + "\\foo.bar";
            var path3 = directory + "\\test";
            var path2 = directory + "\\test\\test\\foo.bar";

            if (Directory.Exists(path3)) Directory.Delete(path3, true);
            CreateFoo();

            var fileInfo = new FileInfo(path1);
            Assert.IsTrue(fileInfo.BruteMove(path2));
            Assert.IsTrue(File.Exists(path2));
        }

        [TestMethod]
        public void BruteMove2()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = directory + "\\foo.bar";
            var path3 = directory + "\\test";
            var path2 = directory + "\\test\\test";

            if (Directory.Exists(path3)) Directory.Delete(path3, true);
            CreateFoo();

            var fileInfo = new FileInfo(path1);
            Assert.IsTrue(fileInfo.BruteMove(new DirectoryInfo(path2)));
            Assert.IsTrue(File.Exists(path2 + "\\foo.bar"));
        }

        [TestMethod]
        public void ChangeExtension()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();

            Assert.IsTrue(fileInfo.Exists);
            fileInfo = fileInfo.ChangeExtension("foo");
            Assert.IsTrue(fileInfo.Exists);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeException1()
        {
            SystemFileInfoExtensions.ChangeExtension(null, "foo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeException2()
        {
            SystemFileInfoExtensions.ChangeExtension(new FileInfo("foo"), null);
        }

        [TestMethod]
        public void ReadBytes()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();

            var bytes = fileInfo.ReadBytes();

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual("teste", bytes.FromBytesToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadBytesException()
        {
            SystemFileInfoExtensions.ReadBytes(null);
        }

        [TestMethod]
        public void GetContent()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();
            Assert.AreEqual("teste", fileInfo.GetContent());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetContentException()
        {
            SystemFileInfoExtensions.GetContent(null);
        }

        [TestMethod]
        public void PutContent()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();

            fileInfo.PutContent("abcde");
            Assert.AreEqual("abcde", fileInfo.GetContent());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutContentException1()
        {
            SystemFileInfoExtensions.PutContent(null, "foo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutContentException2()
        {
            new FileInfo("foo").PutContent(null);
        }

        [TestMethod]
        public void CreateDirectortRecursively()
        {
            var directory = ApplicationPaths.RootDirectory;
            var directoryInfo = new DirectoryInfo(directory + "\\test\\test\\test\\test");
            var mainDirectoryInfo = new DirectoryInfo(directory + "\\test\\");

            if (mainDirectoryInfo.Exists)
            {
                mainDirectoryInfo.Delete(true);
            }
            Assert.IsFalse(Directory.Exists(mainDirectoryInfo.FullName));
            directoryInfo.CreateRecursively();
            Assert.IsTrue(Directory.Exists(mainDirectoryInfo.FullName));
            Assert.IsTrue(Directory.Exists(directoryInfo.FullName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDirectortRecursivelyException()
        {
            SystemFileInfoExtensions.CreateRecursively(null);
        }

        [TestMethod]
        public void Closest()
        {
            var directory = new DirectoryInfo(ApplicationPaths.RootDirectory);
            Assert.IsNotNull(directory.Closest("dongle.net"));

            directory = new DirectoryInfo(ApplicationPaths.RootDirectory);
            Assert.IsNull(directory.Closest("blablabla"));
        }

        [TestMethod]
        public void CopyDirectory()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = new DirectoryInfo(directory + @"\path\path");
            path1.CreateRecursively();
            CreateFoo(@"\path\path");
            path1.CreateSubdirectory("path");

            path1.CopyTo(directory + @"\moved");

            Assert.IsTrue(File.Exists(directory + @"\moved\foo.bar"));
            Assert.IsTrue(Directory.Exists(directory + @"\moved\path"));
        }

        private static void CreateFoo(string folder = "")
        {
            var directory = ApplicationPaths.RootDirectory;
            var path1 = directory + @"\" + folder + @"\foo.bar";

            if (File.Exists(path1)) File.Delete(path1);
            using (var f = File.CreateText(path1))
            {
                f.Write("teste");
            }
        }
    }
}