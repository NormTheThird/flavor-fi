//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopify.Admin.Console.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class sales_bestsellers_aggregated_daily
    {
        public long id { get; set; }
        public Nullable<System.DateTime> period { get; set; }
        public Nullable<int> store_id { get; set; }
        public Nullable<long> product_id { get; set; }
        public string product_name { get; set; }
        public decimal product_price { get; set; }
        public decimal qty_ordered { get; set; }
        public int rating_pos { get; set; }
    
        public virtual catalog_product_entity catalog_product_entity { get; set; }
        public virtual core_store core_store { get; set; }
    }
}
