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
    
    public partial class bakerloo_payment_installments
    {
        public long id { get; set; }
        public long parent_id { get; set; }
        public long order_id { get; set; }
        public long order_increment_id { get; set; }
        public long pos_order_id { get; set; }
        public long payment_id { get; set; }
        public decimal amount_paid { get; set; }
        public decimal amount_refunded { get; set; }
        public string currency { get; set; }
        public string payment_method { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
    }
}
