using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.DomainModel.MulitTask
{
    /// <summary> 
    /// 任务管理中心 
    /// 使用它可以管理一个或则多个同时运行的任务 
    /// </summary> 
    public static class TaskScheduler
    {
        private static List<Task> taskScheduler;

        public static int Count
        {
            get { return taskScheduler.Count; }
        }

        static TaskScheduler()
        {
            taskScheduler = new List<Task>();
        }

        /// <summary>
        /// 查找任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Task Find(string name)
        {
            return taskScheduler.Find(task => task.Name == name);
        }

        public static IEnumerator<Task> GetEnumerator()
        {
            return taskScheduler.GetEnumerator();
        }

        /// <summary> 
        /// 终止任务 
        /// </summary> 
        public static void TerminateAllTask()
        {
            lock (taskScheduler)
            {
                taskScheduler.ForEach(task => task.Close());
                taskScheduler.Clear();
                taskScheduler.TrimExcess();
            }
        }

        internal static void Register(Task task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Add(task);
            }
        }

        internal static void Deregister(Task task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Remove(task);
            }
        }
    }
}
