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
    
    public partial class report_event
    {
        public long event_id { get; set; }
        public System.DateTime logged_at { get; set; }
        public int event_type_id { get; set; }
        public long object_id { get; set; }
        public long subject_id { get; set; }
        public int subtype { get; set; }
        public int store_id { get; set; }
    
        public virtual core_store core_store { get; set; }
        public virtual report_event_types report_event_types { get; set; }
    }
}
