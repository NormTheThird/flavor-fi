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
    
    public partial class giftvoucher_template
    {
        public long template_id { get; set; }
        public string type { get; set; }
        public string template_name { get; set; }
        public string pattern { get; set; }
        public Nullable<decimal> balance { get; set; }
        public string currency { get; set; }
        public Nullable<System.DateTime> expired_at { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<System.DateTime> day_to_send { get; set; }
        public int store_id { get; set; }
        public string conditions_serialized { get; set; }
        public int is_generated { get; set; }
        public int giftcard_template_id { get; set; }
        public string giftcard_template_image { get; set; }
    }
}
