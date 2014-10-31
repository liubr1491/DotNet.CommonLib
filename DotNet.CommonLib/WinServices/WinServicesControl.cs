using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace DotNet.CommonLib.WinServices
{
    public class WinServicesControl
    {
        /// <summary>
        /// 如果Windows服务处于停止状态，启动Windows服务
        /// </summary>
        /// <param name="serviceName">Windows服务名</param>
        /// <returns></returns>
        public static ServiceStatus StartWinServices(string serviceName)
        {
            var serviceControllers = ServiceController.GetServices();
            // 0表示未启动状态
            var serviceState = ServiceStatus.Stopped;
            foreach (var serviceController in serviceControllers)
            {
                if (serviceController.DisplayName.Contains(serviceName) &&
                    serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        serviceController.Start();
                        // 让线程睡眠1秒钟，等待其他服务的加载，不设置则会出错
                        Thread.Sleep(1000);
                        serviceState = ServiceStatus.Started;
                    }
                    catch (Exception)
                    {
                        // 2 表示启动失败
                        serviceState = ServiceStatus.StartError;
                        throw;
                    }
                }
                else
                {
                    serviceState = ServiceStatus.Stopped;
                }
            }
            return serviceState;
        }

        /// <summary>
        /// 停止Windows服务
        /// </summary>
        /// <param name="serviceName">Windows服务名</param>
        /// <returns></returns>
        public static ServiceStatus StopWinServices(string serviceName)
        {
            var serviceControllers = ServiceController.GetServices();
            var serviceStatus = ServiceStatus.Started;
            foreach (var serviceController in serviceControllers)
            {
                if (serviceController.DisplayName.Contains(serviceName))
                {
                    try
                    {
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        serviceController.Stop();
                        serviceStatus = ServiceStatus.Stopped;
                    }
                    catch (Exception)
                    {
                        serviceStatus = ServiceStatus.StopError;
                        throw;
                    }
                }
            }
            return serviceStatus;
        }

        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <param name="serviceName"></param>
        public static void GetWinServicesInfo(string serviceName)
        {
            var version = string.Empty;
            var path = string.Empty;
            if (HasInstalledService(serviceName, out version, out path))
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// 检测是否安装了服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool HasInstalledService(string serviceName, out string version, out string path)
        {
            version = string.Empty;
            path = string.Empty;

            foreach (var service in ServiceController.GetServices().Where(x => string.Compare(x.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
