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
    
    public partial class m_email_trigger_chain
    {
        public int chain_id { get; set; }
        public int trigger_id { get; set; }
        public string delay { get; set; }
        public string template_id { get; set; }
        public Nullable<int> run_rule_id { get; set; }
        public Nullable<int> stop_rule_id { get; set; }
        public int coupon_enabled { get; set; }
        public Nullable<int> coupon_sales_rule_id { get; set; }
        public Nullable<int> coupon_expires_days { get; set; }
        public int cross_sells_enabled { get; set; }
        public string cross_sells_type_id { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
    }
}
