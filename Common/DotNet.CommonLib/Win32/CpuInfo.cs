using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Win32
{
    public class CpuInfo
    {
        /// <summary>
        /// 输出CPU信息
        /// </summary>
        /// <returns></returns>
        public static string GetCpuInfo()
        {
            //总共条数
            int totalHits = 0;
            StringBuilder sb = new StringBuilder();
            int cpuPercent = Convert.ToInt32(GetCpuCounter());
            if (cpuPercent >= 90)
            {
                totalHits = totalHits + 1;
                if (totalHits == 60)
                {
                    //发送邮件
                }
                totalHits = 0;
            }
            else
            {
                totalHits = 0;
            }
            sb.Append(cpuPercent + " % CPU" + "\r\n");
            //sb.Append(getRAMCounter() + " RAM Free" + "\r\n");
            sb.Append(totalHits + " seconds over 20% usage" + "\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        private static object GetCpuCounter()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            // will always start at 0 
            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            // now matches task manager reading 
            dynamic secondValue = cpuCounter.NextValue();
            return secondValue;
        }
    }
}
