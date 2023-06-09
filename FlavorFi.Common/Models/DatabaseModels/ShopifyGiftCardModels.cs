using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyGiftCardModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid? ShopifyUserId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? ShopifyCustomerId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? ShopifyOrderId { get; set; } = null;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyUserId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyCustomerId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyLineItemId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyApiClientId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string LastFour { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Note { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal Balance { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal InitialValue { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTime.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTime.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset? DisabledAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset? ExpiresOn { get; set; } = null;
    }

    [DataContract]
    public class ShopifyGiftCardReportExportModel
    {
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string LastFour { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string CustomerName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Currency { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Email { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string OrderName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal InitialValue { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal Balance { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public bool Expired { get; set; } = false;
        [DataMember(IsRequired = true)] public bool Enabled { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTime.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset? DisabledAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset? ExpiresOn { get; set; } = null;
    }

    [DataContract]
    public class ShopifyGiftCardTotalsModel
    {
        [DataMember(IsRequired = true)] public decimal GiftCardTotalAmount { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal GiftCardAmountFromReturns { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal RemainingTotalBalance { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal RemainingReturnBalance { get; set; } = 0.0m;
    }

    [DataContract]
    public class ShopifyGiftCardMonthlyTotalsModel
    {
        [DataMember(IsRequired = true)] public int Month { get; set; } = 0;
        [DataMember(IsRequired = true)] public decimal GiftCardTotalAmount { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal GiftCardAmountFromReturns { get; set; } = 0.0m;
    }
}