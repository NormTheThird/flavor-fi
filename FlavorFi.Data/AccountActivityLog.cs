
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
    
public partial class AccountActivityLog
{

    public System.Guid Id { get; set; }

    public System.Guid AccountId { get; set; }

    public string ActivityType { get; set; }

    public string Note { get; set; }

    public System.DateTimeOffset DateCreated { get; set; }



    public virtual Account Account { get; set; }

}

}
