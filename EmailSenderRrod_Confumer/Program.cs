using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;

using System.Collections.Concurrent;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading;
using System.Threading.Tasks;

using EmailSenderRrod_Confumer.Models;
using System.Net.Mail;
using SmtpStatusCode = System.Net.Mail.SmtpStatusCode;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailSenderRrod_Confumer
{
    class Program
    {
        public static string AdminEmail = "justfortestsfaludore@gmail.com";
        public static string AdminPassword = "TestForJust123";
        static public void Main()
        {
            
            new Program().Run();
            Console.ReadKey();
        }

        BlockingCollection<Mail> q = new BlockingCollection<Mail>()  ;

        void Run()
        {
            var threads = new[] { new Thread(Consumer), new Thread(Consumer) };
            foreach (var t in threads)
                t.Start();

          
           
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "highkhkny54t", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hifghfghfgh", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghjghjghfj", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hihgkfghkgh", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "highkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghkghkf", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hihgkfghkgh", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghjghjghfj", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hihgkfghkgh", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "highkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "highkghk", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });
            q.Add(new Mail { EmailFrom = AdminEmail, EmailTo = "some@gmail.com", Message = "hi123ghkghkf", DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true });

            q.CompleteAdding(); // останавливаем

            foreach (var t in threads)
                t.Join();
        }

        void Consumer()
        {
            foreach (var s in q.GetConsumingEnumerable())
            {             
                Console.WriteLine("Sending: {0}, to {1} with message: {2}", s.EmailFrom, s.EmailTo, s.Message);            
                SendMail(s.EmailTo, s.EmailFrom, AdminPassword, s.Message);
                Thread.Sleep(2000);                
                Console.WriteLine("Sended: {0}, to {1} with message: {2}", s.EmailFrom, s.EmailTo, s.Message);
         
            }
        }
        void SendMail(string emailTo, string emailFrom, string password, string message )
        {

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Site admin", emailFrom));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Subject = "Register on day book";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(emailFrom, password);              
                try
                {
                    client.Send(emailMessage);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                        {
                            Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                            System.Threading.Thread.Sleep(5000);
                            client.Send(emailMessage);
                        }
                        else
                        {
                            Console.WriteLine("Failed to deliver message to {0}",
                                ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
                client.Disconnect(true);
            }  

            //add to db
            var optionsBuilder = new DbContextOptionsBuilder<EmailSenderDbContext>();   
            var options = optionsBuilder
                    .UseSqlServer(@"Server=(local)\sqlexpress;Database=EmailSenderDb;Trusted_Connection=True;")
                    .Options;
            using (EmailSenderDbContext db = new EmailSenderDbContext(options))
            {
                Mail mail = new Mail { EmailFrom = emailFrom, EmailTo = emailTo, Message = message, DnT = DateTime.Now.ToString("dd/MM/yyyy"), Status = true };

                db.Mails.Add(mail);
                db.SaveChanges();
            }
        }


    }
}
