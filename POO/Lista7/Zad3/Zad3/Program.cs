using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    abstract class Handler
    {
        protected Handler _succesor;
        public abstract void Handle(string message);

        public void SetSuccesor(Handler h)
        {
            this._succesor = h;
        }
    }

    class PraiseHandler : Handler
    {
        public override void Handle(string message)
        {
            if (message.Contains("Pochwała"))
                Mail.bossMail.Add(message);

            this._succesor?.Handle(message);
        }
    }

    class ComplainHandler : Handler
    {
        public override void Handle(string message)
        {
            if (message.Contains("Skarga"))
                Mail.legalDepartmentMail.Add(message);

            this._succesor?.Handle(message);
        }
    }

    class OrderHandler : Handler
    {
        public override void Handle(string message)
        {
            if (message.Contains("Zamówienie"))
                Mail.tradeDepartmentMail.Add(message);

            this._succesor?.Handle(message);
        }
    }

    class OtherHandler : Handler
    {
        public override void Handle(string message)
        {
            if (message.Contains("Witam"))
                Mail.marketingDepartmentMail.Add(message);

            this._succesor?.Handle(message);
        }
    }

    class ArchiveHandler : Handler
    {
        public override void Handle(string message)
        {
            Mail.archives.Add(message);

            this._succesor?.Handle(message);
        }
    }

    class Mail
    {
        public static List<string> bossMail,
                legalDepartmentMail,
                tradeDepartmentMail,
                marketingDepartmentMail;

        public static List<string> archives;

        static Mail()
        {
            Mail.archives = new List<string>();
            Mail.bossMail = new List<string>();
            Mail.legalDepartmentMail = new List<string>();
            Mail.marketingDepartmentMail = new List<string>();
            Mail.tradeDepartmentMail = new List<string>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PraiseHandler praiseHandler = new PraiseHandler();       
            OrderHandler orderHandler = new OrderHandler();       
            ComplainHandler complainHandler = new ComplainHandler();       
            OtherHandler otherHandler = new OtherHandler();       
            ArchiveHandler archiveHandler = new ArchiveHandler();

            praiseHandler.SetSuccesor(orderHandler);
            orderHandler.SetSuccesor(complainHandler);
            complainHandler.SetSuccesor(otherHandler);
            otherHandler.SetSuccesor(archiveHandler);

            praiseHandler.Handle("Pochwała, Witam");
            praiseHandler.Handle("Zamówienie, Pochwała");
            praiseHandler.Handle("aaaaaa");
            praiseHandler.Handle("Skarga, Witam");

            Console.WriteLine("Pochwały:");
            foreach(string mess in Mail.bossMail)
            {
                Console.WriteLine(mess);
            }
            Console.WriteLine("Skargi:");
            foreach (string mess in Mail.legalDepartmentMail)
            {
                Console.WriteLine(mess);
            }
            Console.WriteLine("Zamówienia:");
            foreach (string mess in Mail.tradeDepartmentMail)
            {
                Console.WriteLine(mess);
            }
            Console.WriteLine("Inne:");
            foreach (string mess in Mail.marketingDepartmentMail)
            {
                Console.WriteLine(mess);
            }
            Console.WriteLine("Archiwum:");
            foreach (string mess in Mail.archives)
            {
                Console.WriteLine(mess);
            }

            Console.Read();
        }
    }
}
