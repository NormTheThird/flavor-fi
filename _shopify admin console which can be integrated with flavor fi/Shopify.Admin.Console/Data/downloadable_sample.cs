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
    
    public partial class downloadable_sample
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public downloadable_sample()
        {
            this.downloadable_sample_title = new HashSet<downloadable_sample_title>();
        }
    
        public long sample_id { get; set; }
        public long product_id { get; set; }
        public string sample_url { get; set; }
        public string sample_file { get; set; }
        public string sample_type { get; set; }
        public long sort_order { get; set; }
    
        public virtual catalog_product_entity catalog_product_entity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<downloadable_sample_title> downloadable_sample_title { get; set; }
    }
}
