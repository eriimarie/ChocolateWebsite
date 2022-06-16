using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChocoShop.Extensions
{
    public static class StringExtensions
    {
        public static string ToHash(this string unHashed)
        {
            using (var x = new HMACMD5 { Key = Encoding.ASCII.GetBytes("PKey") })
            {
                var data = Encoding.ASCII.GetBytes(unHashed);
                data = x.ComputeHash(data);
                return Encoding.ASCII.GetString(data);
            }
        }

        public static void SendEmail(string subject, string to, string from, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            var username = "noemail@gmail.com";
            var password = "No_Password";
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(username, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }
    }
}
