using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.IO
{
    public static class ReadFile
    {
        public static string ReadFileByStreamReader(string filePath)
        {
            var objReader = new StreamReader(filePath, Encoding.UTF8);
            var sLine = "";
            var sb = new StringBuilder();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(""))
                {
                    sb.AppendLine(sLine);
                }
            }
            objReader.Close();
            return sb.ToString();
        }
    }
}
