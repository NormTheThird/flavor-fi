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
    
    public partial class aw_hdu3_ticket_history_additional
    {
        public long id { get; set; }
        public long ticket_history_id { get; set; }
        public Nullable<long> department_agent_id { get; set; }
        public long department_id { get; set; }
        public int status { get; set; }
        public short priority { get; set; }
    
        public virtual aw_hdu3_ticket_history aw_hdu3_ticket_history { get; set; }
    }
}
