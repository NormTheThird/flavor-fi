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
    
    public partial class catalogrule_product_price
    {
        public long rule_product_price_id { get; set; }
        public System.DateTime rule_date { get; set; }
        public int customer_group_id { get; set; }
        public long product_id { get; set; }
        public decimal rule_price { get; set; }
        public int website_id { get; set; }
        public Nullable<System.DateTime> latest_start_date { get; set; }
        public Nullable<System.DateTime> earliest_end_date { get; set; }
        public System.Guid ssma_rowid { get; set; }
    
        public virtual catalog_product_entity catalog_product_entity { get; set; }
        public virtual core_website core_website { get; set; }
        public virtual customer_group customer_group { get; set; }
    }
}
