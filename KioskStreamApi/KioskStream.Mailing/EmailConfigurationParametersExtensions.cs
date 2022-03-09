
using System;

namespace KioskStream.Mailing
{
    public static class EmailConfigurationParametersExtensions
    {
        public static EmailConfigurationParameters BuildNewUserConfirmationEmail(this EmailConfigurationParameters emailMessage, ConfirmationEmailParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            emailMessage.Body = parameters.Template
                .Replace("{userName}", parameters.UserName)
                .Replace("{callbackUrl}", parameters.CallbackUrl)
                ;

            emailMessage.Subject = $"{parameters.UserName}, please confirm the registration.";

            return emailMessage;
        }

        public static EmailConfigurationParameters BuildProvideFeedbackEmail(this EmailConfigurationParameters emailMessage, ProvideFeedbackParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            emailMessage.Body = parameters.Template
                .Replace("{userName}", parameters.Reviewer)
                .Replace("{callbackUrl}", parameters.CallbackUrl)
                .Replace("{requester}", parameters.Requester)
                .Replace("{reviewee}", parameters.Reviewee)
                .Replace("{deadline}", parameters.Deadline.ToString("MMMM dd, yyyy"))
                ;

            emailMessage.Subject = $"Feedback Request {parameters.FeedbackId}";

            return emailMessage;
        }
    }
}
