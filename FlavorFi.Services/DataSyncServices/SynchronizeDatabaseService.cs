using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DataSyncModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.DataSyncRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using Microsoft.ServiceBus.Messaging;
using System;

namespace FlavorFi.Services.DataSyncServices
{
    public class SynchronizeDatabaseService : BaseService
    {
        public GetDatabaseSyncStatusResponse GetDatabaseSyncStatus(GetDatabaseSyncStatusRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new GetDatabaseSyncStatusResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new GetDatabaseSyncStatusResponse();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetDatabaseSyncStatusResponse { ErrorMessage = "Unable to get databse sync status." };
            }
        }


        public SynchronizeDatabaseCustomersResponse SynchronizeDatabaseCustomers(SynchronizeDatabaseCustomersRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseCustomersResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseCustomersResponse();
                var brokeredMessage = new BrokeredMessage(request);
                brokeredMessage.SessionId = request.SessionId;
                SynchronizeDatabaseCustomersQueueClient.Send(new BrokeredMessage(request));
                response.IsSuccess = true;
                response.Message = "synchronization queued.";
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SynchronizeDatabaseCustomersResponse { ErrorMessage = "Unable to synchronize database customers." };
            }
        }

        public SynchronizeDatabaseCustomersStatusResponse SynchronizeDatabaseCustomersStatus(SynchronizeDatabaseCustomersStatusRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseCustomersStatusResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseCustomersStatusResponse();
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
                return new SynchronizeDatabaseCustomersStatusResponse { ErrorMessage = "Unable to retreive synchronize database customers status." };
            }
        }


        public SynchronizeDatabaseGiftCardsResponse SynchronizeDatabaseGiftCards(SynchronizeDatabaseGiftCardsRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseGiftCardsResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseGiftCardsResponse();
                var brokeredMessage = new BrokeredMessage(request);
                brokeredMessage.SessionId = request.SessionId;
                SynchronizeDatabaseGiftCardsQueueClient.Send(new BrokeredMessage(request));
                response.IsSuccess = true;
                response.Message = "synchronization queued.";
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SynchronizeDatabaseGiftCardsResponse { ErrorMessage = "Unable to synchronize database gift cards." };
            }
        }

        public SynchronizeDatabaseGiftCardsStatusResponse SynchronizeDatabaseGiftCardsStatus(SynchronizeDatabaseGiftCardsStatusRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseGiftCardsStatusResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseGiftCardsStatusResponse();
                MessageSession messageSession = null;
                while (messageSession == null)
                {
                    try { messageSession = SynchronizeDatabaseGiftCardsStatusQueueClient.AcceptMessageSession(request.SessionId, new TimeSpan(100)); }
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
                return new SynchronizeDatabaseGiftCardsStatusResponse { ErrorMessage = "Unable to retreive synchronize database gift cards status." };
            }
        }


        public SynchronizeDatabaseProductsResponse SynchronizeDatabaseProducts(SynchronizeDatabaseProductsRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseProductsResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseProductsResponse();
                var brokeredMessage = new BrokeredMessage(request);
                brokeredMessage.SessionId = request.SessionId;
                SynchronizeDatabaseProductsQueueClient.Send(new BrokeredMessage(request));
                response.IsSuccess = true;
                response.Message = "synchronization queued.";
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SynchronizeDatabaseProductsResponse { ErrorMessage = "Unable to synchronize database products." };
            }
        }

        public SynchronizeDatabaseProductsStatusResponse SynchronizeDatabaseProductsStatus(SynchronizeDatabaseProductsStatusRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseProductsStatusResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseProductsStatusResponse();
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
                return new SynchronizeDatabaseProductsStatusResponse { ErrorMessage = "Unable to retreive synchronize database products status." };
            }
        }


        public SynchronizeDatabaseOrdersResponse SynchronizeDatabaseOrders(SynchronizeDatabaseOrdersRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseOrdersResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseOrdersResponse();
                var brokeredMessage = new BrokeredMessage(request);
                brokeredMessage.SessionId = request.SessionId;
                SynchronizeDatabaseOrdersQueueClient.Send(new BrokeredMessage(request));
                response.IsSuccess = true;
                response.Message = "synchronization queued.";
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SynchronizeDatabaseOrdersResponse { ErrorMessage = "Unable to synchronize database orders." };
            }
        }

        public SynchronizeDatabaseOrdersStatusResponse SynchronizeDatabaseOrdersStatus(SynchronizeDatabaseOrdersStatusRequest request)
        {
            try
            {
                var user = Security.DecryptUserToken(request.UserToken);
                if (user == null) return new SynchronizeDatabaseOrdersStatusResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                var response = new SynchronizeDatabaseOrdersStatusResponse();
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
                return new SynchronizeDatabaseOrdersStatusResponse { ErrorMessage = "Unable to retreive synchronize database orders status." };
            }
        }
    }
}