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
    
    public partial class core_email_queue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public core_email_queue()
        {
            this.core_email_queue_recipients = new HashSet<core_email_queue_recipients>();
        }
    
        public long message_id { get; set; }
        public Nullable<long> entity_id { get; set; }
        public string entity_type { get; set; }
        public string event_type { get; set; }
        public string message_body_hash { get; set; }
        public string message_body { get; set; }
        public string message_parameters { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> processed_at { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<core_email_queue_recipients> core_email_queue_recipients { get; set; }
    }
}
