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
    
    public partial class enterprise_rma_status_history
    {
        public long entity_id { get; set; }
        public long rma_entity_id { get; set; }
        public Nullable<int> is_customer_notified { get; set; }
        public int is_visible_on_front { get; set; }
        public string comment { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<short> is_admin { get; set; }
    
        public virtual enterprise_rma enterprise_rma { get; set; }
    }
}
