
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
    
public partial class ShopifyWebhookLog
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ShopifyWebhookLog()
    {

        this.ShopifyWebhookActivityLogs = new HashSet<ShopifyWebhookActivityLog>();

    }


    public System.Guid Id { get; set; }

    public System.Guid CompanySiteId { get; set; }

    public System.Guid ShopifyWebhookId { get; set; }

    public long RecordId { get; set; }

    public bool Verified { get; set; }

    public System.DateTimeOffset DateCreated { get; set; }

    public System.DateTime ValidFrom { get; set; }

    public System.DateTime ValidTo { get; set; }



    public virtual CompanySite CompanySite { get; set; }

    public virtual ShopifyWebhook ShopifyWebhook { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ShopifyWebhookActivityLog> ShopifyWebhookActivityLogs { get; set; }

}

}
