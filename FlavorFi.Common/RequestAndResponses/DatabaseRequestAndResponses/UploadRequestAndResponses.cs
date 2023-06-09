using FlavorFi.Common.Enums;
using System;
using System.Runtime.Serialization;

namespace FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses
{
    [DataContract]
    public class GetLatestsUploadDateRequest : BaseRequest
    {
        public GetLatestsUploadDateRequest()
        {
            this.UploadType = UploadType.Unknown;
        }

        [DataMember(IsRequired = true)]
        public UploadType UploadType { get; set; }
    }

    [DataContract]
    public class GetLatestsUploadDateResponse : BaseResponse
    {
        public GetLatestsUploadDateResponse()
        {
            this.LatestUploadDate = null;
        }

        [DataMember(IsRequired = true)]
        public DateTimeOffset? LatestUploadDate { get; set; }
    }
}
