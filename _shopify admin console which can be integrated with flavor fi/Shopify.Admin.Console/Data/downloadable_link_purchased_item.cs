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
    
    public partial class downloadable_link_purchased_item
    {
        public long item_id { get; set; }
        public long purchased_id { get; set; }
        public Nullable<long> order_item_id { get; set; }
        public Nullable<long> product_id { get; set; }
        public string link_hash { get; set; }
        public long number_of_downloads_bought { get; set; }
        public long number_of_downloads_used { get; set; }
        public long link_id { get; set; }
        public string link_title { get; set; }
        public int is_shareable { get; set; }
        public string link_url { get; set; }
        public string link_file { get; set; }
        public string link_type { get; set; }
        public string status { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
    
        public virtual downloadable_link_purchased downloadable_link_purchased { get; set; }
        public virtual sales_flat_order_item sales_flat_order_item { get; set; }
    }
}
