using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lista4
{
    public class Singleton
    {
        private static Singleton _instance;
        private static object mutexLock = new object();

        private Singleton()
        {

        }

        public static Singleton GetInstance()
        {
            if(_instance == null)
            {
                lock(mutexLock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }

            return _instance;
        }
    }

    public class ThreadSingleton
    {
        private static Dictionary<int, ThreadSingleton> _threadInstances = new Dictionary<int, ThreadSingleton>();
        private static object mutexLock = new object();

        private ThreadSingleton()
        {
        }

        public static ThreadSingleton GetInstance()
        {
            
            if (!_threadInstances.Keys.Contains(Thread.CurrentThread.ManagedThreadId))
            {
                lock(mutexLock)
                {
                    if (!_threadInstances.Keys.Contains(Thread.CurrentThread.ManagedThreadId))
                        _threadInstances[Thread.CurrentThread.ManagedThreadId] = new ThreadSingleton();
                }
            }

            return _threadInstances[Thread.CurrentThread.ManagedThreadId];
        }
    }

    public class TimeSingleton
    {
        private static TimeSingleton _instance;
        private static object mutexLock = new object();
        private static DateTime lastCreated;

        private TimeSingleton()
        {

        }

        public static TimeSingleton GetInstance()
        {
            if (_instance == null || (DateTime.Now - lastCreated).Seconds > 5 )
            {
                lock (mutexLock)
                {
                    if (_instance == null || (DateTime.Now - lastCreated).Seconds > 5)
                    {
                        _instance = new TimeSingleton();
                        lastCreated = DateTime.Now;
                    }
                }
            }

            return _instance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
