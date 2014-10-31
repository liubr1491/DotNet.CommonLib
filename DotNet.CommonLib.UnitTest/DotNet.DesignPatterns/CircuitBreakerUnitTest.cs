using System;
using System.Threading;
using DotNet.DesignPatterns.CircuitBreaker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.CommonLib.UnitTest.DotNet.DesignPatterns
{
    [TestClass]
    public class CircuitBreakerUnitTest
    {
        [TestMethod]
        public void ClosesIfProtectedCodeSucceedsInHalfOpenState()
        {
            var stub = new Stub(10);
            // 定义熔断器，失败10次进入断开状态
            // 5秒后进入半断开状态
            // 在半断开状态下，连续成功15次，进入闭合状态
            var circuitBreaker = new CircuitBreaker(10, 15, TimeSpan.FromMilliseconds(5000));
            Assert.IsTrue(circuitBreaker.IsClosed);
            // 失败10次调用
            CallXAmountOfTimes(() => AssertThatExceptionIsThrown<ApplicationException>(() => circuitBreaker.AttemptCall(stub.DoStuff)), 10);

            Assert.AreEqual(10, circuitBreaker.FailureCount);

            Assert.IsTrue(circuitBreaker.IsOpen);

            // 等待从Open转到HalfOpen
            Thread.Sleep(6000);
            Assert.IsTrue(circuitBreaker.IsHalfOpen);
            // 成功调用15次
            CallXAmountOfTimes(() => circuitBreaker.AttemptCall(stub.DoStuff), 15);

            Assert.AreEqual(15, circuitBreaker.ConsecutiveSuccessCount);
            Assert.AreEqual(0, circuitBreaker.FailureCount);
            Assert.IsTrue(circuitBreaker.IsClosed);
        }

        private static void CallXAmountOfTimes(Action codeToCall, int timesToCall)
        {
            for (int i = 0; i < timesToCall; i++)
            {
                codeToCall();
            }
        }

        private static void AssertThatExceptionIsThrown<T>(Action code) where T : Exception
        {
            try
            {
                code();
            }
            catch (T)
            {
                return;
            }

            Assert.Fail("Expected exception of type {0} was not thrown", typeof(T).FullName);
        }
    }

    public class Stub
    {
        public Stub(int count) { }

        internal void DoStuff()
        {
            throw new NotImplementedException();
        }
    }
}
