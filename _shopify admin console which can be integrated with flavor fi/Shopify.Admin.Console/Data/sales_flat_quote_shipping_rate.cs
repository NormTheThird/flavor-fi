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
    
    public partial class sales_flat_quote_shipping_rate
    {
        public long rate_id { get; set; }
        public long address_id { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
        public string carrier { get; set; }
        public string carrier_title { get; set; }
        public string code { get; set; }
        public string method { get; set; }
        public string method_description { get; set; }
        public decimal price { get; set; }
        public string error_message { get; set; }
        public string method_title { get; set; }
    
        public virtual sales_flat_quote_address sales_flat_quote_address { get; set; }
    }
}
