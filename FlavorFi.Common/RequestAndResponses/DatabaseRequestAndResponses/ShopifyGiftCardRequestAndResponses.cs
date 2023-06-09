using FlavorFi.Common.Models.DatabaseModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class SaveShopifyGiftCardRequest : BaseRequest
    {
        public SaveShopifyGiftCardRequest()
        {
            this.GiftCard = new ShopifyGiftCardModel();
        }

        [DataMember(IsRequired = true)]
        public ShopifyGiftCardModel GiftCard { get; set; }
    }

    [DataContract]
    public class SaveShopifyGiftCardReportRequest : BaseRequest
    {
        public SaveShopifyGiftCardReportRequest()
        {
            this.GiftCard = new ShopifyGiftCardReportExportModel();
        }

        [DataMember(IsRequired = true)]
        public ShopifyGiftCardReportExportModel GiftCard { get; set; }
    }

    [DataContract]
    public class SaveShopifyGiftCardResponse : BaseResponse { }

    [DataContract]
    public class GetShopifyGiftCardTotalsRequest : BaseRequest
    {
        public GetShopifyGiftCardTotalsRequest()
        {
            this.Year = 0;
        }

        [DataMember(IsRequired = true)]
        public int Year { get; set; }
    }

    [DataContract]
    public class GetShopifyGiftCardTotalsResponse : BaseResponse
    {
        public GetShopifyGiftCardTotalsResponse()
        {
            this.ShopifyGiftCardTotals = new ShopifyGiftCardTotalsModel();
        }

        [DataMember(IsRequired = true)]
        public ShopifyGiftCardTotalsModel ShopifyGiftCardTotals { get; set; }
    }

    [DataContract]
    public class GetShopifyGiftCardMonthlyTotalsRequest : BaseRequest
    {
        public GetShopifyGiftCardMonthlyTotalsRequest()
        {
            this.Year = 0;
        }

        [DataMember(IsRequired = true)]
        public int Year { get; set; }
    }

    [DataContract]
    public class GetShopifyGiftCardMonthlyTotalsResponse : BaseResponse
    {
        public GetShopifyGiftCardMonthlyTotalsResponse()
        {
            this.ShopifyGiftCardMonthlyTotals = new List<ShopifyGiftCardMonthlyTotalsModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ShopifyGiftCardMonthlyTotalsModel> ShopifyGiftCardMonthlyTotals { get; set; }
    }
}