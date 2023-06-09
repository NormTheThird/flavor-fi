using FlavorFi.Common.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.IO;
using System.Collections.Generic;
using DatabaseModels = FlavorFi.Common.Models.DatabaseModels;
using ShopifyModels = FlavorFi.Common.Models.ShopifyModels;
using DatabaseServices = FlavorFi.Services.DatabaseServices;
using ShopifyServices = FlavorFi.Services.ShopifyServices;
using DatabaseRequestAndResponses = FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using ShopifyRequestAndResponses = FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;

namespace FlavorFi.WebJob.DataSync
{
    public class Functions
    {
        //public void SynchronizeDatabaseCustomers([ServiceBusTrigger("SynchronizeDatabaseCustomers-Request")] DatabaseRequestAndResponses.SynchronizeDatabaseCustomersRequest request,
        //                                         [ServiceBus("SynchronizeDatabaseCustomersStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        //{
        //    try
        //    {
        //        // Add start message to the queue
        //        var statusModel = new SynchronizeDatabaseStatusModel { Message = "Getting all customers from shopify." };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });

        //        // Start database sync and return completed message
        //        var perPage = 100;
        //        var shopifyCustomerRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = GetShopifyBaseSite(request.BaseSite), ResourceType = ShopifyResourceType.customers };
        //        var shopifyCustomerService = new ShopifyServices.ShopifyCustomerService();
        //        var shopifyCountResponse = shopifyCustomerService.GetShopifyRecordCount(shopifyCustomerRequest);
        //        var pageCount = shopifyCountResponse.Count / perPage;
        //        if (shopifyCountResponse.Count % perPage != 0) pageCount++;

        //        SaveNewSyncStatusRecord(request.SessionId, "Customers", shopifyCountResponse.Count, request.BaseSite);
        //        var syncMessage = $"Synchronizing {shopifyCountResponse.Count} customers";
        //        statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = 0 };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });

        //        var databaseShopifyCustomerService = new DatabaseServices.ShopifyCustomerService();
        //        var mapperService = new DatabaseServices.MapperService();
        //        for (int i = 0; i <= pageCount; i++)
        //        {
        //            var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = GetShopifyBaseSite(request.BaseSite), Parameters = new Dictionary<string, string>() };
        //            perPageRequest.Parameters.Add("limit", perPage.ToString());
        //            perPageRequest.Parameters.Add("page", i.ToString());
        //            var perPageResponse = shopifyCustomerService.GetShopifyCustomersPerPage(perPageRequest);
        //            if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
        //            foreach (var customer in perPageResponse.Customers)
        //            {

        //                var saveShopifyCustomerRequest = new DatabaseRequestAndResponses.SaveShopifyCustomerRequest { CompanySiteId = request.BaseSite.SiteId };
        //                mapperService.Map(customer, saveShopifyCustomerRequest.Customer);
        //                var saveShopifyCustomerResponse = databaseShopifyCustomerService.SaveShopifyCustomer(saveShopifyCustomerRequest);
        //                if (!saveShopifyCustomerResponse.IsSuccess)
        //                    log.WriteLine($"Customer Error [Customer: {customer.Id}][Error: {saveShopifyCustomerResponse.ErrorMessage}]");
        //                else
        //                {
        //                    //var metafieldRequest = new ShopifyRequestAndResponses.GetShopifyRecordRequest
        //                    //{
        //                    //    RecordId = customer.Id
        //                    //};
        //                    //var metafieldResponse = shopifyCustomerService.GetShopifyCustomerMetafields(metafieldRequest);
        //                    //if (!metafieldResponse.IsSuccess)
        //                    //    Console.WriteLine($"Customer Get Metafield Error [Customer: {customer.Id}][Error: {metafieldResponse.ErrorMessage}]");
        //                    //else
        //                    //{
        //                    //    foreach (var metafield in metafieldResponse.Metafields)
        //                    //    {
        //                    //        var saveShopifyMetafieldRequest = new DatabaseRequestAndResponses.SaveShopifyMetafieldRequest { };
        //                    //        mapperService.Map(metafield, saveShopifyMetafieldRequest.Metafield);
        //                    //        var saveShopifyMetafieldResponse = databaseShopifyCustomerService.SaveShopifyCustomerMetafield(saveShopifyMetafieldRequest);
        //                    //        if (!saveShopifyMetafieldResponse.IsSuccess)
        //                    //            Console.WriteLine($"Customer Save Metafield Error [Customer: {customer.Id}][Metafield: {metafield.Id}][Error: {metafieldResponse.ErrorMessage}]");
        //                    //    }
        //                    //}
        //                }
        //            }

