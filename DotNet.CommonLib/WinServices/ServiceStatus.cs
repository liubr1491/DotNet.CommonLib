using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.WinServices
{
    /// <summary>
    /// 服务状态
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        /// 启动状态
        /// </summary>
        Started,

        /// <summary>
        /// 启动失败
        /// </summary>
        StartError,

        /// <summary>
        /// 停止状态
        /// </summary>
        Stopped,

        /// <summary>
        /// 停止失败
        /// </summary>
        StopError
    }
}
