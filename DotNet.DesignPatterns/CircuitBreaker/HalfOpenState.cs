using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DesignPatterns.CircuitBreaker
{
    public class HalfOpenState : CircuitBreakerState
    {
        public HalfOpenState(CircuitBreaker circuitBreaker)
            : base(circuitBreaker)
        {
            // 重置连续成功计数
            circuitBreaker.ResetConsecutiveSuccessCount();
        }

        public override void ActUponException(Exception e)
        {
            base.ActUponException(e);
            // 只要有失败，立即切换到断开模式
            CircuitBreaker.MoveToOpenState();
        }

        public override void ProtectedCodeHasBeenCalled()
        {
            base.ProtectedCodeHasBeenCalled();
            // 如果连续成功次数达到阈值，切换到闭合状态
            if (CircuitBreaker.ConsecutiveSuccessThresholdReached())
            {
                CircuitBreaker.MoveToClosedState();
            }
        }
    }
}
