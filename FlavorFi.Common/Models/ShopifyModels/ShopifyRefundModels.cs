using System;
using System.Collections.Generic;

namespace FlavorFi.Common.Models.ShopifyModels
{
    public class ShopifyRefundModel : ShopifyBaseModel
    {
        public ShopifyRefundModel()
        {
            this.UserId = 0;
            this.Note = string.Empty;
            this.LineItems = new List<ShopifyRefundLineItemModel>();
            this.Restock = false;
            this.Transactions = new List<ShopifyTransactionModel>();
            this.ProcessedAt = null;
        }

        public long UserId { get; set; }
        public string Note { get; set; }
        public List<ShopifyRefundLineItemModel> LineItems { get; set; }
        public bool Restock { get; set; }
        public List<ShopifyTransactionModel> Transactions { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
    }

    public class ShopifyRefundLineItemModel : ShopifyBaseModel
    {
        public ShopifyRefundLineItemModel()
        {
            this.Quantity = 0;
            this.LineItemId = 0;
            this.LineItem = new ShopifyOrderLineItemModel();
        }

        public int Quantity { get; set; }
        public long LineItemId { get; set; }
        public ShopifyOrderLineItemModel LineItem { get; set; }
    }
}