        //            statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = perPage * i };
        //            responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //        }

        //        SaveCompletedSyncStatusRecord(Guid.Parse(request.SessionId), request.BaseSite);
        //        statusModel = new SynchronizeDatabaseStatusModel
        //        {
        //            Message = "Synchronization of customers in the database is complete.",
        //            TotalRecordsToSync = shopifyCountResponse.Count,
        //            RecordsSynced = shopifyCountResponse.Count,
        //            IsProcessComplete = true,
        //        };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.WriteLine("Unable to synchronize customers");
        //        log.WriteLine(ex.Message);
        //        SaveErrorSyncStatusRecord(Guid.Parse(request.SessionId), request.BaseSite);
        //        var databaseShopifyCustomerService = new DatabaseServices.ShopifyCustomerService();
        //        databaseShopifyCustomerService.LoggingService.LogError(new DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
        //        var statusModel = new SynchronizeDatabaseStatusModel { Message = "Unable to synchronize customers, check logs for issues.", IsProcessComplete = true };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //    }
        //}

        //public void SynchronizeDatabaseProducts([ServiceBusTrigger("SynchronizeDatabaseProducts-Request")] DatabaseRequestAndResponses.SynchronizeDatabaseProductsRequest request,
        //                                        [ServiceBus("SynchronizeDatabaseProductsStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        //{
        //    try
        //    {
        //        // Add start message to the queue
        //        var statusModel = new SynchronizeDatabaseStatusModel { Message = "Getting all products from shopify." };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });

        //        // Start database sync and return completed message
        //        var perPage = 100;
        //        var shopifyProductRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = GetShopifyBaseSite(request.BaseSite), ResourceType = ShopifyResourceType.products };
        //        var shopifyProductService = new ShopifyServices.ShopifyProductService();
        //        var shopifyCountResponse = shopifyProductService.GetShopifyRecordCount(shopifyProductRequest);
        //        var pageCount = shopifyCountResponse.Count / perPage;
        //        if (shopifyCountResponse.Count % perPage != 0) pageCount++;

        //        SaveNewSyncStatusRecord(Guid.Parse(request.SessionId), "Products", shopifyCountResponse.Count, request.BaseSite);
        //        var syncMessage = $"Synchronizing {shopifyCountResponse.Count} products";
        //        statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = 0 };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });

        //        var databaseShopifyProductService = new DatabaseServices.ShopifyProductService();
        //        var mapperService = new DatabaseServices.MapperService();
        //        for (int i = 0; i <= pageCount; i++)
        //        {
        //            var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = GetShopifyBaseSite(request.BaseSite), Parameters = new Dictionary<string, string>() };
        //            perPageRequest.Parameters.Add("limit", perPage.ToString());
        //            perPageRequest.Parameters.Add("page", i.ToString());
        //            var perPageResponse = shopifyProductService.GetShopifyProductsPerPage(perPageRequest);
        //            if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
        //            foreach (var product in perPageResponse.Products)
        //            {

        //                var saveShopifyProductRequest = new DatabaseRequestAndResponses.SaveShopifyProductRequest { CompanySiteId = request.BaseSite.SiteId };
        //                mapperService.Map(product, saveShopifyProductRequest.Product);
        //                var saveShopifyProductResponse = databaseShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
        //                if (!saveShopifyProductResponse.IsSuccess)
        //                    log.WriteLine($"Product Error [Product: {product.Id}][Error: {saveShopifyProductResponse.ErrorMessage}]");
        //                else
        //                {
        //                    //var metafieldRequest = new ShopifyRequestAndResponses.GetShopifyRecordRequest
        //                    //{
        //                    //    RecordId = product.Id
        //                    //};
        //                    //var metafieldResponse = shopifyProductService.GetShopifyProductMetafields(metafieldRequest);
        //                    //if (!metafieldResponse.IsSuccess)
        //                    //    Console.WriteLine($"Product Get Metafield Error [Product: {product.Id}][Error: {metafieldResponse.ErrorMessage}]");
        //                    //else
        //                    //{
        //                    //    foreach (var metafield in metafieldResponse.Metafields)
        //                    //    {
        //                    //        var saveShopifyMetafieldRequest = new DatabaseRequestAndResponses.SaveShopifyMetafieldRequest { };
        //                    //        mapperService.Map(metafield, saveShopifyMetafieldRequest.Metafield);
        //                    //        var saveShopifyMetafieldResponse = databaseShopifyProductService.SaveShopifyProductMetafield(saveShopifyMetafieldRequest);
        //                    //        if (!saveShopifyMetafieldResponse.IsSuccess)
        //                    //            Console.WriteLine($"Product Save Metafield Error [Product: {product.Id}][Metafield: {metafield.Id}][Error: {metafieldResponse.ErrorMessage}]");
        //                    //    }
        //                    //}
        //                }
        //            }

