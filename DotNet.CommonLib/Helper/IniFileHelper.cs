using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNet.CommonLib.Helper
{
    /// <summary>
    /// 开发接口程序时，对于接口程序配置的IP地址、端口等都需要是可配置的，
    /// 而在Win Api原生实现了INI文件的读写操作，
    /// 因此只需要调用Win Api中的方法即可操作INI配置文件，
    /// 关键代码就是如何调用Win Api中的方法
    /// </summary>
    public class IniFileHelper
    {
        /// <summary>
        /// INI文件路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 构造函数,参数为INI文件路径 
        /// </summary>
        /// <param name="path">INI文件的路径</param>
        public IniFileHelper(string path)
        {
            Path = path;
        }

        #region 调用WinApi 原方法声明

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        #endregion

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string IniReadValue(string section, string key)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(section, key, "", temp, 255, Path);
            return temp.ToString();
        }

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        /// <summary>
        /// 清楚INI文件中所有的段落
        /// </summary>
        public void ClearAllSection()
        {
            IniWriteValue(null, null, null);
        }

        /// <summary>
        /// 清楚INI文件中指定段落内容
        /// </summary>
        /// <param name="section">段落</param>
        public void ClearSection(string section)
        {
            IniWriteValue(section, null, null);
        }
    }
}