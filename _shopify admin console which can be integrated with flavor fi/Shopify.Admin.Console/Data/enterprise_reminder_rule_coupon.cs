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
    
    public partial class enterprise_reminder_rule_coupon
    {
        public long rule_id { get; set; }
        public Nullable<long> coupon_id { get; set; }
        public long customer_id { get; set; }
        public System.DateTime associated_at { get; set; }
        public int emails_failed { get; set; }
        public int is_active { get; set; }
    
        public virtual enterprise_reminder_rule enterprise_reminder_rule { get; set; }
    }
}
