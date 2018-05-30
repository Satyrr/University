using System;
using System.IO;

namespace Zad1
{
    public interface ILogger
    {
        void Log(string Message);
    }
    public enum LogType { None, Console, File }

    public class ConsoleLogger : ILogger
    {
        public void Log(string Message)
        {
            Console.WriteLine(Message);
        }
    }

    public class FileLogger : ILogger
    {
        StreamWriter _FileWriter;
        public FileLogger(string FilePath)
        {
            _FileWriter = new StreamWriter(FilePath);
        }

        public void Log(string Message)
        {
            _FileWriter.Write(Message);
            _FileWriter.Dispose();
        }
    }

    public class NullLogger : ILogger
    {
        public void Log(string Message)
        {
        }
    }

    public class LoggerFactory
    {
        private static LoggerFactory _instance;
        public ILogger GetLogger(LogType LogType, string Parameters = null)
        {
            switch (LogType)
            {
                case LogType.File:
                    return new FileLogger(Parameters);
                case LogType.Console:
                    return new ConsoleLogger();
                default:
                    return new NullLogger();
            }

        }

        public static LoggerFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new LoggerFactory();
            }

            return _instance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
