using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class ShopifyOrderForPickupModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OrderNumber { get; set; } = 0;
        [DataMember(IsRequired = true)] public DateTimeOffset OrderDate { get; set; } = DateTime.MinValue;
        [DataMember(IsRequired = true)] public string FullName { get; set; } = string.Empty;
    }

    [DataContract]
    public class ShopifyOrderModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyCustomerId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public ShopifyCustomerModel Customer { get; set; } = new ShopifyCustomerModel();
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyCustomerId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long CheckoutId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long UserId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long AppId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long LocationId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OrderNumber { get; set; } = 0;
        [DataMember(IsRequired = true)] public long Number { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string SourceName { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string FinancialStatus { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string FulfillmentStatus { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal SubtotalPrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalTax { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalPrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalPriceUsd { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalDiscounts { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalLineItemsPrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalWeight { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public bool TaxesIncluded { get; set; } = false;
        [DataMember(IsRequired = true)] public bool Confirmed { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset? ProcessedAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset? ClosedAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset? CancelledAt { get; set; } = null;
        [DataMember(IsRequired = true)] public DateTimeOffset? DatePickedUp { get; set; } = null;
        [DataMember(IsRequired = true)] public List<ShopifyOrderLineItemModel> LineItems { get; set; } = new List<ShopifyOrderLineItemModel>();
    }

    [DataContract]
    public class ShopifyOrderLineItemModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid? ShopifyProductId { get; set; } = null;
        [DataMember(IsRequired = true)] public Guid? ShopifyProductVariantId { get; set; } = null;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyProductId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyProductVariantId { get; set; } = 0;
        [DataMember(IsRequired = true)] public int Quantity { get; set; } = 0;
        [DataMember(IsRequired = true)] public decimal Price { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal PreTaxPrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal TotalDiscount { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public bool IsTaxable { get; set; } = false;
        [DataMember(IsRequired = true)] public bool IsGiftCard { get; set; } = false;
        [DataMember(IsRequired = true)] public bool ProductExists { get; set; } = false;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
        [DataMember(IsRequired = true)] public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.MinValue;
    }

    [DataContract]
    public class ShopifyOrderTransactionModel : BaseModel
    {

        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid? ShopifyGiftCardId { get; set; } = null;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyGiftCardId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long LocationId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long UserId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long ParentId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long DeviceId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Status { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string ErrorCode { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal Amount { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;
    }

    [DataContract]
    public class ShopifyOrderShippingLineModel : BaseModel
    {
        [DataMember(IsRequired = true)] public Guid ShopifyOrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OriginalShopifyId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalShopifyOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Title { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Code { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Source { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal Price { get; set; } = 0.0m;
    }
}