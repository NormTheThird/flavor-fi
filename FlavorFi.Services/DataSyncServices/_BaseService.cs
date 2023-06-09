using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace FlavorFi.Services.DataSyncServices
{
    public class BaseService
    {
        private static string ServicesBusConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString;

        private static NamespaceManager _namespaceMgr = null;
        protected static NamespaceManager NamespaceMgr
        {
            get
            {
                if (_namespaceMgr == null) _namespaceMgr = NamespaceManager.CreateFromConnectionString(ServicesBusConnectionString);
                return _namespaceMgr;
            }
        }

        private static MessagingFactory _msgFactory = null;
        public static MessagingFactory MsgFactory
        {
            get
            {
                if (_msgFactory == null) _msgFactory = MessagingFactory.Create(NamespaceMgr.Address, NamespaceMgr.Settings.TokenProvider);
                return _msgFactory;
            }
        }


        private static QueueClient _synchronizeDatabaseCustomersQueueClient = null;
        protected static QueueClient SynchronizeDatabaseCustomersQueueClient
        {
            get
            {
                if (_synchronizeDatabaseCustomersQueueClient == null)
                    _synchronizeDatabaseCustomersQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomers-Request");
                return _synchronizeDatabaseCustomersQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseCustomersStatusQueueClient = null;
        protected static QueueClient SynchronizeDatabaseCustomersStatusQueueClient
        {
            get
            {
                if (_synchronizeDatabaseCustomersStatusQueueClient == null)
                    _synchronizeDatabaseCustomersStatusQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseCustomersStatus-Response", ReceiveMode.ReceiveAndDelete);
                return _synchronizeDatabaseCustomersStatusQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseGiftCardsQueueClient = null;
        protected static QueueClient SynchronizeDatabaseGiftCardsQueueClient
        {
            get
            {
                if (_synchronizeDatabaseGiftCardsQueueClient == null)
                    _synchronizeDatabaseGiftCardsQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseGiftCards-Request");
                return _synchronizeDatabaseGiftCardsQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseGiftCardsStatusQueueClient = null;
        protected static QueueClient SynchronizeDatabaseGiftCardsStatusQueueClient
        {
            get
            {
                if (_synchronizeDatabaseGiftCardsStatusQueueClient == null)
                    _synchronizeDatabaseGiftCardsStatusQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseGiftCardsStatus-Response", ReceiveMode.ReceiveAndDelete);
                return _synchronizeDatabaseGiftCardsStatusQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseProductsQueueClient = null;
        protected static QueueClient SynchronizeDatabaseProductsQueueClient
        {
            get
            {
                if (_synchronizeDatabaseProductsQueueClient == null)
                    _synchronizeDatabaseProductsQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseProducts-Request");
                return _synchronizeDatabaseProductsQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseProductsStatusQueueClient = null;
        protected static QueueClient SynchronizeDatabaseProductsStatusQueueClient
        {
            get
            {
                if (_synchronizeDatabaseProductsStatusQueueClient == null)
                    _synchronizeDatabaseProductsStatusQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseProductsStatus-Response", ReceiveMode.ReceiveAndDelete);
                return _synchronizeDatabaseProductsStatusQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseOrdersQueueClient = null;
        protected static QueueClient SynchronizeDatabaseOrdersQueueClient
        {
            get
            {
                if (_synchronizeDatabaseOrdersQueueClient == null)
                    _synchronizeDatabaseOrdersQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseOrders-Request");
                return _synchronizeDatabaseOrdersQueueClient;
            }
        }

        private static QueueClient _synchronizeDatabaseOrdersStatusQueueClient = null;
        protected static QueueClient SynchronizeDatabaseOrdersStatusQueueClient
        {
            get
            {
                if (_synchronizeDatabaseOrdersStatusQueueClient == null)
                    _synchronizeDatabaseOrdersStatusQueueClient = MsgFactory.CreateQueueClient("SynchronizeDatabaseOrdersStatus-Response", ReceiveMode.ReceiveAndDelete);
                return _synchronizeDatabaseOrdersStatusQueueClient;
            }
        }
    }
}