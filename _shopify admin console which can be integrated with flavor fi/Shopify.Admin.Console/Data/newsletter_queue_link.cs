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
    
    public partial class newsletter_queue_link
    {
        public long queue_link_id { get; set; }
        public long queue_id { get; set; }
        public long subscriber_id { get; set; }
        public Nullable<System.DateTime> letter_sent_at { get; set; }
    
        public virtual newsletter_queue newsletter_queue { get; set; }
        public virtual newsletter_subscriber newsletter_subscriber { get; set; }
    }
}
