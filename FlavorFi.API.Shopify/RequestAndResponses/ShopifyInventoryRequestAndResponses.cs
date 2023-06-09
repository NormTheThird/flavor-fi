using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlavorFi.API.Shopify.RequestAndResponses
{
    [DataContract]
    public class GetInventoryLocationsRequest : BaseAppllicationRequest { }

    [DataContract]
    public class GetInventoryLocationsResponse : BaseResponse
    {
        public GetInventoryLocationsResponse()
        {
            this.Locations = new List<InventoryLocationModel>();
        }

        [DataMember(IsRequired = true)]
        public List<InventoryLocationModel> Locations { get; set; }
    }

    [DataContract]
    public class SaveInventoryLocationRequest : BaseAppllicationRequest
    {
        public SaveInventoryLocationRequest()
        {
            this.LocationId = 0;
            this.IsEnabled = false;
        }

        [DataMember(IsRequired = true)]
        public long LocationId { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsEnabled { get; set; }
    }

    [DataContract]
    public class SaveInventoryLocationResponse : BaseResponse { }

    [DataContract]
    public class InventoryLocationModel
    {
        public InventoryLocationModel()
        {
            this.LocationId = 0;
            this.Name = string.Empty;
            this.IsEnabled = false;
        }

        [DataMember(IsRequired = true)]
        public long LocationId { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsEnabled { get; set; }
    }
}