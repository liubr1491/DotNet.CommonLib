using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNet.DomainModel.MulitTask
{
    /// <summary> 
    /// 计划立即执行任务 
    /// </summary> 
    public struct ImmediateExecution : ISchedule
    {
        public DateTime ExecutionTime
        {
            get { return DateTime.Now; }
            set { }
        }

        public long DueTime
        {
            get { return 0; }
        }

        public long Period
        {
            get { return Timeout.Infinite; }
        }
    }
}
