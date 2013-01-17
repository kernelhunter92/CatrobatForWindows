﻿using Catrobat.Core.Storage;

namespace Catrobat.IDEWindowsStore.Misc.Storage
{
  public class StorageFactoryWindowsStore : IStorageFactory
  {
    public IStorage CreateStorage()
    {
      return new StorageWindowsStore();
    }
  }
}
