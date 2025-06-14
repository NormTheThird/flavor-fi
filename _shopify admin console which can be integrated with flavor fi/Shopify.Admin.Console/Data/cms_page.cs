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
    
    public partial class cms_page
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cms_page()
        {
            this.enterprise_cms_hierarchy_node = new HashSet<enterprise_cms_hierarchy_node>();
            this.enterprise_cms_page_revision = new HashSet<enterprise_cms_page_revision>();
            this.enterprise_cms_page_version = new HashSet<enterprise_cms_page_version>();
            this.seosuite_report_cms = new HashSet<seosuite_report_cms>();
            this.core_store = new HashSet<core_store>();
        }
    
        public short page_id { get; set; }
        public string title { get; set; }
        public string root_template { get; set; }
        public string meta_keywords { get; set; }
        public string meta_description { get; set; }
        public string identifier { get; set; }
        public string content_heading { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> creation_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public short is_active { get; set; }
        public short sort_order { get; set; }
        public string layout_update_xml { get; set; }
        public string custom_theme { get; set; }
        public string custom_root_template { get; set; }
        public string custom_layout_update_xml { get; set; }
        public Nullable<System.DateTime> custom_theme_from { get; set; }
        public Nullable<System.DateTime> custom_theme_to { get; set; }
        public long published_revision_id { get; set; }
        public int website_root { get; set; }
        public int under_version_control { get; set; }
        public string meta_title { get; set; }
        public short exclude_from_sitemap { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<enterprise_cms_hierarchy_node> enterprise_cms_hierarchy_node { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<enterprise_cms_page_revision> enterprise_cms_page_revision { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<enterprise_cms_page_version> enterprise_cms_page_version { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<seosuite_report_cms> seosuite_report_cms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<core_store> core_store { get; set; }
    }
}
