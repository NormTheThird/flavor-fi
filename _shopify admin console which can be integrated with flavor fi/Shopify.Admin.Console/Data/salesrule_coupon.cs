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
    
    public partial class salesrule_coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public salesrule_coupon()
        {
            this.salesrule_coupon_usage = new HashSet<salesrule_coupon_usage>();
        }
    
        public long coupon_id { get; set; }
        public long rule_id { get; set; }
        public string code { get; set; }
        public Nullable<long> usage_limit { get; set; }
        public Nullable<long> usage_per_customer { get; set; }
        public long times_used { get; set; }
        public Nullable<System.DateTime> expiration_date { get; set; }
        public Nullable<int> is_primary { get; set; }
        public System.DateTime created_at { get; set; }
        public Nullable<short> type { get; set; }
    
        public virtual salesrule salesrule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<salesrule_coupon_usage> salesrule_coupon_usage { get; set; }
    }
}
