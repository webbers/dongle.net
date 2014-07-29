using System;
using System.IO;
using Dongle.System;
using Dongle.System.IO;
using NUnit.Framework;

namespace Dongle.Tests.System.IO
{
    [TestFixture]
    public class FileInfoExtensionsTest
    {
        [Test]
        public void BruteMoveIfAlreadyExists()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = directory + "\\foo.bar";
            CreateFoo();

            var fileInfo = new FileInfo(path1);
            Assert.IsTrue(fileInfo.BruteMove(path1));
            Assert.IsTrue(File.Exists(path1));
        }

        [Test]
        public void BruteMoveException1()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.BruteMove(null, "foo"));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void ChangeExtension()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();
            if (File.Exists(directory + "\\foo.foo")) File.Delete(directory + "\\foo.foo");

            Assert.IsTrue(fileInfo.Exists);
            fileInfo = fileInfo.ChangeExtension("foo");
            Assert.IsTrue(File.Exists(directory + "\\foo.foo"));
        }

        [Test]
        public void ChangeException1()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.ChangeExtension(null, "foo"));
        }

        [Test]
        public void ChangeException2()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.ChangeExtension(new FileInfo("foo"), null));
        }

        [Test]
        public void ReadBytes()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();

            var bytes = fileInfo.ReadBytes();

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual("teste", bytes.FromBytesToString());
        }

        [Test]
        public void ReadBytesException()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.ReadBytes(null));
        }

        [Test]
        public void GetContent()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();
            Assert.AreEqual("teste", fileInfo.GetContent());
        }

        [Test]
        public void GetContentException()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.GetContent(null));
        }

        [Test]
        public void PutContent()
        {
            var directory = ApplicationPaths.RootDirectory;
            var fileInfo = new FileInfo(directory + "\\foo.bar");

            CreateFoo();

            fileInfo.PutContent("abcde");
            Assert.AreEqual("abcde", fileInfo.GetContent());
        }

        [Test]
        public void PutContentException1()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.PutContent(null, "foo"));
        }

        [Test]
        public void PutContentException2()
        {
            Assert.Throws<ArgumentException>(() => new FileInfo("foo").PutContent(null));
        }

        [Test]
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

        [Test]
        public void CreateDirectortRecursivelyException()
        {
            Assert.Throws<ArgumentException>(() => SystemFileInfoExtensions.CreateRecursively(null));
        }

        [Test]
        public void Closest()
        {
            var directory = new DirectoryInfo(ApplicationPaths.RootDirectory);
            Assert.IsNotNull(directory.Closest("dongle.net"));

            directory = new DirectoryInfo(ApplicationPaths.RootDirectory);
            Assert.IsNull(directory.Closest("blablabla"));
        }

        [Test]
        public void CopyDirectory()
        {
            var directory = ApplicationPaths.RootDirectory;

            var path1 = new DirectoryInfo(directory + @"\path\path");
            path1.CreateRecursively();
            CreateFoo(@"\path\path");
            path1.CreateSubdirectory("path");

            if (Directory.Exists(directory + @"\moved")) Directory.Delete(directory + @"\moved", true);
            path1.CopyTo(directory + @"\moved");

            Assert.IsTrue(File.Exists(directory + @"\moved\foo.bar"));
            Assert.IsTrue(Directory.Exists(directory + @"\moved\path"));
        }

        [Test]
        public void GetFolderSizeTest()
        {
            var testSizeFolder = new DirectoryInfo(ApplicationPaths.CurrentPathCombine("FolderSizeTest"));
            testSizeFolder.Create();

            var testSizeSubFolder = new DirectoryInfo(ApplicationPaths.CurrentPathCombine(testSizeFolder.FullName, "SubFolderSizeTest"));
            testSizeSubFolder.Create();

            using (var f = File.CreateText(Path.Combine(testSizeFolder.FullName, "test.txt")))
            {
                f.Write("teste");
            }
            using (var f = File.CreateText(Path.Combine(testSizeSubFolder.FullName, "test.txt")))
            {
                f.Write("teste");
            }
            Assert.AreEqual(10, testSizeFolder.GetFolderSize());
            Assert.AreEqual(5, testSizeFolder.GetFolderSize(false));
            Assert.AreEqual(0, new DirectoryInfo("FakeDirectory").GetFolderSize(false));
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