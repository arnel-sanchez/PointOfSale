using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Reciever { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public class ConfigurationEmailMessage
    {
        public string EmailTo { get; set; }
        
        public string Subject { get; set; }
        
        public string HtmlMessage { get; set; }

        public string EmailFrom { get; set; }

        public string Password { get; set; }

        public string NameApp { get; set; }
    }

    public interface IEmailSender
    {
        public Task SendEmailAsync(ConfigurationEmailMessage configuration);
    }

    public class EmailSender : IEmailSender
    {
        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            { Text = message.Content };
            return mimeMessage;
        }

        public async Task SendEmailAsync(ConfigurationEmailMessage configuration)
        {
            EmailMessage message = new EmailMessage();
            message.Sender = new MailboxAddress(configuration.NameApp, configuration.EmailFrom);
            message.Reciever = new MailboxAddress(configuration.NameApp, configuration.EmailTo);
            message.Subject = configuration.Subject;
            message.Content = configuration.HtmlMessage;
            var mimeMessage = CreateMimeMessageFromEmailMessage(message);
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 465, true);
                await smtpClient.AuthenticateAsync(configuration.EmailFrom, configuration.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
