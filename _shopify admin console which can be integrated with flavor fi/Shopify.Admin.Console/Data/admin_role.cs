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
    
    public partial class admin_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public admin_role()
        {
            this.admin_rule = new HashSet<admin_rule>();
        }
    
        public long role_id { get; set; }
        public long parent_id { get; set; }
        public int tree_level { get; set; }
        public int sort_order { get; set; }
        public string role_type { get; set; }
        public long user_id { get; set; }
        public string role_name { get; set; }
        public int gws_is_all { get; set; }
        public string gws_websites { get; set; }
        public string gws_store_groups { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<admin_rule> admin_rule { get; set; }
    }
}
