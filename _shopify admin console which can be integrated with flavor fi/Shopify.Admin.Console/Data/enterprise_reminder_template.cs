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
    
    public partial class enterprise_reminder_template
    {
        public long rule_id { get; set; }
        public short store_id { get; set; }
        public Nullable<long> template_id { get; set; }
        public string label { get; set; }
        public string description { get; set; }
    
        public virtual core_email_template core_email_template { get; set; }
        public virtual enterprise_reminder_rule enterprise_reminder_rule { get; set; }
    }
}
