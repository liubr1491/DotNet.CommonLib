using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.DomainModel.MulitTask
{
    /// <summary> 
    /// 周期性的执行计划 
    /// </summary> 
    public struct CycExecution : ISchedule
    {
        private DateTime schedule;
        private TimeSpan period;

        public DateTime ExecutionTime
        {
            get { return schedule; }
            set { schedule = value; }
        }

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
            get { return period.Ticks / 10000; }
        }

        /// <summary> 
        /// 构造函数,马上开始运行 
        /// </summary> 
        /// <param name="period">周期时间</param> 
        public CycExecution(TimeSpan period)
        {
            this.schedule = DateTime.Now;
            this.period = period;
        }

        /// <summary> 
        /// 构造函数，在一个将来时间开始运行 
        /// </summary> 
        /// <param name="shedule">计划执行的时间</param> 
        /// <param name="period">周期时间</param> 
        public CycExecution(DateTime shedule, TimeSpan period)
        {
            this.schedule = shedule;
            this.period = period;
        }
    }
}
