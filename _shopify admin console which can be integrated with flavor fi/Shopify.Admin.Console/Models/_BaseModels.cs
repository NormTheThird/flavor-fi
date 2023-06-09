using System;
using System.Runtime.Serialization;

namespace Shopify.Admin.Console.Models
{
    [DataContract]
    public class BaseModel
    {
        public BaseModel()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        [DataMember(IsRequired = true)]
        public DateTime CreatedAt { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime UpdatedAt { get; set; }
    }
}