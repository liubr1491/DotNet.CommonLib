using System;
using System.IO;
using System.Text;

namespace DotNet.CommonLib.Log
{
    public class Log
    {
        /// <summary>
        /// 程序当前目录
        /// </summary>
        private readonly DirectoryInfo _dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        /// <summary>
        /// 默认日志文件最大数量为20
        /// </summary>
        private readonly int _maxLogNum = 20;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxLogNum">Log目录下日志文件的最大数量</param>
        public Log(int maxLogNum)
        {
            _maxLogNum = maxLogNum;
        }

        /// <summary>
        /// 字符串写入日志文件
        /// </summary>
        /// <param name="msg">写入的字符串文本</param>
        public void WriteLog_Txt(string msg)
        {
            FileStream stream = null;
            var sb = new StringBuilder();
            var path = _dir + "Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var str2 = path + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ");
            sb.Append(msg);
            var bytes = Encoding.UTF8.GetBytes(sb + "\r\n");
            try
            {
                if (!File.Exists(str2))
                {
                    DeleteUnnecessaryLogFiles();
                }
                stream = File.OpenWrite(str2);
                stream.Position = stream.Length;
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine("文件打开失败{0}", exception.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// 字节数组写入日志文件
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="data">字节数组</param>
        public void WriteLog_Bytes(string msg, byte[] data)
        {
            FileStream stream = null;
            var sb = new StringBuilder();
            var path = _dir + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var str2 = path + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ");
            sb.Append(msg);
            foreach (var num in data)
            {
                sb.AppendFormat("{0:x2} ", num);
            }
            var bytes = Encoding.UTF8.GetBytes(sb + "\r\n");
            try
            {
                if (!File.Exists(str2))
                {
                    DeleteUnnecessaryLogFiles();
                }
                stream = File.OpenWrite(str2);
                stream.Position = stream.Length;
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine("文件打开失败{0}", exception.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// 删除Log目录多余的日志文件
        /// </summary>
        public void DeleteUnnecessaryLogFiles()
        {
            var path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"Log\");
            var fileInfos = path.GetFiles("*.log");
            Array.Sort(fileInfos, new FileSorter());
            if (fileInfos.Length <= (_maxLogNum - 1))
            {
                return;
            }
            for (var i = _maxLogNum - 1; i < fileInfos.Length; i++)
            {
                var filepath = fileInfos[i].FullName;
                if (!File.Exists(filepath))
                {
                    continue;
                }
                try
                {
                    File.Delete(filepath);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}