        //            statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = perPage * i };
        //            responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //        }

        //        SaveCompletedSyncStatusRecord(Guid.Parse(request.SessionId), request.BaseSite);
        //        statusModel = new SynchronizeDatabaseStatusModel
        //        {
        //            Message = "Synchronization of products in the database is complete.",
        //            TotalRecordsToSync = shopifyCountResponse.Count,
        //            RecordsSynced = shopifyCountResponse.Count,
        //            IsProcessComplete = true,
        //        };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.WriteLine("Unable to synchronize products.");
        //        log.WriteLine(ex.Message);
        //        SaveErrorSyncStatusRecord(Guid.Parse(request.SessionId), request.BaseSite);
        //        var databaseShopifyProductService = new DatabaseServices.ShopifyProductService();
        //        databaseShopifyProductService.LoggingService.LogError(new DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
        //        var statusModel = new SynchronizeDatabaseStatusModel { Message = "Unable to synchronize products, check logs for issues.", IsProcessComplete = true };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //    }
        //}

        //public static void SynchronizeDatabaseOrders([ServiceBusTrigger("SynchronizeDatabaseOrders-Request")] SynchronizeDatabaseOrdersRequest request, [ServiceBus("SynchronizeDatabaseOrdersStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        //{
        //    SynchronizeDatabaseStatusModel queueMessage = null;
        //    try
        //    {
        //        // Add start message to the queue
        //        var statusModel = new SynchronizeDatabaseStatusModel { Message = "Getting all orders from shopify." };
        //        responseQueue.Add(new BrokeredMessage(queueMessage) { SessionId = request.SessionId });

        //        // Start database sync and return completed message
        //        var response = new SynchronizeDatabaseOrdersResponse();
        //        var perPage = 20;
        //        var shopifyOrderRequest = new Common.RequestAndResponses.ShopifyRequestAndResponses.GetShopifyRecordsRequest { ResourceType = ShopifyResourceType.orders };
        //        var baseService = new Services.ShopifyServices.ShopifyBaseService(request.CompanySiteId);
        //        var shopifyCountResponse = baseService.GetShopifyRecordCount(shopifyOrderRequest);
        //        var pageCount = shopifyCountResponse.Count / perPage;
        //        if (shopifyCountResponse.Count % perPage != 0) pageCount++;
        //        var syncMessage = "Synchronizing " + shopifyCountResponse.Count.ToString() + " orders";

        //        statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = 0 };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });

        //        for (int i = 0; i <= pageCount; i++)
        //        {
        //            var perPageRequest = new Common.RequestAndResponses.ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest
        //            {
        //                NumberPerPage = perPage,
        //                PageNumber = i
        //            };
        //            var perPageResponse = new Services.ShopifyServices.ShopifyOrderService().GetShopifyOrdersPerPage(perPageRequest);
        //            if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
        //            foreach (var order in perPageResponse.Orders)
        //            {
        //                var saveShopifyOrderRequest = new SaveShopifyOrderRequest {};
        //                Mappers.MobMapper.Map(order, saveShopifyOrderRequest.Order);
        //                saveShopifyOrderRequest.Order.CompanySiteId = baseService.CompanySite.Id;
        //                var saveShopifyOrderResponse = ShopifyOrderService.SaveShopifyOrder(saveShopifyOrderRequest);
        //                if (!saveShopifyOrderResponse.IsSuccess) log.WriteLine("Order Error [Order: " + order.Id + "][Error: " + saveShopifyOrderResponse.ErrorMessage + "]");
        //                else
        //                {

