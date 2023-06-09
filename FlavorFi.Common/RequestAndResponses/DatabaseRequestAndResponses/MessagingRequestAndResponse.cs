using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{

    public class SendEmailRequest : BaseRequest
    {
        public SendEmailRequest()
        {
            this.Recipients = new List<string>();
            this.From = string.Empty;
            this.Subject = string.Empty;
            this.Body = string.Empty;
        }

        public List<string> Recipients { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }

    public class SendEmailResponse : BaseResponse { }


    public class SendResetEmailRequest : BaseRequest
    {
        public SendResetEmailRequest()
        {
            this.Recipient = string.Empty;
            this.Url = string.Empty;
        }

        public string Recipient { get; set; }
        public string Url { get; set; }
    }


    public class SendCreatePasswordEmailRequest : BaseRequest
    {
        public SendCreatePasswordEmailRequest()
        {
            this.Recipient = string.Empty;
            this.Url = string.Empty;
        }

        public string Recipient { get; set; }
        public string Url { get; set; }
    }
}