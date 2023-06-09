using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IDatabaseSyncService
    {
        GetDatabaseSyncStatusResponse GetDatabaseSyncStatus(GetDatabaseSyncStatusRequest request);
        BaseSyncResponse SynchronizeDatabaseCustomCollections(BaseSyncRequest request);
        BaseSyncStatusResponse SynchronizeDatabaseCustomCollectionsStatus(BaseSyncStatusRequest request);
        BaseSyncResponse SynchronizeDatabaseCustomers(BaseSyncRequest request);
        BaseSyncStatusResponse SynchronizeDatabaseCustomersStatus(BaseSyncStatusRequest request);
        BaseSyncResponse SynchronizeDatabaseOrders(BaseSyncRequest request);
        BaseSyncStatusResponse SynchronizeDatabaseOrdersStatus(BaseSyncStatusRequest request);
        BaseSyncResponse SynchronizeDatabaseProducts(BaseSyncRequest request);
        BaseSyncStatusResponse SynchronizeDatabaseProductsStatus(BaseSyncStatusRequest request);
        BaseSyncResponse SynchronizeDatabaseUsers(BaseSyncRequest request);
        BaseSyncStatusResponse SynchronizeDatabaseUsersStatus(BaseSyncStatusRequest request);
    }

    public class DatabaseSyncService : BaseService, IDatabaseSyncService
    {
        public GetDatabaseSyncStatusResponse GetDatabaseSyncStatus(GetDatabaseSyncStatusRequest request)
        {
            try
            {
                var response = new GetDatabaseSyncStatusResponse();
                using (var context = new FlavorFiEntities())
                {
                    var syncRecord = context.DatabaseSyncLogs.AsNoTracking()
                                                             .OrderByDescending(sl => sl.DateCreated)
                                                             .FirstOrDefault(sl => sl.Entity.Equals(request.Entity, StringComparison.CurrentCultureIgnoreCase));
                    if (syncRecord != null)
                        response.SyncRecord = MapperService.Map<DatabaseSyncLog, DatabaseSyncModel>(syncRecord);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetDatabaseSyncStatusResponse { ErrorMessage = "Unable to get databse sync status." };
            }
        }

        public SaveDatabaseSyncStatusResponse SaveDatabaseSyncStatus(SaveDatabaseSyncStatusRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveDatabaseSyncStatusResponse();
                using (var context = new FlavorFiEntities())
                {
                    var syncLog = context.DatabaseSyncLogs.FirstOrDefault(sl => sl.Id == request.SyncRecord.Id);
                    if (syncLog == null)
                    {
                        syncLog = new DatabaseSyncLog
                        {
                            Id = request.SyncRecord.Id,
                            SyncedByAccountId = request.SyncRecord.SycnedByAccountId,
                            CompanySiteId = request.CompanySiteId,
                            Entity = request.SyncRecord.Entity,
                            NumberSynced = request.SyncRecord.NumberSynced,
                            DateStarted = request.SyncRecord.DateStarted,
                            DateCreated = request.SyncRecord.DateCreated
                        };
                        context.DatabaseSyncLogs.Add(syncLog);
                    }

                    syncLog.Message = request.SyncRecord.Message;
                    syncLog.CurrentlySyncing = request.SyncRecord.CurrentlySyncing;
                    syncLog.HadError = request.SyncRecord.HadError;
                    syncLog.DateEnded = request.SyncRecord.DateEnded;
                    context.SaveChanges();

                    response.SyncRecord = MapperService.Map<DatabaseSyncLog, DatabaseSyncModel>(syncLog);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveDatabaseSyncStatusResponse { ErrorMessage = "Unable to save databse sync status." };
            }
        }

        public BaseSyncResponse SynchronizeDatabaseCustomCollections(BaseSyncRequest request)
        {
            try
            {
                var response = new BaseSyncResponse();
                var brokeredMessage = new BrokeredMessage(request);
                var sessionId = request.BaseSite.SiteId.ToString();

                brokeredMessage.SessionId = sessionId;
                SynchronizeDatabaseCustomCollectionsQueueClient.Send(new BrokeredMessage(request));

                response.SessionId = sessionId;
                response.Message = "synchronization queued.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncResponse { ErrorMessage = "Unable to synchronize database custom collections." };
            }
        }

        public BaseSyncStatusResponse SynchronizeDatabaseCustomCollectionsStatus(BaseSyncStatusRequest request)
        {
            try
            {
                var response = new BaseSyncStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseCustomCollectionsStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
                    catch (TimeoutException) { }
                }

                var brokeredMessage = messageSession.Receive(new TimeSpan(1));
                while (brokeredMessage != null)
                {
                    response.Statuses.Add(brokeredMessage.GetBody<SynchronizeDatabaseStatusModel>());
                    brokeredMessage = messageSession.Receive(new TimeSpan(1));
                }
                messageSession.Close();

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncStatusResponse { ErrorMessage = "Unable to retreive synchronize database custom collections status." };
            }
        }

        public BaseSyncResponse SynchronizeDatabaseCustomers(BaseSyncRequest request)
        {
            try
            {
                var response = new BaseSyncResponse();
                var brokeredMessage = new BrokeredMessage(request);
                var sessionId = request.BaseSite.SiteId.ToString();

                brokeredMessage.SessionId = sessionId;
                SynchronizeDatabaseCustomersQueueClient.Send(new BrokeredMessage(request));

                response.SessionId = sessionId;
                response.Message = "synchronization queued.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncResponse { ErrorMessage = "Unable to synchronize database customers." };
            }
        }

        public BaseSyncStatusResponse SynchronizeDatabaseCustomersStatus(BaseSyncStatusRequest request)
        {
            try
            {
                var response = new BaseSyncStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseCustomersStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
                    catch (TimeoutException) { }
                }

                var brokeredMessage = messageSession.Receive(new TimeSpan(1));
                while (brokeredMessage != null)
                {
                    response.Statuses.Add(brokeredMessage.GetBody<SynchronizeDatabaseStatusModel>());
                    brokeredMessage = messageSession.Receive(new TimeSpan(1));
                }
                messageSession.Close();

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncStatusResponse { ErrorMessage = "Unable to retreive synchronize database customers status." };
            }
        }

        public BaseSyncResponse SynchronizeDatabaseOrders(BaseSyncRequest request)
        {
            try
            {
                var response = new BaseSyncResponse();
                var brokeredMessage = new BrokeredMessage(request);
                var sessionId = request.BaseSite.SiteId.ToString();

                brokeredMessage.SessionId = sessionId;
                SynchronizeDatabaseOrdersQueueClient.Send(new BrokeredMessage(request));

                response.SessionId = sessionId;
                response.Message = "synchronization queued.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncResponse { ErrorMessage = "Unable to synchronize database orders." };
            }
        }

        public BaseSyncStatusResponse SynchronizeDatabaseOrdersStatus(BaseSyncStatusRequest request)
        {
            try
            {
                var response = new BaseSyncStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseOrdersStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
                    catch (TimeoutException) { }
                }

                var brokeredMessage = messageSession.Receive(new TimeSpan(1));
                while (brokeredMessage != null)
                {
                    response.Statuses.Add(brokeredMessage.GetBody<SynchronizeDatabaseStatusModel>());
                    brokeredMessage = messageSession.Receive(new TimeSpan(1));
                }
                messageSession.Close();

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncStatusResponse { ErrorMessage = "Unable to retreive synchronize database orders status." };
            }
        }

        public BaseSyncResponse SynchronizeDatabaseProducts(BaseSyncRequest request)
        {
            try
            {
                var response = new BaseSyncResponse();
                var brokeredMessage = new BrokeredMessage(request);
                var sessionId = request.BaseSite.SiteId.ToString();

                brokeredMessage.SessionId = sessionId;
                SynchronizeDatabaseProductsQueueClient.Send(new BrokeredMessage(request));

                response.SessionId = sessionId;
                response.Message = "synchronization queued.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncResponse { ErrorMessage = "Unable to synchronize database products." };
            }
        }

        public BaseSyncStatusResponse SynchronizeDatabaseProductsStatus(BaseSyncStatusRequest request)
        {
            try
            {
                var response = new BaseSyncStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseProductsStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
                    catch (TimeoutException) { }
                }

                var brokeredMessage = messageSession.Receive(new TimeSpan(1));
                while (brokeredMessage != null)
                {
                    response.Statuses.Add(brokeredMessage.GetBody<SynchronizeDatabaseStatusModel>());
                    brokeredMessage = messageSession.Receive(new TimeSpan(1));
                }
                messageSession.Close();

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncStatusResponse { ErrorMessage = "Unable to retreive synchronize database products status." };
            }
        }

        public BaseSyncResponse SynchronizeDatabaseUsers(BaseSyncRequest request)
        {
            try
            {
                var response = new BaseSyncResponse();
                var brokeredMessage = new BrokeredMessage(request);
                var sessionId = request.BaseSite.SiteId.ToString();

                brokeredMessage.SessionId = sessionId;
                SynchronizeDatabaseUsersQueueClient.Send(new BrokeredMessage(request));

                response.SessionId = sessionId;
                response.Message = "synchronization queued.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncResponse { ErrorMessage = "Unable to synchronize database users." };
            }
        }

        public BaseSyncStatusResponse SynchronizeDatabaseUsersStatus(BaseSyncStatusRequest request)
        {
            try
            {
                var response = new BaseSyncStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseUsersStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
                    catch (TimeoutException) { }
                }

                var brokeredMessage = messageSession.Receive(new TimeSpan(1));
                while (brokeredMessage != null)
                {
                    response.Statuses.Add(brokeredMessage.GetBody<SynchronizeDatabaseStatusModel>());
                    brokeredMessage = messageSession.Receive(new TimeSpan(1));
                }
                messageSession.Close();

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new BaseSyncStatusResponse { ErrorMessage = "Unable to retreive synchronize database users status." };
            }
        }
    }
}