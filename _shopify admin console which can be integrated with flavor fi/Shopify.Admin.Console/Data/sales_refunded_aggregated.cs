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
    
    public partial class sales_refunded_aggregated
    {
        public long id { get; set; }
        public Nullable<System.DateTime> period { get; set; }
        public Nullable<int> store_id { get; set; }
        public string order_status { get; set; }
        public int orders_count { get; set; }
        public Nullable<decimal> refunded { get; set; }
        public Nullable<decimal> online_refunded { get; set; }
        public Nullable<decimal> offline_refunded { get; set; }
    
        public virtual core_store core_store { get; set; }
    }
}
