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
    
    public partial class affiliateplus_referer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public affiliateplus_referer()
        {
            this.affiliateplusstatistics = new HashSet<affiliateplusstatistic>();
        }
    
        public long referer_id { get; set; }
        public long account_id { get; set; }
        public string referer { get; set; }
        public string url_path { get; set; }
        public int total_clicks { get; set; }
        public int unique_clicks { get; set; }
        public string ip_list { get; set; }
        public int store_id { get; set; }
    
        public virtual affiliateplus_account affiliateplus_account { get; set; }
        public virtual core_store core_store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<affiliateplusstatistic> affiliateplusstatistics { get; set; }
    }
}
