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
    
    public partial class tag_relation
    {
        public long tag_relation_id { get; set; }
        public long tag_id { get; set; }
        public Nullable<long> customer_id { get; set; }
        public long product_id { get; set; }
        public int store_id { get; set; }
        public int active { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual catalog_product_entity catalog_product_entity { get; set; }
        public virtual core_store core_store { get; set; }
        public virtual customer_entity customer_entity { get; set; }
    }
}
