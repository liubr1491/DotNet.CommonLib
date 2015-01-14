using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Win32
{
    public class Win32Utility
    {
        public static decimal ConvertBytes(string b, int iteration)
        {
            long iter = 1;
            for (int i = 0; i < iteration; i++)
            {
                iter *= 1024;
            }
            return Math.Round((Convert.ToDecimal(b)) / Convert.ToDecimal(iter), 2, MidpointRounding.AwayFromZero);
        }
    }
}
