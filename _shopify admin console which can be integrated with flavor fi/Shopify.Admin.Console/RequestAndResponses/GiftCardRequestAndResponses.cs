using Shopify.Admin.Console.Models;
using System.Runtime.Serialization;

namespace Shopify.Admin.Console.RequestAndResponses
{
    [DataContract]
    public class SaveGiftCardRequest : BaseRequest
    {
        public SaveGiftCardRequest()
        {
            this.UserId = 0;
            this.InitialValue = 0.0m;
        }

        [DataMember(IsRequired = true)]
        public long UserId { get; set; }
        [DataMember(IsRequired = true)]
        public decimal InitialValue { get; set; }

    }

    [DataContract]
    public class SaveGiftCardResponse : BaseResponse
    {
        public SaveGiftCardResponse()
        {
            this.GiftCard = new GiftCardModel();
        }

        [DataMember(IsRequired = true)]
        public GiftCardModel GiftCard { get; set; }
    }
}