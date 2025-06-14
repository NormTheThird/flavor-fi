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
    
    public partial class sales_flat_order_address
    {
        public long entity_id { get; set; }
        public Nullable<long> parent_id { get; set; }
        public Nullable<int> customer_address_id { get; set; }
        public Nullable<int> quote_address_id { get; set; }
        public Nullable<int> region_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public string fax { get; set; }
        public string region { get; set; }
        public string postcode { get; set; }
        public string lastname { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string country_id { get; set; }
        public string firstname { get; set; }
        public string address_type { get; set; }
        public string prefix { get; set; }
        public string middlename { get; set; }
        public string suffix { get; set; }
        public string company { get; set; }
        public string vat_id { get; set; }
        public Nullable<short> vat_is_valid { get; set; }
        public string vat_request_id { get; set; }
        public string vat_request_date { get; set; }
        public Nullable<short> vat_request_success { get; set; }
        public Nullable<int> giftregistry_item_id { get; set; }
    
        public virtual enterprise_customer_sales_flat_order_address enterprise_customer_sales_flat_order_address { get; set; }
        public virtual sales_flat_order sales_flat_order { get; set; }
    }
}
