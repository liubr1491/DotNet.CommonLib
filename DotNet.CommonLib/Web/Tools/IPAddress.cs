using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNet.CommonLib.Web.Tools
{
    /// <summary>
    /// IPAddress 的摘要说明
    /// </summary>
    public class IPAddress
    {
        public static Int64 ToDenaryIp(string ip)
        {
            Int64 int64 = 0;
            string _ip = ip;
            if (_ip == null)
            {
                throw new ArgumentNullException("_ip");
            }
            if (_ip.LastIndexOf(".", StringComparison.Ordinal) <= -1)
            {
                return int64;
            }

            string[] iparray = _ip.Split('.');

            int64 = Int64.Parse(iparray.GetValue(0).ToString()) * 256 * 256 * 256 +
                    Int64.Parse(iparray.GetValue(1).ToString()) * 256 * 256 +
                    Int64.Parse(iparray.GetValue(2).ToString()) * 256 +
                    Int64.Parse(iparray.GetValue(3).ToString()) - 1;
            return int64;
        }

        /// <summary>
        /// /ip十进制
        /// </summary>
        public static Int64 DenaryIp
        {
            get
            {
                Int64 int64 = 0;
                string _ip = IP;
                if (_ip.LastIndexOf(".", StringComparison.Ordinal) <= -1)
                {
                    return int64;
                }

                string[] iparray = _ip.Split('.');
                int64 = Int64.Parse(iparray.GetValue(0).ToString()) * 256 * 256 * 256 +
                        Int64.Parse(iparray.GetValue(1).ToString()) * 256 * 256 +
                        Int64.Parse(iparray.GetValue(2).ToString()) * 256 +
                        Int64.Parse(iparray.GetValue(3).ToString()) - 1;
                return int64;
            }
        }

        public static string IP
        {
            get
            {
                string result = string.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理
                    if (result.IndexOf(".", StringComparison.Ordinal) == -1) //没有"."肯定是非IPv4格式
                    {
                        result = null;
                    }
                    else
                    {
                        if (result.IndexOf(",", StringComparison.Ordinal) != -1)
                        {
                            //有","，估计多个代理。取第一个不是内网的IP。
                            result = result.Replace(" ", "").Replace("", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            foreach (string tempIp in temparyip)
                            {
                                if (IsIpAddress(tempIp)
                                    && tempIp.Substring(0, 3) != "10."
                                    && tempIp.Substring(0, 7) != "192.168"
                                    && tempIp.Substring(0, 7) != "172.16.")
                                {
                                    return tempIp; //找到不是内网的地址
                                }
                            }
                        }
                        else if (IsIpAddress(result)) //代理即是IP格式
                        {
                            return result;
                        }
                        else
                        {
                            result = null; //代理中的内容 非IP，取IP
                        }
                    }
                }

                string ipAddress = !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ?
                    HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] :
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }

                return result;
            }
        }

        //是否ip格式
        public static bool IsIpAddress(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 7 || str.Length > 15)
            {
                return false;
            }

            const string regformat = @"^\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }

    }
}
