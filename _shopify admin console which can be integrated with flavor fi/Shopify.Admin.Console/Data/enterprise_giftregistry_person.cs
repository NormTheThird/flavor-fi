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
    
    public partial class enterprise_giftregistry_person
    {
        public long person_id { get; set; }
        public long entity_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string custom_values { get; set; }
    
        public virtual enterprise_giftregistry_entity enterprise_giftregistry_entity { get; set; }
    }
}
