using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DotNet.DesignPatterns.CircuitBreaker
{
    public class OpenState : CircuitBreakerState
    {
        private readonly Timer timer;

        public OpenState(CircuitBreaker circuitBreaker)
            : base(circuitBreaker)
        {
            timer = new Timer(circuitBreaker.Timeout.TotalMilliseconds);
            timer.Elapsed += TimeoutHasBeenReached;
            timer.AutoReset = false;
            timer.Start();
        }

        // 断开超过设定的阈值，自动切换到半断开状态
        private void TimeoutHasBeenReached(object sender, ElapsedEventArgs e)
        {
            CircuitBreaker.MoveToHalfOpenState();
        }

        public override void ProtectedCodeIsAboutToBeCalled()
        {
            base.ProtectedCodeIsAboutToBeCalled();
            throw new OpenCircuitException();
        }
    }
}
