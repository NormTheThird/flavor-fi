using System.Runtime.Serialization;

namespace Shopify.Admin.Console.RequestAndResponses
{
    [DataContract]
    public class BaseRequest { }

    [DataContract]
    public class BaseResponse
    {
        public BaseResponse()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public bool IsSuccess { get; set; }
        [DataMember(IsRequired = true)]
        public string ErrorMessage { get; set; }
    }
}