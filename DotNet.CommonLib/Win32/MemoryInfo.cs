using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNet.CommonLib.Win32
{
    public class MemoryInfo
    {       //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder WinDir, int count);
        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder SysDir, int count);
        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static string GetMemInfo()
        {
            //调用GlobalMemoryStatus函数获取内存的相关信息
            MEMORY_INFO memInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref memInfo);

            StringBuilder sb = new StringBuilder();

            //*%的内存正在使用
            sb.Append(memInfo.dwMemoryLoad + "% of the memory is being used " + "\r\n");
            //总共的物理内存
            sb.Append("Physical memory total :" + Win32Utility.ConvertBytes(memInfo.dwTotalPhys.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            //可使用的物理内存
            sb.Append("Use of physical memory :" + Win32Utility.ConvertBytes(memInfo.dwAvailPhys.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            //交换文件总大小
            sb.Append("Total size of the swap file" + Win32Utility.ConvertBytes(memInfo.dwTotalPageFile.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            //尚可交换文件大小为
            sb.Append(" Can still swap file size :" + Win32Utility.ConvertBytes(memInfo.dwAvailPageFile.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            //总虚拟内存
            sb.Append("The Total virtual memory :" + Win32Utility.ConvertBytes(memInfo.dwTotalVirtual.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            //未用虚拟内存有
            sb.Append("Unused virtual memory :" + Win32Utility.ConvertBytes(memInfo.dwAvailVirtual.ToString(CultureInfo.InvariantCulture), 3) + "GB" + "\r\n");
            // ConvertBytes(totMem, 3) + " GB"
            return sb.ToString();
        }
    }
}
