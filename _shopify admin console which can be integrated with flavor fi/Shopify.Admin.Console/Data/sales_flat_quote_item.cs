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
    
    public partial class sales_flat_quote_item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sales_flat_quote_item()
        {
            this.sales_flat_quote_address_item = new HashSet<sales_flat_quote_address_item>();
            this.sales_flat_quote_item1 = new HashSet<sales_flat_quote_item>();
            this.sales_flat_quote_item_option = new HashSet<sales_flat_quote_item_option>();
        }
    
        public long item_id { get; set; }
        public long quote_id { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
        public Nullable<long> product_id { get; set; }
        public Nullable<int> store_id { get; set; }
        public Nullable<long> parent_item_id { get; set; }
        public Nullable<int> is_virtual { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string applied_rule_ids { get; set; }
        public string additional_data { get; set; }
        public int free_shipping { get; set; }
        public Nullable<int> is_qty_decimal { get; set; }
        public Nullable<int> no_discount { get; set; }
        public Nullable<decimal> weight { get; set; }
        public decimal qty { get; set; }
        public decimal price { get; set; }
        public decimal actual_cost { get; set; }
        public decimal base_price { get; set; }
        public Nullable<decimal> custom_price { get; set; }
        public Nullable<decimal> discount_percent { get; set; }
        public Nullable<decimal> discount_amount { get; set; }
        public Nullable<decimal> base_discount_amount { get; set; }
        public Nullable<decimal> tax_percent { get; set; }
        public Nullable<decimal> tax_amount { get; set; }
        public Nullable<decimal> base_tax_amount { get; set; }
        public decimal row_total { get; set; }
        public decimal base_row_total { get; set; }
        public Nullable<decimal> row_total_with_discount { get; set; }
        public Nullable<decimal> row_weight { get; set; }
        public string product_type { get; set; }
        public Nullable<decimal> base_tax_before_discount { get; set; }
        public Nullable<decimal> tax_before_discount { get; set; }
        public Nullable<decimal> original_custom_price { get; set; }
        public string redirect_url { get; set; }
        public Nullable<decimal> base_cost { get; set; }
        public Nullable<decimal> price_incl_tax { get; set; }
        public Nullable<decimal> base_price_incl_tax { get; set; }
        public Nullable<decimal> row_total_incl_tax { get; set; }
        public Nullable<decimal> base_row_total_incl_tax { get; set; }
        public Nullable<decimal> hidden_tax_amount { get; set; }
        public Nullable<decimal> base_hidden_tax_amount { get; set; }
        public Nullable<int> gift_message_id { get; set; }
        public Nullable<decimal> weee_tax_disposition { get; set; }
        public Nullable<decimal> weee_tax_row_disposition { get; set; }
        public Nullable<decimal> base_weee_tax_disposition { get; set; }
        public Nullable<decimal> base_weee_tax_row_disposition { get; set; }
        public string weee_tax_applied { get; set; }
        public Nullable<decimal> weee_tax_applied_amount { get; set; }
        public Nullable<decimal> weee_tax_applied_row_amount { get; set; }
        public Nullable<decimal> base_weee_tax_applied_amount { get; set; }
        public Nullable<decimal> base_weee_tax_applied_row_amnt { get; set; }
        public Nullable<int> event_id { get; set; }
        public Nullable<int> giftregistry_item_id { get; set; }
        public Nullable<int> gw_id { get; set; }
        public Nullable<decimal> gw_base_price { get; set; }
        public Nullable<decimal> gw_price { get; set; }
        public Nullable<decimal> gw_base_tax_amount { get; set; }
        public Nullable<decimal> gw_tax_amount { get; set; }
        public string pos_discount_reason { get; set; }
    
        public virtual catalog_product_entity catalog_product_entity { get; set; }
        public virtual core_store core_store { get; set; }
        public virtual sales_flat_quote sales_flat_quote { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_flat_quote_address_item> sales_flat_quote_address_item { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_flat_quote_item> sales_flat_quote_item1 { get; set; }
        public virtual sales_flat_quote_item sales_flat_quote_item2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sales_flat_quote_item_option> sales_flat_quote_item_option { get; set; }
    }
}
