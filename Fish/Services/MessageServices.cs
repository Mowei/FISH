using System.Threading.Tasks;
using FISH.Utility;
using MailKit.Net.Smtp;
using MimeKit;

namespace FISH.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        /*
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        */

        //public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return ExecuteAsync(subject, message, email );
        }

        public async Task ExecuteAsync(string subject, string message, string email)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("TESTMAIL", "mowei.tw@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "utron.com.tw";
                await client.ConnectAsync(CommEnvironment.SmtpHost, CommEnvironment.SmtpPort, false).ConfigureAwait(false);
                client.Authenticate(CommEnvironment.SmtpAccount, CommEnvironment.SmtpPassWord);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            //Test using mail
            // Plug in your SMS service here to send a text message.
            return ExecuteAsync("簡訊認證碼", message, number);
        }
    }
}
