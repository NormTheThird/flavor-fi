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
    
    public partial class enterprise_giftregistry_data
    {
        public long entity_id { get; set; }
        public Nullable<System.DateTime> event_date { get; set; }
        public string event_country { get; set; }
        public Nullable<int> event_country_region { get; set; }
        public string event_country_region_text { get; set; }
        public string event_location { get; set; }
    
        public virtual enterprise_giftregistry_entity enterprise_giftregistry_entity { get; set; }
    }
}
