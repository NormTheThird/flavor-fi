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
    
    public partial class review_entity_summary
    {
        public long primary_id { get; set; }
        public long entity_pk_value { get; set; }
        public short entity_type { get; set; }
        public short reviews_count { get; set; }
        public short rating_summary { get; set; }
        public int store_id { get; set; }
    
        public virtual core_store core_store { get; set; }
    }
}
