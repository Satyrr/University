using NUnit.Framework;
using System;
using Zad1;
using System.IO;

namespace Testy
{
    [TestFixture]
    public class Zad1Tests
    {
        [Test]
        public void SingletonTest()
        {
            var loggerInstance = LoggerFactory.Instance();

            var loggerInstance2 = LoggerFactory.Instance();

            Assert.AreSame(loggerInstance, loggerInstance2);
        }

        [Test]
        public void FileLoggerTest()
        {
            ILogger fl = LoggerFactory.Instance().GetLogger(LogType.File, @"C:\Users\Satyr\source\repos\Lista6\Zad1\bin\Debug\netcoreapp2.0\log.txt");

            fl.Log("Przykładowy log");

            string log = new StreamReader(@"C:\Users\Satyr\source\repos\Lista6\Zad1\bin\Debug\netcoreapp2.0\log.txt").ReadToEnd();

            Assert.AreEqual(log, "Przykładowy log");
        }

        [Test]
        public void NullLoggerTest()
        {
            ILogger nullLogger = LoggerFactory.Instance().GetLogger(LogType.None);

            Assert.DoesNotThrow(() => nullLogger.Log("whatever"));
        }
    }
}
