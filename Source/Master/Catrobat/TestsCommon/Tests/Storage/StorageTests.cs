﻿using System;
using System.IO;
using System.IO.IsolatedStorage;
using Catrobat.Core.Storage;
using Catrobat.Core.ZIP;
using Catrobat.TestsCommon.Misc;
using Catrobat.TestsCommon.Misc.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Catrobat.Core;

namespace Catrobat.TestsCommon.Tests.Storage
{
  [TestClass]
  public class StorageWindowsStoreTests
  {
    [ClassInitialize()]
    public static void TestClassInitialize(TestContext testContext)
    {
      TestHelper.InitializeTests();

      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath;
        if(Directory.Exists(basePath))
          Directory.Delete(basePath,true);
      }
    }


    [TestMethod]
    public void DeleteDirectoryTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "DeleteDirectoryTest/";

        Directory.CreateDirectory(basePath);

        Directory.CreateDirectory(basePath + "Folder1/F2/F3");

        var file1 = File.Create(basePath + "Folder1/F2/F3/file1.txt");
        file1.Close();
        var file2 = File.Create(basePath + "Folder1/F2/F3/file2.bin");
        file2.Close();

        storage.DeleteDirectory("DeleteDirectoryTest/Folder1/F2");

        Assert.IsTrue(Directory.Exists(basePath + "Folder1"));
        Assert.IsFalse(Directory.Exists(basePath + "Folder1/F2"));
      }
    }

    [TestMethod]
    public void DeleteFileTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "DeleteFileTest/";

        Directory.CreateDirectory(basePath);

        var file1 = File.Create(basePath + "file1.txt");
        file1.Close();

        var file2 = File.Create(basePath + "file2.bin");
        file2.Close();

        storage.DeleteFile("DeleteFileTest/file1.txt");

        Assert.IsFalse(File.Exists(basePath + "file1.txt"));
        Assert.IsTrue(File.Exists(basePath + "file2.bin"));
      }
    }

    [TestMethod]
    public void CopyDirectoryTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "CopyDirectoryTest/";

        Directory.CreateDirectory(basePath + "CopyDirectoryTestFolder1/f2/f3");

        var file1 = File.Create(basePath + "CopyDirectoryTestFolder1/f2/f3/file1.txt");
        file1.Close();

        var file2 = File.Create(basePath + "CopyDirectoryTestFolder1/f2/file2.bin");
        file2.Close();

        storage.CopyDirectory("CopyDirectoryTest/CopyDirectoryTestFolder1", "CopyDirectoryTest/CopyDirectoryTestFolder1_copy");

        Assert.IsTrue(Directory.Exists(basePath + "CopyDirectoryTestFolder1_copy"));
        Assert.IsTrue(Directory.Exists(basePath + "CopyDirectoryTestFolder1_copy/f2"));
        Assert.IsTrue(Directory.Exists(basePath + "CopyDirectoryTestFolder1_copy/f2/f3"));

        Assert.IsTrue(File.Exists(basePath + "CopyDirectoryTestFolder1_copy/f2/file2.bin"));
        Assert.IsTrue(File.Exists(basePath + "CopyDirectoryTestFolder1_copy/f2/f3/file1.txt"));
      }
    }

    [TestMethod]
    public void CopyFileTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "CopyFileTest/";

        Directory.CreateDirectory(basePath);

        File.WriteAllText(basePath + "file1.txt", "TESTING\n123");

        storage.CopyFile("CopyFileTest/file1.txt", "CopyFileTest/Copy/copied_file1.txt");
        storage.CopyFile("CopyFileTest/file1.txt", "CopyFileTest/copied_file2.txt");

        Assert.IsTrue(Directory.Exists(basePath + "Copy/"));

        Assert.IsTrue(File.Exists(basePath + "file1.txt"));
        Assert.IsTrue(File.Exists(basePath + "Copy/copied_file1.txt"));
        Assert.IsTrue(File.Exists(basePath + "copied_file2.txt"));

        string file1Content = File.ReadAllText((basePath + "file1.txt"), System.Text.Encoding.UTF8);
        string copiedFile1Content = File.ReadAllText((basePath + "Copy/copied_file1.txt"), System.Text.Encoding.UTF8);
        string copiedFile2Content = File.ReadAllText((basePath + "copied_file2.txt"), System.Text.Encoding.UTF8);
        Assert.AreEqual(file1Content, copiedFile1Content);
        Assert.AreEqual(file1Content, copiedFile2Content);
      }
    }

    [TestMethod]
    public void OpenFileTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "OpenFileTest/";

        Directory.CreateDirectory(basePath);

        var file1 = File.Create(basePath + "file1.txt");
        file1.Close();

        Stream fileStream1 = storage.OpenFile("OpenFileTest/file1.txt", StorageFileMode.OpenOrCreate, StorageFileAccess.ReadWrite);
        Assert.IsTrue(fileStream1.CanRead);
        Assert.IsTrue(fileStream1.CanWrite);
        fileStream1.Close();

        Stream fileStream2 = storage.OpenFile("OpenFileTest/file2.txt", StorageFileMode.OpenOrCreate, StorageFileAccess.Read);
        Assert.IsTrue(fileStream2.CanRead);
        Assert.IsFalse(fileStream2.CanWrite);
        fileStream2.Close();

        Stream fileStream3 = storage.OpenFile("OpenFileTest/file2.txt", StorageFileMode.OpenOrCreate, StorageFileAccess.Write);
        Assert.IsFalse(fileStream3.CanRead);
        Assert.IsTrue(fileStream3.CanWrite);
        fileStream3.Close();
      }
    }

    [TestMethod]
    public void RenameDirectoryTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "RenameDirectoryTest/";

        Directory.CreateDirectory(basePath);

        Directory.CreateDirectory(basePath + "Folder1/F2/F3");

        var file1 = File.Create(basePath + "Folder1/F2/F3/file1.txt");
        file1.Close();
        var file2 = File.Create(basePath + "Folder1/F2/file2.bin");
        file2.Close();

        storage.RenameDirectory("RenameDirectoryTest/Folder1/F2", "renamed2");

        Assert.IsTrue(Directory.Exists(basePath + "Folder1/renamed2"));
        Assert.IsTrue(Directory.Exists(basePath + "Folder1/renamed2/F3"));
        Assert.IsTrue(File.Exists(basePath + "Folder1/renamed2/file2.bin"));
        Assert.IsTrue(File.Exists(basePath + "Folder1/renamed2/F3/file1.txt"));
      }
    }

    [TestMethod]
    public void LoadImageTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = "LoadImageTest/";
        var sampleProjectsPath = BasePathHelper.GetSampleProjectsPath();

        Directory.CreateDirectory(storage.BasePath + basePath);

        Stream stream = ResourceLoader.GetResourceStream(ResourceScope.TestCommon, sampleProjectsPath + "test.catroid");
        CatrobatZip.UnzipCatrobatPackageIntoIsolatedStorage(stream, basePath);
        stream.Close();

        var image = storage.LoadImage("LoadImageTest/screenshot.png");
        Assert.AreNotEqual(image, null);
      }
    }

    //[TestMethod]
    //public void SaveImageTest()
    //{
    //  using (IStorage storage = new StorageTest())
    //  {
    //    var basePath = storage.BasePath + "SaveImageTest/";
    //    var sampleProjectsPath = BasePathHelper.GetSampleProjectsPath();

    //    Directory.CreateDirectory(basePath);

    //    Stream stream = ResourceLoader.GetResourceStream(Projects.TestCommon, sampleProjectsPath + "test.catroid");
    //    CatrobatZip.UnzipCatrobatPackageIntoIsolatedStorage(stream, basePath);
    //    stream.Close();

    //    var image = storage.LoadImage("LoadImageTest/screenshot.png");
    //    storage.SaveImage("TestLoadImage2/screenshot.png", image);
    //    BitmapImage image2 = storage.LoadImage("TestLoadImage2/screenshot.png");

    //     TODO: Maybe check if pixels are corect?

    //    Assert.AreNotEqual(image, null);
    //  }
    //}

    [TestMethod]
    public void ReadWriteTextFileTest()
    {
      using (IStorage storage = new StorageTest())
      {
        var basePath = storage.BasePath + "ReadWriteTextFileTest/";

        Directory.CreateDirectory(basePath);

        storage.WriteTextFile("ReadWriteTextFileTest/test.txt", "test123");
        Assert.AreEqual(storage.ReadTextFile("ReadWriteTextFileTest/test.txt"), "test123");
      }
    }

    [TestMethod]
    public void ReadWriteSerializableObjectTest()
    {
      TestHelper.InitializeAndClearCatrobatContext();

      using (var storage = StorageSystem.GetStorage())
      {
        LocalSettings settingsWrite = new LocalSettings();
        settingsWrite.CurrentProjectName = "ProjectName";

        storage.WriteSerializableObject("testobject", settingsWrite);
        LocalSettings settingsRead = (LocalSettings)storage.ReadSerializableObject("testobject", settingsWrite.GetType());

        Assert.AreEqual(settingsRead.CurrentProjectName, "ProjectName");
      }
    }
  }
}
