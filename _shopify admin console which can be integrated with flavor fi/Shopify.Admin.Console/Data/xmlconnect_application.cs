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
    
    public partial class xmlconnect_application
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public xmlconnect_application()
        {
            this.xmlconnect_config_data = new HashSet<xmlconnect_config_data>();
            this.xmlconnect_history = new HashSet<xmlconnect_history>();
            this.xmlconnect_images = new HashSet<xmlconnect_images>();
            this.xmlconnect_notification_template = new HashSet<xmlconnect_notification_template>();
        }
    
        public int application_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public Nullable<int> store_id { get; set; }
        public Nullable<System.DateTime> active_from { get; set; }
        public Nullable<System.DateTime> active_to { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public int status { get; set; }
        public Nullable<int> browsing_mode { get; set; }
    
        public virtual core_store core_store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlconnect_config_data> xmlconnect_config_data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlconnect_history> xmlconnect_history { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlconnect_images> xmlconnect_images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<xmlconnect_notification_template> xmlconnect_notification_template { get; set; }
    }
}
