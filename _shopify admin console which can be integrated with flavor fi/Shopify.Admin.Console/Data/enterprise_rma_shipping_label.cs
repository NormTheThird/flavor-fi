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
    
    public partial class enterprise_rma_shipping_label
    {
        public long entity_id { get; set; }
        public long rma_entity_id { get; set; }
        public byte[] shipping_label { get; set; }
        public string packages { get; set; }
        public string track_number { get; set; }
        public string carrier_title { get; set; }
        public string method_title { get; set; }
        public string carrier_code { get; set; }
        public string method_code { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<short> is_admin { get; set; }
    
        public virtual enterprise_rma enterprise_rma { get; set; }
    }
}
