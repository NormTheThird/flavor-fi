using System;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using FlavorFi.Data.Interceptors;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IBaseService
    {

    }

    public class BaseService : IBaseService
    {
        private static string ServicesBusConnectionString = string.Empty;

        public ILoggingService LoggingService { get; private set; }
        public IMapperService MapperService { get; private set; }
        public BaseService()
        {
            try { ServicesBusConnectionString = ConfigurationManager.ConnectionStrings["AzureServiceBusSharedAccessKeyPrimary"].ConnectionString; }
            catch (Exception) { }

            this.LoggingService = new LoggingService();
            this.MapperService = new MapperService();
            DbInterception.Add(new TemporalTableCommandTreeInterceptor());
        }

        protected static NamespaceManager NamespaceMgr => NamespaceManager.CreateFromConnectionString(ServicesBusConnectionString);

        public static MessagingFactory MsgFactory => MessagingFactory.Create(NamespaceMgr.Address, NamespaceMgr.Settings.TokenProvider);

        private static QueueClient _synchronizeDatabaseCustomCollectionsQueueClient = null;
        protected static QueueClient SynchronizeDatabaseCustomCollectionsQueueClient
        {
            get => _synchronizeDatabaseCustomCollectionsQueueClient == null 
                ? MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomCollections-Request") 
                : _synchronizeDatabaseCustomCollectionsQueueClient;
        }

        private static QueueClient _synchronizeDatabaseCustomCollectionsStatusQueueClient = null;
        protected static QueueClient SynchronizeDatabaseCustomCollectionsStatusQueueClient
        {
            get => _synchronizeDatabaseCustomCollectionsStatusQueueClient == null 
                ? MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomCollectionsStatus-Response", ReceiveMode.ReceiveAndDelete) 
                : _synchronizeDatabaseCustomCollectionsStatusQueueClient;
        }

        protected static QueueClient SynchronizeDatabaseCustomersQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomers-Request");
        protected static QueueClient SynchronizeDatabaseCustomersStatusQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomersStatus-Response", ReceiveMode.ReceiveAndDelete);

        protected static QueueClient SynchronizeDatabaseOrdersQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseOrders-Request");
        protected static QueueClient SynchronizeDatabaseOrdersStatusQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseOrdersStatus-Response", ReceiveMode.ReceiveAndDelete);

        protected static QueueClient SynchronizeDatabaseProductsQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseProducts-Request");
        protected static QueueClient SynchronizeDatabaseProductsStatusQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseProductsStatus-Response", ReceiveMode.ReceiveAndDelete);

        protected static QueueClient SynchronizeDatabaseUsersQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseUsers-Request");
        protected static QueueClient SynchronizeDatabaseUsersStatusQueueClient => MsgFactory.CreateQueueClient("SynchronizeDatabaseUsersStatus-Response", ReceiveMode.ReceiveAndDelete);
    }
}