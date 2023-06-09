using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace FlavorFi.WebJob.Mobile
{
    class Program
    {
        private static string ServicesBusConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString;
        private static string DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
        private static List<string> RequestQueues = new List<string> { "SendPushNotificationAsync-Request" };
        private static List<string> ResponseQueues = new List<string> { "GetSendPushNotificationStatus-Response" };

        static void Main()
        {
            if (string.IsNullOrEmpty(ServicesBusConnectionString) || string.IsNullOrEmpty(DashboardConnectionString))
            {
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");
                Console.ReadLine();
                return;
            }

            CheckRequestQueues();
            CheckResponseQueues();
            //DeleteQueues();

            var config = new JobHostConfiguration();
            if (config.IsDevelopment) config.UseDevelopmentSettings();
            config.UseServiceBus();
            config.Queues.BatchSize = 1;
            var host = new JobHost(config);
            host.RunAndBlock();
        }

        private static void CheckRequestQueues()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ServicesBusConnectionString);
            foreach (var queue in RequestQueues)
                if (!namespaceManager.QueueExists(queue))
                {
                    var queueDescription = new QueueDescription(queue);
                    queueDescription.RequiresSession = false;
                    queueDescription.MaxDeliveryCount = 1;
                    queueDescription.DefaultMessageTimeToLive = new TimeSpan(1, 0, 0);
                    namespaceManager.CreateQueue(queueDescription);
                    Console.WriteLine("Request queue created: " + queue);
                }
        }

        private static void CheckResponseQueues()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ServicesBusConnectionString);
            foreach (var queue in ResponseQueues)
                if (!namespaceManager.QueueExists(queue))
                {
                    var queueDescription = new QueueDescription(queue);
                    queueDescription.RequiresSession = true;
                    queueDescription.MaxDeliveryCount = 1;
                    queueDescription.DefaultMessageTimeToLive = new TimeSpan(1, 0, 0);
                    namespaceManager.CreateQueue(queueDescription);
                    Console.WriteLine("Response queue created: " + queue);
                }
        }

        private static void DeleteQueues()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ServicesBusConnectionString);
            foreach (var queue in RequestQueues)
                if (namespaceManager.QueueExists(queue))
                {
                    namespaceManager.DeleteQueue(queue);
                    Console.WriteLine("Request Queue deleted: " + queue);
                }
            foreach (var queue in ResponseQueues)
                if (namespaceManager.QueueExists(queue))
                {
                    namespaceManager.DeleteQueue(queue);
                    Console.WriteLine("Response Queue deleted: " + queue);
                }
        }
    }
}