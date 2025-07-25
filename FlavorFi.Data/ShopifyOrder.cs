
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FlavorFi.Data
{

using System;
    using System.Collections.Generic;
    
public partial class ShopifyOrder
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ShopifyOrder()
    {

        this.ShopifyGiftCards = new HashSet<ShopifyGiftCard>();

        this.ShopifyOrderLineItems = new HashSet<ShopifyOrderLineItem>();

        this.ShopifyOrderShippingLines = new HashSet<ShopifyOrderShippingLine>();

        this.ShopifyOrderTransactions = new HashSet<ShopifyOrderTransaction>();

    }


    public System.Guid Id { get; set; }

    public System.Guid CompanySiteId { get; set; }

    public Nullable<System.Guid> ShopifyCustomerId { get; set; }

    public long OriginalShopifyId { get; set; }

    public long OriginalShopifyCustomerId { get; set; }

    public long CheckoutId { get; set; }

    public long UserId { get; set; }

    public long AppId { get; set; }

    public long LocationId { get; set; }

    public long OrderNumber { get; set; }

    public long Number { get; set; }

    public string Name { get; set; }

    public string SourceName { get; set; }

    public string FinancialStatus { get; set; }

    public string FulfillmentStatus { get; set; }

    public decimal SubtotalPrice { get; set; }

    public decimal TotalTax { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal TotalPriceUsd { get; set; }

    public decimal TotalDiscounts { get; set; }

    public decimal TotalLineItemsPrice { get; set; }

    public bool TaxesIncluded { get; set; }

    public bool Confirmed { get; set; }

    public System.DateTimeOffset CreatedAt { get; set; }

    public System.DateTimeOffset UpdatedAt { get; set; }

    public Nullable<System.DateTimeOffset> ProcessedAt { get; set; }

    public Nullable<System.DateTimeOffset> ClosedAt { get; set; }

    public Nullable<System.DateTimeOffset> CancelledAt { get; set; }

    public System.DateTimeOffset DateCreated { get; set; }

    public System.DateTime ValidFrom { get; set; }

    public System.DateTime ValidTo { get; set; }

    public Nullable<System.DateTimeOffset> DatePickedUp { get; set; }



    public virtual CompanySite CompanySite { get; set; }

    public virtual ShopifyCustomer ShopifyCustomer { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ShopifyGiftCard> ShopifyGiftCards { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ShopifyOrderLineItem> ShopifyOrderLineItems { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ShopifyOrderShippingLine> ShopifyOrderShippingLines { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ShopifyOrderTransaction> ShopifyOrderTransactions { get; set; }

}

}
