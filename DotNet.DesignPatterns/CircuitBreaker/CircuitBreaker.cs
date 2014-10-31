using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DesignPatterns.CircuitBreaker
{
    public class CircuitBreaker
    {
        private readonly object _monitor = new object();
        private CircuitBreakerState _state;
        public int FailureCount { get; private set; }
        public int ConsecutiveSuccessCount { get; private set; }
        public int FailureThreshold { get; private set; }
        public int ConsecutiveSuccessThreshold { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public Exception LastException { get; private set; }

        public CircuitBreaker(int failedthreshold, int consecutiveSuccessThreshold, TimeSpan timeout)
        {
            if (failedthreshold < 1 || consecutiveSuccessThreshold < 1)
            {
                throw new ArgumentOutOfRangeException("threshold", "Threshold should be greater than 0");
            }

            if (timeout.TotalMilliseconds < 1)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout should be greater than 0");
            }

            FailureThreshold = failedthreshold;
            ConsecutiveSuccessThreshold = consecutiveSuccessThreshold;
            Timeout = timeout;
            MoveToClosedState();
        }

        public bool IsClosed
        {
            get { return _state is ClosedState; }
        }

        public bool IsOpen
        {
            get { return _state is OpenState; }
        }

        public bool IsHalfOpen
        {
            get { return _state is HalfOpenState; }
        }

        internal void MoveToClosedState()
        {
            _state = new ClosedState(this);
        }

        internal void MoveToOpenState()
        {
            _state = new OpenState(this);
        }

        internal void MoveToHalfOpenState()
        {
            _state = new HalfOpenState(this);
        }

        internal void IncreaseFailureCount(Exception ex)
        {
            LastException = ex;
            FailureCount++;
        }

        internal void ResetFailureCount()
        {
            FailureCount = 0;
        }

        internal bool FailureThresholdReached()
        {
            return FailureCount >= FailureThreshold;
        }

        internal void IncreaseSuccessCount()
        {
            ConsecutiveSuccessCount++;
        }

        internal void ResetConsecutiveSuccessCount()
        {
            ConsecutiveSuccessCount = 0;
        }

        internal bool ConsecutiveSuccessThresholdReached()
        {
            return ConsecutiveSuccessCount >= ConsecutiveSuccessThreshold;
        }

        public void AttemptCall(Action protectedCode)
        {
            using (TimedLock.Lock(_monitor))
            {
                _state.ProtectedCodeIsAboutToBeCalled();
            }

            try
            {
                protectedCode();
            }
            catch (Exception e)
            {
                using (TimedLock.Lock(_monitor))
                {
                    _state.ActUponException(e);
                }
                throw;
            }

            using (TimedLock.Lock(_monitor))
            {
                _state.ProtectedCodeHasBeenCalled();
            }
        }
        public void Close()
        {
            using (TimedLock.Lock(_monitor))
            {
                MoveToClosedState();
            }
        }

        public void Open()
        {
            using (TimedLock.Lock(_monitor))
            {
                MoveToOpenState();
            }
        }
    }
}
