using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Cache
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond,

        /// <summary>
        /// 秒
        /// </summary>
        Second,

        /// <summary>
        /// 分钟
        /// </summary>
        Minute,

        /// <summary>
        /// 小时
        /// </summary>
        Hour,

        /// <summary>
        /// 天
        /// </summary>
        Day
    }
}
