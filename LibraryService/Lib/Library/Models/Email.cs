using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Library.Models
{
    public class Email
    {
        private string smtpServer;
        private string usernae; // from
        private string password;
        private int port;
        public string topicMessage;
        public string message;

        public Email()
        {
            smtpServer = "smtp.gmail.com";
            usernae = ""; //write your email
            password = ""; //write your password
            port = 587;
            topicMessage = "Message";
            message = "You took the following books in our library: \n";


        }

        public void Send(string to, List<Book> books)
        {
            try
            {
                if (!string.IsNullOrEmpty(to))
                {
                    var messageText = message;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(usernae);
                    mail.To.Add(new MailAddress(to));
                    mail.Subject = topicMessage;
                    foreach (var book in books)
                    {
                        messageText += book.NameBook + "\n";
                    }
                    mail.Body = messageText;

                    SmtpClient client = new SmtpClient();
                    client.Host = smtpServer;
                    client.Port = port;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(usernae.Split('@')[0], password);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail);
                    client.Dispose();
                }

            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

    }
}