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
    
    public partial class core_variable_value
    {
        public long value_id { get; set; }
        public long variable_id { get; set; }
        public int store_id { get; set; }
        public string plain_value { get; set; }
        public string html_value { get; set; }
    
        public virtual core_store core_store { get; set; }
        public virtual core_variable core_variable { get; set; }
    }
}
