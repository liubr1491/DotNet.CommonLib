using System;
using System.Collections;
using System.IO;

namespace DotNet.CommonLib.Log
{
    /// <summary>
    /// 实现IComparer接口，实现文件按名称降序排序
    /// </summary>
    class FileSorter : IComparer
    {
        /// <summary>
        /// 继承IComparer接口必须实现的方法
        /// </summary>
        /// <param name="x">FileInfo文件x</param>
        /// <param name="y">FileInfo文件y</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            var xInfo = (FileInfo)x;
            var yInfo = (FileInfo)y;

            //按名称降序排序
            return String.Compare(yInfo.FullName, xInfo.FullName, StringComparison.Ordinal);
        }
    }
}