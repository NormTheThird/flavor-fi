using System;
using System.Runtime.Serialization;

namespace Shopify.Admin.Console.Models
{
    [DataContract]
    public class GiftCardModel : BaseModel
    {
        public GiftCardModel()
        {
            this.Id = 0;
            this.ApiClientId = 0;
            this.UserId = 0;
            this.OrderId = 0;
            this.LineItemId = 0;
            this.Balance = 0.0m;
            this.Currency = string.Empty;
            this.Code = string.Empty;
            this.Last4Characters = string.Empty;
            this.Note = string.Empty;
            this.TemplateSuffix = string.Empty;
            this.DisabledAt = null;
            this.ExpiresOn = null;
        }

        [DataMember(IsRequired = true)]
        public long Id { get; set; }
        [DataMember(IsRequired = true)]
        public long ApiClientId { get; set; }
        [DataMember(IsRequired = true)]
        public long UserId { get; set; }
        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }
        [DataMember(IsRequired = true)]
        public long LineItemId { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Balance { get; set; }
        [DataMember(IsRequired = true)]
        public string Currency { get; set; }
        [DataMember(IsRequired = true)]
        public string Code { get; set; }
        [DataMember(IsRequired = true)]
        public string Last4Characters { get; set; }
        [DataMember(IsRequired = true)]
        public string Note { get; set; }
        [DataMember(IsRequired = true)]
        public string TemplateSuffix { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DisabledAt { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? ExpiresOn { get; set; }
    }
}