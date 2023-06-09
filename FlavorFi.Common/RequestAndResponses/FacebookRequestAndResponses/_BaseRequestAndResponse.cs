using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.FacebookRequestAndResponses
{
    [DataContract]
    public class BaseRequest
    {
        public BaseRequest()
        {

        }

    }

    [DataContract]
    public class BaseResponse
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
            this.ErrorMessage = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public bool IsSuccess { get; set; }
        [DataMember(IsRequired = true)]
        public string ErrorMessage { get; set; }
    }
}