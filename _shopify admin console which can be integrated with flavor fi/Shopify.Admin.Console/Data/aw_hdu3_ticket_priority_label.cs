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
    
    public partial class aw_hdu3_ticket_priority_label
    {
        public long id { get; set; }
        public long priority_id { get; set; }
        public string value { get; set; }
        public short store_id { get; set; }
    
        public virtual aw_hdu3_ticket_priority aw_hdu3_ticket_priority { get; set; }
    }
}
