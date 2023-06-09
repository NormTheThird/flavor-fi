using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyOrderModel : ShopifyBaseModel
    {
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset? ClosedAt { get; set; } = null;
        public long Number { get; set; } = 0;
        public string Note { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string Test { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; } = 0.0m;
        public decimal SubtotalPrice { get; set; } = 0.0m;
        public decimal TotalWeight { get; set; } = 0.0m;
        public decimal TotalTax { get; set; } = 0.0m;
        public bool TaxesIncluded { get; set; } = false;
        public string Currency { get; set; } = string.Empty;
        public string FinancialStatus { get; set; } = string.Empty;
        public bool Confirmed { get; set; } = false;
        public decimal TotalDiscounts { get; set; } = 0.0m;
        public decimal TotalLineItemsPrice { get; set; } = 0.0m;
        public string CartToken { get; set; } = string.Empty;
        public bool BuyerAcceptsMarketing { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string ReferringSite { get; set; } = string.Empty;
        public string LandingSite { get; set; } = string.Empty;
        public DateTimeOffset? CancelledAt { get; set; } = null;
        public string CancelReason { get; set; } = string.Empty;
        public decimal TotalPriceUsd { get; set; } = 0.0m;
        public string CheckoutToken { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public long UserId { get; set; } = 0;
        public long LocationId { get; set; } = 0;
        public string SourceIdentifier { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
        public DateTimeOffset? ProcessedAt { get; set; } = null;
        public string DeviceId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CustomerLocale { get; set; } = string.Empty;
        public long AppId { get; set; } = 0;
        public string BrowserIP { get; set; } = string.Empty;
        public string LandingSiteRef { get; set; } = string.Empty;
        public long OrderNumber { get; set; } = 0;
        public List<ShopifyOrderDiscountModel> DiscountCodes { get; set; } = new List<ShopifyOrderDiscountModel>();
        public Dictionary<string, string> NoteAttributes { get; set; } = new Dictionary<string, string>();
        public List<string> PaymentGatewayNames { get; set; } = new List<string>();
        public string ProcessingMethod { get; set; } = string.Empty;
        public long CheckoutId { get; set; } = 0;
        public string SourceName { get; set; } = string.Empty;
        public string FulfillmentStatus { get; set; } = string.Empty;
        public List<ShopifyTaxLineModel> TaxLines { get; set; } = new List<ShopifyTaxLineModel>();
        public string Tags { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string OrderStatusUrl { get; set; } = string.Empty;
        public List<ShopifyOrderLineItemModel> LineItems { get; set; } = new List<ShopifyOrderLineItemModel>();
        public List<ShopifyOrderShippingLineModel> ShippingLines { get; set; } = new List<ShopifyOrderShippingLineModel>();
        public ShopifyAddressModel BillingAddress { get; set; } = new ShopifyAddressModel();
        public ShopifyAddressModel ShippingAddress { get; set; } = new ShopifyAddressModel();
        public List<ShopifyOrderFulfillmentModel> Fulfillments { get; set; } = new List<ShopifyOrderFulfillmentModel>();
        public ShopifyClientDetailModel ClientDetails { get; set; } = new ShopifyClientDetailModel();
        public List<ShopifyRefundModel> Refunds { get; set; } = new List<ShopifyRefundModel>();
        public ShopifyPaymentDetailModel PaymentDetails { get; set; } = new ShopifyPaymentDetailModel();
        public ShopifyCustomerModel Customer { get; set; } = new ShopifyCustomerModel();
    }

    public class ShopifyOrderLineItemModel : ShopifyBaseModel
    {
        public long VariantId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m;
        public int Grams { get; set; } = 0;
        public string Sku { get; set; } = string.Empty;
        public string VariantTitle { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public string FulfillmentService { get; set; } = string.Empty;
        public long ProductId { get; set; } = 0;
        public bool RequiresShipping { get; set; } = false;
        public bool IsTaxable { get; set; } = false;
        public bool IsGiftCard { get; set; } = false;
        public decimal PreTaxPrice { get; set; } = 0.0m;
        public string Name { get; set; } = string.Empty;
        public string VariantInventoryManagement { get; set; } = string.Empty;
        public List<Dictionary<string, string>> Properties { get; set; } = new List<Dictionary<string, string>>();
        public bool ProductExists { get; set; } = false;
        public int FulfillmentQuantity { get; set; } = 0;
        public decimal TotalDiscount { get; set; } = 0.0m;
        public string FulfillmentStatus { get; set; } = string.Empty;
        public List<ShopifyTaxLineModel> TaxLines { get; set; } = new List<ShopifyTaxLineModel>();
        public ShopifyAddressModel OriginLocation { get; set; } = new ShopifyAddressModel();
        public ShopifyAddressModel DestinationLocation { get; set; } = new ShopifyAddressModel();
    }

    public class ShopifyOrderTransactionModel : ShopifyBaseModel
    {
        public long OrderId { get; set; } = 0;
        public long GiftCardId { get; set; } = 0;
        public long LocationId { get; set; } = 0;
        public long UserId { get; set; } = 0;
        public long ParentId { get; set; } = 0;
        public long DeviceId { get; set; } = 0;
        public string Kind { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Authorization { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.0m;
        public bool Test { get; set; } = false;
    }

    public class ShopifyOrderDiscountModel
    {
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.0m;
    }

    public class ShopifyOrderFulfillmentModel : ShopifyBaseModel
    {
        public long OrderId { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public string TrackingCompany { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
    }

    public class ShopifyTaxLineModel
    {
        public decimal Price { get; set; } = 0.0m;
        public decimal Rate { get; set; } = 0.0m;
        public string Title { get; set; } = string.Empty;
    }

    [JsonObject(MemberSerialization.OptIn, Title = "shipping_lines")]
    public class ShopifyOrderShippingLineModel
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; } = 0;
        [JsonProperty(PropertyName = "title")] public string Title { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "code")] public string Code { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "source")] public string Source { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "price")] public string Price { get; set; } = string.Empty;
    }

    public class ShopifyPaymentDetailModel
    {
        public string AVSResultCode { get; set; } = string.Empty;
        public string CreditCardBin { get; set; } = string.Empty;
        public string CVVResultCode { get; set; } = string.Empty;
        public string CreditCardNumber { get; set; } = string.Empty;
        public string CreditCardCompany { get; set; } = string.Empty;
    }

    public class ShopifyClientDetailModel
    {
        public string BrowserIp { get; set; } = string.Empty;
        public string AcceptLanguage { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string SessionHash { get; set; } = string.Empty;
        public int BrowserWidth { get; set; } = 0;
        public int BrowserHeight { get; set; } = 0;
    }

    public class ShopifyQualityCheckOrderItemModel
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int NumberOf { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m;
    }
}