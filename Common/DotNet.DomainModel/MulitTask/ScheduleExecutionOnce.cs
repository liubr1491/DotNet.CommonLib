using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNet.DomainModel.MulitTask
{
    /// <summary> 
    /// 计划在某一未来的时间执行一个操作一次，如果这个时间比现在的时间小，就变成了立即执行的方式 
    /// </summary> 
    public struct ScheduleExecutionOnce : ISchedule
    {
        private DateTime schedule;

        public DateTime ExecutionTime
        {
            get { return schedule; }
            set { schedule = value; }
        }

        /// <summary> 
        /// 得到该计划还有多久才能运行 
        /// </summary> 
        public long DueTime
        {
            get
            {
                long ms = (schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0)
                {
                    ms = 0;
                }
                return ms;
            }
        }

        public long Period
        {
            get { return Timeout.Infinite; }
        }

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="schedule">计划开始执行的时间</param> 
        public ScheduleExecutionOnce(DateTime time)
        {
            schedule = time;
        }
    }
}