        //                    var getTransactionsRequest = new Common.RequestAndResponses.ShopifyRequestAndResponses.GetShopifyRecordRequest { CompanySiteId = baseService.CompanySite.Id, RecordId = order.Id };
        //                    var getTransactionsResponse = new Services.ShopifyServices.ShopifyOrderService().GetShopifyOrderTransactions(getTransactionsRequest);
        //                    foreach (var transaction in getTransactionsResponse.OrderTransactions)
        //                    {
        //                        var saveShopifyOrderTransactionsRequest = new SaveShopifyOrderTransactionRequest { UserToken = request.UserToken };
        //                        Mappers.MobMapper.Map(transaction, saveShopifyOrderTransactionsRequest.OrderTransaction);
        //                        saveShopifyOrderTransactionsRequest.OrderTransaction.CompanySiteId = baseService.CompanySite.Id;
        //                        var saveShopifyOrderTransactionResponse = ShopifyOrderService.SaveShopifyOrderTransaction(saveShopifyOrderTransactionsRequest);
        //                        if (!saveShopifyOrderTransactionResponse.IsSuccess)
        //                            log.WriteLine("Order Transaction Error [Order: " + order.Id + "][Transaction: " + transaction.Id + "][Error: " + saveShopifyOrderTransactionResponse.ErrorMessage + "]");
        //                    }
        //                }
        //            }

        //            statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCountResponse.Count, RecordsSynced = perPage * i };
        //            responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //        }

        //        statusModel = new SynchronizeDatabaseStatusModel
        //        {
        //            Message = "Synchronization of orders in the database is complete.",
        //            TotalRecordsToSync = shopifyCountResponse.Count,
        //            RecordsSynced = shopifyCountResponse.Count,
        //            IsProcessComplete = true,
        //        };
        //        responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = request.SessionId });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.WriteLine("Unable to sync database orders.");
        //        log.WriteLine(ex.Message);
        //        LoggingService.LogError(new LogErrorRequest { ex = ex });
        //        queueMessage = new SynchronizeDatabaseStatusModel { Message = "Unable to synchronize database orders, check logs for issues.", IsProcessComplete = true };
        //        responseQueue.Add(new BrokeredMessage(queueMessage) { SessionId = request.SessionId });
        //    }
        //}

        public void SynchronizeDatabaseCustomSollections([ServiceBusTrigger("SynchronizeDatabaseCustomCollections-Request")] DatabaseRequestAndResponses.BaseSyncRequest request,
                                                         [ServiceBus("SynchronizeDatabaseCustomCollectionsStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        {
            try
            {
                // Add start message to the queue
                var sessionId = request.BaseSite.SiteId.ToString();
                var statusModel = new SynchronizeDatabaseStatusModel { Message = "Getting all custom collections from shopify." };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });

                // Start database sync and return completed message
                var perPage = 20;
                var shopifyCustomCollectionRequest = new ShopifyRequestAndResponses.GetShopifyRecordsRequest { BaseSite = GetShopifyBaseSite(request.BaseSite) };
                var shopifyCollectionService = new ShopifyServices.ShopifyCollectionService();
                var shopifyCustomCollectionCountResponse = shopifyCollectionService.GetShopifyRecordCount(shopifyCustomCollectionRequest);
                var pageCount = shopifyCustomCollectionCountResponse.Count / perPage;
                if (shopifyCustomCollectionCountResponse.Count % perPage != 0) pageCount++;

                SaveNewSyncStatusRecord(sessionId, "CustomCollections", shopifyCustomCollectionCountResponse.Count, request.BaseSite);
                var syncMessage = $"Synchronizing {shopifyCustomCollectionCountResponse.Count} custom collections";
                statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCustomCollectionCountResponse.Count, RecordsSynced = 0 };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });

