using System;

namespace KioskStream.Mailing
{
    public class ProvideFeedbackParameters
    {
        public string Recipient { get; set; }

        public string Reviewer { get; set; }

        public string CallbackUrl { get; set; }

        public int FeedbackId { get; set; }

        public string Template { get; set; }

        public DateTime Deadline { get; set; }

        public string Requester { get; set; }

        public string Reviewee { get; set; }
    }
}
