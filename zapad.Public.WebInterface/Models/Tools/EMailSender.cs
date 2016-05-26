using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace zapad.Public.WebInterface.Models.Tools
{
    /// <summary>
    /// Хелпер для отправки сообщений электронной почты
    /// </summary>
    public class EMailSender
    {
        public string SmtpServer { get; set; }
        public bool SmtpServerEnableSSL { get; set; } = true;
        public int SmptServerPort { get; set; } = 25;

        public string FromEMail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Создает и отправляет сообщение электронной почты
        /// </summary>
        /// <param name="ToEMail">Адрес получателя</param>
        /// <param name="Subject">Тема письма</param>
        /// <param name="Body">Тело письма</param>
        /// <param name="priority">Приоритет письма</param>
        public void Send(string ToEMail, string Subject, string Body, MailPriority priority = MailPriority.Normal)
        {
            using (MailMessage m = new MailMessage())
            {
                m.From = new MailAddress(this.FromEMail);
                m.To.Add(ToEMail);
                m.Priority = priority;
                m.IsBodyHtml = false;
                m.Subject = Subject;
                m.Body = Body;

                SmtpClient smtp = new SmtpClient(this.SmtpServer);
                smtp.EnableSsl = this.SmtpServerEnableSSL;
                smtp.Port = this.SmptServerPort;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(this.UserName, this.Password);
                smtp.Send(m);
            }
        }
    }
}