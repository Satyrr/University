using System;
using System.IO;
using System.Net.Mail;

namespace Lista5
{
    interface ISmtpFacade
    {
        void Send(string From, string To,
           string Subject, string Body,
           Stream Attachment, string AttachmentMimeType);
    }

    class SmtpFacadeTimeProxy : ISmtpFacade
    {
        ISmtpFacade _sf;

        public SmtpFacadeTimeProxy()
        {
            _sf = new SmtpFacade();
        }

        public SmtpFacadeTimeProxy(ISmtpFacade sf)
        {
            _sf = sf;
        }

        public void Send(string From, string To,
           string Subject, string Body,
           Stream Attachment, string AttachmentMimeType)
        {
            if (DateTime.Now.Hour < 8.0f || DateTime.Now.Hour > 22.0f)
            {
                throw new Exception("Próba wysłania maila w nocy");
            }

             _sf.Send(From, To, Subject, Body, Attachment, AttachmentMimeType);

        }


    }

    class SmtpFacadeLogProxy : IDisposable, ISmtpFacade
    {
        StreamWriter _logFile = File.CreateText("log.txt");
        ISmtpFacade _sf;

        public SmtpFacadeLogProxy()
        {
            _sf = new SmtpFacade();
        }

        public SmtpFacadeLogProxy(ISmtpFacade sf)
        {
            _sf = sf;
        }


        public void Dispose()
        {
            _logFile.Flush();
            _logFile.Dispose();
        }

        public void Send(string From, string To,
           string Subject, string Body,
           Stream Attachment, string AttachmentMimeType)
        {
            _logFile.WriteLine(string.Format("{5}, Metoda: Send, Mail from {0} to {1}, subject: {2}, body: {3}, Attachment type: {4}", 
                From, To, Subject, Body, AttachmentMimeType, DateTime.Now.ToLongDateString() ));

            try
            {
                _sf.Send(From, To, Subject, Body, Attachment, AttachmentMimeType);
            }
            catch(Exception e)
            {
                _logFile.WriteLine(string.Format("{0}, Error: {1}", DateTime.Now, e.Message));
            }
        }

        
    }

    class SmtpFacade : ISmtpFacade
    {
        public virtual void Send(string From, string To,
            string Subject, string Body,
            Stream Attachment, string AttachmentMimeType)
        {
            MailMessage message = new MailMessage(From, To);
            message.Subject = "Using the new SMTP client.";
            message.Body = @"Using this new feature, you can send an e-mail message from an application very easily.";
            if (Attachment != null)
                message.Attachments.Add(new Attachment(Attachment, AttachmentMimeType));

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.mailtrap.io",
                Port = 2525,
                EnableSsl = true
            };
            client.Credentials = new System.Net.NetworkCredential() { UserName = "23d404438b8d49", Password = "3d99abd170bbca" };


            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (SmtpFacadeProxy sf = new SmtpFacadeProxy())
            {
                sf.Send("aa@a.pl", "bb@b.pl", "Temat", "Tresc", null, "text");
                Console.Read();
            }
                
        }
    }
}