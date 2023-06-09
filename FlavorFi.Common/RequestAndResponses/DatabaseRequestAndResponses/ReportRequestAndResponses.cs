using FlavorFi.Common.Models.DatabaseModels;
using System;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{ 
    public class GetReportRequest : BaseRequest
    {
        public GetReportRequest()
        {
            this.StartDate = DateTime.Today;
            this.EndDate = DateTime.Today;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class GetCostOfGoodsReportResponse : BaseResponse
    {
        public GetCostOfGoodsReportResponse()
        {
            this.CostOfGoods = new List<CostOfGoodReportModel>();
            this.Vendors = new List<string>();
            this.Discounts = 0.0m;
            this.Refunds = 0.0m;
        }

        public List<CostOfGoodReportModel> CostOfGoods { get; set; }
        public List<string> Vendors { get; set; }
        public decimal Discounts { get; set; }
        public decimal Refunds { get; set; }
    }

    public class GetGiftCardReportResponse : BaseResponse
    {
        public GetGiftCardReportResponse()
        {
            this.GiftCards = new List<GiftCardReportModel>();
            this.TotalAmountUsed = 0.0m;
            this.TotalLiability = 0.0m;
        }

        public List<GiftCardReportModel> GiftCards { get; set; }
        public decimal TotalAmountUsed { get; set; }
        public decimal TotalLiability { get; set; }

    }

    public class GetSalesReportResponse : BaseResponse
    {
        public GetSalesReportResponse()
        {
            this.GrossSales = 0.0m;
            this.Discounts = 0.0m;
            this.Returns = 0.0m;
            this.SalesTax = 0.0m;

        }

        public decimal GrossSales { get; set; }
        public decimal Discounts { get; set; }
        public decimal Returns { get; set; }
        public decimal SalesTax { get; set; }
    }


    public class GetExportReportResponse : BaseResponse
    {
        public GetExportReportResponse()
        {
            this.Report = null;
        }

        public Byte[] Report { get; set; }
    }
}