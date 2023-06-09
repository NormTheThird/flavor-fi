using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.Models.DatabaseModels
{
    [DataContract]
    public class CostOfGoodReportModel
    {
        [DataMember(IsRequired = true)] public Guid OrderId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid OrderDetailId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public Guid ProductId { get; set; } = Guid.Empty;
        [DataMember(IsRequired = true)] public long OriginalOrderId { get; set; } = 0;
        [DataMember(IsRequired = true)] public long OriginalProductId { get; set; } = 0;
        [DataMember(IsRequired = true)] public string Name { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Sku { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Vendor { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Variant { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string CostOfGood { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public int Quantity { get; set; } = 0;
        [DataMember(IsRequired = true)] public decimal SalePrice { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public DateTimeOffset DateSold { get; set; } = DateTimeOffset.MinValue;
    }

    [DataContract]
    public class GiftCardReportModel : BaseModel
    {
        [DataMember(IsRequired = true)] public string LastFour { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string IssuedBy { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Customer { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public string Note { get; set; } = string.Empty;
        [DataMember(IsRequired = true)] public decimal InitialValue { get; set; } = 0.0m;
        [DataMember(IsRequired = true)] public decimal Balance { get; set; } = 0.0m;
    }
}