                for (int i = 0; i <= pageCount; i++)
                {
                    var perPageRequest = new ShopifyRequestAndResponses.GetShopifyRecordsPerPageRequest { BaseSite = GetShopifyBaseSite(request.BaseSite), Parameters = new Dictionary<string, string>() };
                    perPageRequest.Parameters.Add("limit", perPage.ToString());
                    perPageRequest.Parameters.Add("page", i.ToString());
                    var perPageResponse = shopifyCollectionService.GetShopifyCustomCollectionsPerPage(perPageRequest);
                    if (!perPageResponse.IsSuccess) throw new ApplicationException(perPageResponse.ErrorMessage);
                    foreach (var customCollection in perPageResponse.CustomCollections)
                    {

                        var saveShopifyCustomCollectionRequest = new DatabaseRequestAndResponses.SaveShopifyCustomCollectionFromShopifyRequest { CompanySiteId = request.BaseSite.SiteId, CustomCollection = customCollection };
                        var saveShopifyCustomCollectionResponse = new DatabaseServices.ShopifyCollectionService().SaveShopifyCustomCollectionFromShopify(saveShopifyCustomCollectionRequest);
                        if (!saveShopifyCustomCollectionResponse.IsSuccess)
                            log.WriteLine($"Custom Collections Error [Custom Collection: {customCollection.Id}][Error: {saveShopifyCustomCollectionResponse.ErrorMessage}]");
                    }

                    statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = shopifyCustomCollectionCountResponse.Count, RecordsSynced = perPage * i };
                    responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
                }

                SaveCompletedSyncStatusRecord(sessionId, request.BaseSite);
                statusModel = new SynchronizeDatabaseStatusModel
                {
                    Message = "Synchronization of custom collections in the database is complete.",
                    TotalRecordsToSync = shopifyCustomCollectionCountResponse.Count,
                    RecordsSynced = shopifyCustomCollectionCountResponse.Count,
                    IsProcessComplete = true,
                };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
            }
            catch (Exception ex)
            {
                log.WriteLine("Unable to synchronize custom collections");
                log.WriteLine(ex.Message);
                var sessionId = request.BaseSite.SiteId.ToString();
                SaveErrorSyncStatusRecord(sessionId, request.BaseSite);
                new DatabaseServices.BaseService().LoggingService.LogError(new DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                var statusModel = new SynchronizeDatabaseStatusModel { Message = "Unable to synchronize custom collections, check logs for issues.", IsProcessComplete = true };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
            }
        }

        public void SynchronizeDatabaseUsers([ServiceBusTrigger("SynchronizeDatabaseUsers-Request")] DatabaseRequestAndResponses.BaseSyncRequest request,
                                             [ServiceBus("SynchronizeDatabaseUsersStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        {
            try
            {
                // Add start message to the queue
                var sessionId = request.BaseSite.SiteId.ToString();
                var statusModel = new SynchronizeDatabaseStatusModel { Message = "Getting all users from shopify." };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });

                var shopifyUserRequest = new ShopifyRequestAndResponses.GetShopifyRecordByQueryRequest { BaseSite = GetShopifyBaseSite(request.BaseSite) };
                var shopifyUserResponse = new ShopifyServices.ShopifyUserService().GetShopifyUsers(shopifyUserRequest);
                if (!shopifyUserResponse.IsSuccess) throw new ApplicationException(shopifyUserResponse.ErrorMessage);

                var userCount = shopifyUserResponse.Users.Count;
                SaveNewSyncStatusRecord(sessionId, "Users", userCount, request.BaseSite);

                var syncMessage = $"Synchronizing {userCount} shopify users";
                statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = userCount, RecordsSynced = 0 };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId =sessionId });

                foreach (var user in shopifyUserResponse.Users)
                {
                    var saveShopifyUserRequest = new DatabaseRequestAndResponses.SaveShopifyUserFromShopifyRequest { CompanySiteId = request.BaseSite.SiteId, User = user };
                    var saveShopifyUserResponse = new DatabaseServices.ShopfiyUserService().SaveShopifyUserFromShopify(saveShopifyUserRequest);
                    if (!saveShopifyUserResponse.IsSuccess)
                        log.WriteLine($"User Error [User: {user.Id}][Error: {saveShopifyUserResponse.ErrorMessage}]");

                    statusModel = new SynchronizeDatabaseStatusModel { Message = syncMessage, TotalRecordsToSync = userCount, RecordsSynced = shopifyUserResponse.Users.IndexOf(user) };
                    responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
                }

                SaveCompletedSyncStatusRecord(sessionId, request.BaseSite);
                statusModel = new SynchronizeDatabaseStatusModel
                {
                    Message = "Synchronization of shopify users in the database is complete.",
                    TotalRecordsToSync = userCount,
                    RecordsSynced = userCount,
                    IsProcessComplete = true,
                };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
            }
            catch (Exception ex)
            {
                log.WriteLine("Unable to synchronize users");
                log.WriteLine(ex.Message);
                var sessionId = request.BaseSite.SiteId.ToString();
                SaveErrorSyncStatusRecord(sessionId, request.BaseSite);
                new DatabaseServices.BaseService().LoggingService.LogError(new DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                var statusModel = new SynchronizeDatabaseStatusModel { Message = "Unable to synchronize shopify users, check azure logs for issues.", IsProcessComplete = true };
                responseQueue.Add(new BrokeredMessage(statusModel) { SessionId = sessionId });
            }
        }

        public static ShopifyModels.ShopifyBaseSiteModel GetShopifyBaseSite(BaseSiteModel siteModel)
        {
            var shopifyBaseSiteModel = new ShopifyModels.ShopifyBaseSiteModel
            {
                SiteId = siteModel.SiteId,
                BaseUrl = siteModel.BaseUrl,
                BaseWebhookUrl = siteModel.BaseWebhookUrl,
                PublicApiKey = siteModel.PublicApiKey,
                SecretApiKey = siteModel.SecretApiKey,
            };
            return shopifyBaseSiteModel;
        }

        private void SaveNewSyncStatusRecord(string sessionId, string entity, int count, BaseSiteModel baseSite)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var syncLogRecord = new DatabaseSyncModel
                {
                    Id = Guid.Parse(sessionId),
                    SycnedByAccountId = baseSite.AccountId,
                    Entity = entity,
                    Message = "",
                    NumberSynced = count,
                    CurrentlySyncing = true,
                    HadError = false,
                    DateStarted = now,
                    DateCreated = now
                };

                var saveDatabaseSyncStatusRequest = new DatabaseRequestAndResponses.SaveDatabaseSyncStatusRequest { CompanySiteId = baseSite.SiteId, SyncRecord = syncLogRecord };
                var saveDatabaseSyncStatusResponse = new DatabaseServices.DatabaseSyncService().SaveDatabaseSyncStatus(saveDatabaseSyncStatusRequest);
                if (!saveDatabaseSyncStatusResponse.IsSuccess)
                    throw new ApplicationException(saveDatabaseSyncStatusResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void SaveCompletedSyncStatusRecord(string sessionId, BaseSiteModel baseSite)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var syncLogRecord = new DatabaseSyncModel
                {
                    Id = Guid.Parse(sessionId),
                    DateEnded = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime),
                    Message = "Completed",
                    CurrentlySyncing = false
                };

                var saveDatabaseSyncStatusRequest = new DatabaseRequestAndResponses.SaveDatabaseSyncStatusRequest { CompanySiteId = baseSite.SiteId, SyncRecord = syncLogRecord };
                var saveDatabaseSyncStatusResponse = new DatabaseServices.DatabaseSyncService().SaveDatabaseSyncStatus(saveDatabaseSyncStatusRequest);
                if (!saveDatabaseSyncStatusResponse.IsSuccess)
                    throw new ApplicationException(saveDatabaseSyncStatusResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveErrorSyncStatusRecord(string sessionId, BaseSiteModel baseSite)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var syncLogRecord = new DatabaseSyncModel
                {
                    Id = Guid.Parse(sessionId),
                    Message = "Completed",
                    CurrentlySyncing = false,
                    HadError = true,
                    DateEnded = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime)
                };

                var saveDatabaseSyncStatusRequest = new DatabaseRequestAndResponses.SaveDatabaseSyncStatusRequest { CompanySiteId = baseSite.SiteId, SyncRecord = syncLogRecord };
                var saveDatabaseSyncStatusResponse = new DatabaseServices.DatabaseSyncService().SaveDatabaseSyncStatus(saveDatabaseSyncStatusRequest);
                if (!saveDatabaseSyncStatusResponse.IsSuccess)
                    throw new ApplicationException(saveDatabaseSyncStatusResponse.ErrorMessage);
            }
            catch (Exception) { }
        }
    }
}