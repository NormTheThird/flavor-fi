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
    
    public partial class rating_title
    {
        public int rating_id { get; set; }
        public int store_id { get; set; }
        public string value { get; set; }
    
        public virtual core_store core_store { get; set; }
        public virtual rating rating { get; set; }
    }
}
