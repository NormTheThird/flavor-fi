using FlavorFi.Common.Models.DataSyncModels;
using System;
using System.Collections.Generic;

namespace FlavorFi.Common.RequestAndResponses.DataSyncRequestAndResponses
{
    public class GetDatabaseSyncStatusRequest : BaseRequest { }

    public class GetDatabaseSyncStatusResponse : BaseResponse
    {
        public GetDatabaseSyncStatusResponse()
        {
      
        }

    }


    public class SynchronizeDatabaseProductsRequest : BaseRequest  { }

    public class SynchronizeDatabaseProductsResponse : BaseResponse
    {
        public SynchronizeDatabaseProductsResponse()
        {
            this.Message = string.Empty;
        }

        public string Message { get; set; }
    }

    public class SynchronizeDatabaseProductsStatusRequest : BaseRequest { }

    public class SynchronizeDatabaseProductsStatusResponse : BaseResponse
    {
        public SynchronizeDatabaseProductsStatusResponse()
        {
            Statuses = new List<SynchronizeDatabaseStatusModel>();
        }

        public List<SynchronizeDatabaseStatusModel> Statuses { get; set; }
    }


    public class SynchronizeDatabaseOrdersRequest : BaseRequest { }

    public class SynchronizeDatabaseOrdersResponse : BaseResponse
    {
        public SynchronizeDatabaseOrdersResponse()
        {
            this.Message = string.Empty;
        }

        public string Message { get; set; }
    }

    public class SynchronizeDatabaseOrdersStatusRequest : BaseRequest { }

    public class SynchronizeDatabaseOrdersStatusResponse : BaseResponse
    {
        public SynchronizeDatabaseOrdersStatusResponse()
        {
            Statuses = new List<SynchronizeDatabaseStatusModel>();
        }

        public List<SynchronizeDatabaseStatusModel> Statuses { get; set; }
    }


    public class SynchronizeDatabaseCustomersRequest : BaseRequest { }

    public class SynchronizeDatabaseCustomersResponse : BaseResponse
    {
        public SynchronizeDatabaseCustomersResponse()
        {
            this.Message = string.Empty;
        }

        public string Message { get; set; }
    }

    public class SynchronizeDatabaseCustomersStatusRequest : BaseRequest { }

    public class SynchronizeDatabaseCustomersStatusResponse : BaseResponse
    {
        public SynchronizeDatabaseCustomersStatusResponse()
        {
            Statuses = new List<SynchronizeDatabaseStatusModel>();
        }

        public List<SynchronizeDatabaseStatusModel> Statuses { get; set; }
    }


    public class SynchronizeDatabaseGiftCardsRequest : BaseRequest { }

    public class SynchronizeDatabaseGiftCardsResponse : BaseResponse
    {
        public SynchronizeDatabaseGiftCardsResponse()
        {
            this.Message = string.Empty;
        }

        public string Message { get; set; }
    }

    public class SynchronizeDatabaseGiftCardsStatusRequest : BaseRequest { }

    public class SynchronizeDatabaseGiftCardsStatusResponse : BaseResponse
    {
        public SynchronizeDatabaseGiftCardsStatusResponse()
        {
            Statuses = new List<SynchronizeDatabaseStatusModel>();
        }

        public List<SynchronizeDatabaseStatusModel> Statuses { get; set; }
    }
}