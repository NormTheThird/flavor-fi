using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyWebhookService
    {
        GetShopifyWebhooksResponse GetShopifyWebhooks(GetShopifyWebhooksRequest request);
        GetShopifyWebhookActivityLogsResponse GetShopifyWebhookActivityLogs(GetShopifyWebhookActivityLogsRequest request);
    }

    public class ShopifyWebhookService: BaseService, IShopifyWebhookService
    {
        public GetShopifyWebhooksResponse GetShopifyWebhooks(GetShopifyWebhooksRequest request)
        {
            try
            {
                var response = new GetShopifyWebhooksResponse();
                using (var context = new FlavorFiEntities())
                {
                    var webhooks = context.ShopifyWebhooks.AsNoTracking()
                                          .Select(w => new ShopifyWebhookModel
                                          {
                                              Id = w.Id,
                                              Event = w.Event,
                                              Topic = w.Topic,
                                              Format = w.Format,
                                              DateCreated = w.DateCreated
                                          });

                    response.ShopifyWebhooks = webhooks.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyWebhooksResponse { ErrorMessage = "unable to get shopify webhooks" };
            }
        }

        public GetShopifyWebhookActivityLogsResponse GetShopifyWebhookActivityLogs(GetShopifyWebhookActivityLogsRequest request)
        {
            try
            {
                var response = new GetShopifyWebhookActivityLogsResponse();
                using (var context = new FlavorFiEntities())
                {
                    var webhooks = context.ShopifyWebhookActivityLogs.AsNoTracking()
                                          .OrderByDescending(wal => wal.DateCreated)
                                          .Take(5000)
                                          .Select(wal => new ShopifyWebhookActivityLogModel
                                          {
                                              Id = wal.Id,
                                              Topic = wal.ShopifyWebhookLog.ShopifyWebhook.Topic,
                                              Activity = wal.Acivity,
                                              RecordId = wal.ShopifyWebhookLog.RecordId,
                                              DateCreated = wal.DateCreated
                                          });

                    response.ShopifyWebhookActivityLogs = webhooks.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopifyWebhookActivityLogsResponse { ErrorMessage = "unable to get shopify webhook activity logs" };
            }
        }
    }
}