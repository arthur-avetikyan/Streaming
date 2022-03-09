
using System;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using KioskStream.Core.Configurations;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace KioskStream.Mailing
{
    public class EmailProcessor : IEmailProcessor
    {
        private readonly MailingServiceConfiguration _mailingServiceConfiguration;

        public EmailProcessor(IOptions<MailingServiceConfiguration> mailingServiceConfiguration)
        {
            _mailingServiceConfiguration = mailingServiceConfiguration.Value;
        }

        public async Task SendAsync(EmailConfigurationParameters emailMessage)
        {
            try
            {
                var message = new MimeMessage();

                // Set From Address it was not set
                if (emailMessage.SourceAddresses.Count == 0)
                {
                    emailMessage.SourceAddresses.Add(_mailingServiceConfiguration.FromAddress);
                }

                message.To.AddRange(emailMessage.DestinationAddresses.Select(x => new MailboxAddress(x)));
                message.From.AddRange(emailMessage.SourceAddresses.Select(x => new MailboxAddress(x)));
                message.Cc.AddRange(emailMessage.CcAddresses.Select(x => new MailboxAddress(x)));
                message.Bcc.AddRange(emailMessage.BccAddresses.Select(x => new MailboxAddress(x)));

                //Use for testing - send a copy of any email to from address when in debug mode
                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                //    message.To.Clear();
                //    message.To.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.FromAddress));
                //}

                message.Subject = emailMessage.Subject;

                message.Body = emailMessage.IsHtml ? new BodyBuilder { HtmlBody = emailMessage.Body }.ToMessageBody() : new TextPart("plain") { Text = emailMessage.Body };

                //TODO store all emails in Database

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    if (!_mailingServiceConfiguration.SmtpUseSsl)
                    {
                        emailClient.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                    }

                    await emailClient.ConnectAsync(_mailingServiceConfiguration.SmtpServer, _mailingServiceConfiguration.SmtpPort, _mailingServiceConfiguration.SmtpUseSsl).ConfigureAwait(false);

                    //Remove any OAuth functionality as we won't be using it.
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    if (!string.IsNullOrWhiteSpace(_mailingServiceConfiguration.SmtpUsername))
                    {
                        await emailClient.AuthenticateAsync(_mailingServiceConfiguration.SmtpUsername, _mailingServiceConfiguration.SmtpPassword).ConfigureAwait(false);
                    }

                    await emailClient.SendAsync(message).ConfigureAwait(false);

                    await emailClient.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                return;
                //_logger.LogError("Email Send Failed: {0}", ex.Message);
                //return new ApiResponse(Status500InternalServerError, ex.Message);
            }
        }
    }
}
