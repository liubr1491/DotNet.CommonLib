﻿using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNet.CommonLib.MMF
{
    public class SharedMemory<T> where T : struct
    {
        // Data
        private string smName;
        private Mutex smLock;
        private int smSize;
        private bool locked;
        private MemoryMappedFile mmf;
        private MemoryMappedViewAccessor accessor;

        // Constructor
        public SharedMemory(string name, int size)
        {
            smName = name;
            smSize = size;
        }

        // Methods
        public bool Open()
        {
            try
            {
                // Create named MMF
                mmf = MemoryMappedFile.CreateOrOpen(smName, smSize);

                // Create accessors to MMF
                accessor = mmf.CreateViewAccessor(0, smSize, MemoryMappedFileAccess.ReadWrite);

                // Create lock
                smLock = new Mutex(true, "SM_LOCK", out locked);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            accessor.Dispose();
            mmf.Dispose();
            smLock.Close();
        }

        public T Data
        {
            get
            {
                T dataStruct;
                accessor.Read<T>(0, out dataStruct);
                return dataStruct;
            }
            set
            {
                smLock.WaitOne();
                accessor.Write<T>(0, ref value);
                smLock.ReleaseMutex();
            }
        }
    }
}
