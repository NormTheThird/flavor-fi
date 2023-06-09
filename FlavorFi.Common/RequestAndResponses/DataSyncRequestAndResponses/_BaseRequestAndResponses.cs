using System;

namespace FlavorFi.Common.RequestAndResponses.DataSyncRequestAndResponses
{
    public class BaseRequest
    {
        public BaseRequest()
        {
            this.CompanySiteId = Guid.Empty;
            this.UserToken = string.Empty;
            this.SessionId = string.Empty;
        }

        public Guid CompanySiteId { get; set; }
        public string UserToken { get; set; }
        public string SessionId { get; set; }
    }

    public class BaseResponse
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
            this.IsTokenBad = false;
            this.ErrorMessage = string.Empty;
        }

        public bool IsSuccess { get; set; }
        public bool IsTokenBad { get; set; }
        public string ErrorMessage { get; set; }
    }
}