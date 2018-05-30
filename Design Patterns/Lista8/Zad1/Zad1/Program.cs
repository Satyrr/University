using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zad1
{
    interface ICommand
    {
        void Execute();
    }

    class FTPCommand : ICommand
    {
        string _address;
        public FTPCommand(string addres)
        {
            this._address = addres;
        }

        public void Execute()
        {
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this._address); //ftp://speedtest.tele2.net/512KB.zip
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.  
            //request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            
            Stream responseStream = response.GetResponseStream();
            
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

            reader.Close();
            response.Close();
        }
    }

    class HTTPCommand : ICommand
    {
        string _address;
        public HTTPCommand(string addres)
        {
            this._address = addres;
        }

        public void Execute()
        {
            // Get the object used to communicate with the server.  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this._address); //
            request.Method = WebRequestMethods.Http.Get;

            // This example assumes the FTP site uses anonymous logon.  
            //request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

            reader.Close();
            response.Close();
        }
    }

    class NewFileCommand : ICommand
    {
        string _filename;
        public NewFileCommand(string filename)
        {
            this._filename = filename;
        }

        public void Execute()
        {
            using (FileStream f = File.Create(this._filename))
            {
                byte[] b = new byte[100];
                new Random().NextBytes(b);
                f.Write(b,0,100);
            }
        }
    }

    class CopyFileCommand : ICommand
    {
        string _source, _destination;
        public CopyFileCommand(string source, string destination)
        {
            this._source = source;
            this._destination = destination;
        }

        public void Execute()
        {
            using (FileStream f = File.Create(this._destination))
            {
                File.Open(this._source, FileMode.Open).CopyTo(f);
                f.Flush();
            }
        }
    }

    class Invoker
    {
        Queue<ICommand> _commands = new Queue<ICommand>();
        object _lock = new object();
        EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.ManualReset);

        public Invoker()
        {
            Thread t1 = new Thread(new ThreadStart(this.HandleCommands));
            Thread t2 = new Thread(new ThreadStart(this.HandleCommands));
            t1.Start();
            t2.Start();
        }

        public void AddCommand(ICommand command)
        {
            this._commands.Enqueue(command);
            ewh.Set();
        }

        private void HandleCommands()
        {
            while(true)
            {
                while(this._commands.Count == 0)
                {
                    ewh.Reset();
                    ewh.WaitOne();
                }

                ICommand c = _commands.Dequeue();
                c.Execute();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Invoker i = new Invoker();
            new Thread(() =>
            {
                i.AddCommand(new FTPCommand("ftp://speedtest.tele2.net/512KB.zip"));
                i.AddCommand(new HTTPCommand("https://www.google.pl/"));
                i.AddCommand(new NewFileCommand("nowy_plik.txt"));
                i.AddCommand(new CopyFileCommand("nowy_plik.txt", "nowy_plik2.txt"));
            }).Start();
        }
    }
}
