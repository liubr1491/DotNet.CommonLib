using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DotNet.CommonLib.MMF
{
    public class MMFHelper
    {
        public static void MMFCopyFile(string[] args)
        {
            int offset = 0;
            int length = 0;
            byte[] buffer;

            if (args.GetLength(0) != 2)
            {
                Console.WriteLine("Usage: MMFCopyFile.exe file1 file2");
                return;
            }

            FileInfo fi = new FileInfo(args[0]);
            length = (int)fi.Length;

            // Create unnamed MMF
            using (var mmf1 = MemoryMappedFile.CreateFromFile(args[0], FileMode.OpenOrCreate, null, offset + length))
            {
                // Create reader to MMF
                using (var reader = mmf1.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read))
                {
                    // Read from MMF
                    buffer = new byte[length];
                    reader.ReadArray<byte>(0, buffer, 0, length);
                }
            }

            // Create disk file
            using (FileStream fs = File.Create(args[1]))
            {
                fs.Close();
            }

            // Create unnamed MMF
            using (var mmf2 = MemoryMappedFile.CreateFromFile(args[1], FileMode.Create, null, offset + length))
            {
                // Create writer to MMF
                using (var writer = mmf2.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Write))
                {
                    // Write to MMF
                    writer.WriteArray<byte>(0, buffer, 0, length);
                }
            }
        }

        /// <summary>
        /// inter-process communication (or IPC)
        /// </summary>
        /// <param name="args"></param>
        public static void MMFIPCProvider(string[] args)
        {
            int offset = 0;
            int length = 32;
            byte[] buffer = new byte[length];

            if (args.GetLength(0) != 1)
            {
                Console.WriteLine("Usage: NetMMF_Provider.exe name");
                return;
            }

            // Fill buffer with some data
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)('0' + i);
            }

            // Create named MMF
            using (var mmf = MemoryMappedFile.CreateNew(args[0], offset + buffer.Length))
            {
                // Lock
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "MMF_IPC", out mutexCreated);

                // Create accessor to MMF
                using (var accessor = mmf.CreateViewAccessor(offset, buffer.Length))
                {
                    // Write to MMF
                    accessor.WriteArray<byte>(0, buffer, offset, buffer.Length);
                }

                mutex.ReleaseMutex();

                // Press any key to exit...
                Console.ReadKey();
            }

            // Here is the code segment for accessing the MMF and reading data from it
            // Create named MMF
            using (var mmf = MemoryMappedFile.OpenExisting(args[0]))
            {
                // Create accessor to MMF
                using (var accessor = mmf.CreateViewAccessor(offset, buffer.Length))
                {
                    // Wait for the lock
                    Mutex mutex = Mutex.OpenExisting("MMF_IPC");
                    mutex.WaitOne();

                    // Read from MMF
                    accessor.ReadArray<byte>(0, buffer, 0, buffer.Length);
                }
            }
        }
    }
}
