using Hospital.BLL.Helpers;
using Hospital.BLL.Services.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
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
        public async Task send(string to, string subject, string message)
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
            SmtpClient smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(options.SmtpServer, options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(options.From, options.Password);
            await smtpClient.SendAsync(ms);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
