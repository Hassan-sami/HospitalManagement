using Hospital.BLL.Helpers;
using Hospital.BLL.Services.Abstraction;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;


namespace Hospital.BLL.Services.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions options;

        public EmailSender(IOptionsMonitor<EmailOptions> options)
        {
            this.options = options.CurrentValue;
        }
        public  async Task<bool> send(string to, string subject, string message)
        {
            var ms = new MimeMessage
            {
                Sender = MailboxAddress.Parse(options.From),
                Subject = subject

            };
            ms.To.Add(MailboxAddress.Parse(to));
            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };
            ms.Body = bodyBuilder.ToMessageBody();
            ms.From.Add(new MailboxAddress("your hospital", options.From));
            using (SmtpClient smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.Connect(options.SmtpServer, options.Port, SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(options.From, options.Password);
                     await smtpClient.SendAsync(ms);
                    smtpClient.Disconnect(true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
                
        }
    }
}
