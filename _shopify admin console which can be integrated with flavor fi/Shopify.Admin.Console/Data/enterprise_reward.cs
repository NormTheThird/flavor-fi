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
    
    public partial class enterprise_reward
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public enterprise_reward()
        {
            this.enterprise_reward_history = new HashSet<enterprise_reward_history>();
        }
    
        public long reward_id { get; set; }
        public long customer_id { get; set; }
        public Nullable<int> website_id { get; set; }
        public long points_balance { get; set; }
        public string website_currency_code { get; set; }
    
        public virtual customer_entity customer_entity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<enterprise_reward_history> enterprise_reward_history { get; set; }
    }
}
