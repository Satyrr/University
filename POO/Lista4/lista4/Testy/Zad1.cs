using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using lista4;
using System.Threading;

namespace Zad1Tests
{
    [TestFixture]
    public class Zad1Test
    {
        [Test]
        public void SingletonTest()
        {
            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();
            Assert.AreEqual(s1, s2);
        }

        [Test]
        public void ThreadSingletonTest()
        {
            
            new Thread(() =>
            {
                ThreadSingleton st1;
                ThreadSingleton st2;
                st1 = ThreadSingleton.GetInstance();
                new Thread(() =>
                {
                    st2 = ThreadSingleton.GetInstance();
                    Assert.AreNotEqual(st1, st2);
                });
            });

            ThreadSingleton s1;
            ThreadSingleton s2;

            s1 = ThreadSingleton.GetInstance();
            s2 = ThreadSingleton.GetInstance();
            Assert.AreEqual(s1, s2);
            
        }

        [Test]
        public void TimeSingletonTest()
        {
            TimeSingleton ts1 = TimeSingleton.GetInstance();
            TimeSingleton ts2 = TimeSingleton.GetInstance();
            Assert.AreEqual(ts1, ts2);

            ts1 = TimeSingleton.GetInstance();
            Thread.Sleep(6000);
            ts2 = TimeSingleton.GetInstance();
            Assert.AreNotEqual(ts1, ts2);

        }
    }
}
