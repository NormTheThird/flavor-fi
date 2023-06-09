using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.ShopifyModels
{
    [DataContract]
    public class ShopifyTransactionModel : ShopifyBaseModel
    {
        [DataMember(IsRequired = true)] public string Amount { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string AuthorizationCode { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public long DeviceId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Gateway { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string SourceName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public ShopifyPaymentDetailModel PaymentDetails { get; set; } = new ShopifyPaymentDetailModel();
        [DataMember(IsRequired = true)] public string Kind { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public long OtherId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string ErrorCode { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Status { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public bool Test { get; set; } = false;
        [DataMember(IsRequired = true)] public long UserId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Currency { get; set; } = string.Empty;
    }
}