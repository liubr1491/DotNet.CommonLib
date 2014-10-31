using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Win32
{
    public class HdInfo
    {
        private static readonly StringBuilder StrBu = new StringBuilder();

        /// <summary>
        ///  获取硬盘空间大小
        /// </summary>
        /// <param name="drive">指定盘符，默认为ALL</param>
        public static string Get_HD_Space(string drive)
        {
            Dictionary<string, object> dicts = Get_HD_Space();
            if (drive.Trim().Length <= 0)
            {
                return StrBu.ToString();
            }
            object o = null;
            foreach (string key in dicts.Keys)
            {
                if (dicts.ContainsKey(drive.ToLower()))
                {
                    o = dicts[drive.ToLower()];
                    break;
                }
            }
            return o != null ? o.ToString() : "";
        }

        /// <summary>
        /// 获取硬盘空间大小
        /// </summary>
        public static Dictionary<string, object> Get_HD_Space()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DriveInfo d in allDrives)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("   Drive {0}", d.Name.ToLower()));
                sb.Append(string.Format("   File type: {0}", d.DriveType));
                if (d.IsReady)
                {
                    sb.Append(string.Format("Volume label: {0}{1}", d.VolumeLabel, "\r\n"));
                    sb.Append(string.Format("File system: {0}{1}", d.DriveFormat, "\r\n"));
                    sb.Append(string.Format("Available space to current user:{0, 15} GB{1}", Win32Utility.ConvertBytes(d.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture), 3), "\r\n"));
                    sb.Append(string.Format("Total available space:          {0, 15} GB{1}", Win32Utility.ConvertBytes(d.TotalFreeSpace.ToString(CultureInfo.InvariantCulture), 3), "\r\n"));
                    sb.Append(string.Format("Total size of drive:            {0, 15} GB{1}", Win32Utility.ConvertBytes(d.TotalSize.ToString(CultureInfo.InvariantCulture), 3), "\r\n"));
                }

                StrBu.Append(sb + "\r\n");

                dict.Add(d.Name, sb);
            }
            return dict;
        }
    }
}
