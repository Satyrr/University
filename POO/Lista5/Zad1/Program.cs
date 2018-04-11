using System;
using System.IO;
using System.Net.Mail;

namespace Lista5
{
    class SmtpFacade
    {
        public void Send(string From, string To,
            string Subject, string Body,
            Stream Attachment, string AttachmentMimeType)
        {
            MailMessage message = new MailMessage(From, To);
            message.Subject = "Using the new SMTP client.";
            message.Body = @"Using this new feature, you can send an e-mail message from an application very easily.";
            if(Attachment != null)
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
            SmtpFacade sf = new SmtpFacade();
            sf.Send("aa@a.pl", "bb@b.pl", "Temat", "Tresc", null, "text");
            Console.Read();
        }
    }
}