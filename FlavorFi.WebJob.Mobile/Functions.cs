using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.IO;

namespace FlavorFi.WebJob.Mobile
{
    public class Functions
    {
        public static void SendPushNotificationAsync([ServiceBusTrigger("SendPushNotificationAsync-Request")] SendPushNotificationAsyncRequest request, [ServiceBus("GetSendPushNotificationStatus-Response")] ICollector<BrokeredMessage> responseQueue, TextWriter log)
        {
            BaseStatusModel queueMessage = null;
            try
            {
                // Add start message to the queue
                queueMessage = new BaseStatusModel { Message = "Sending push notification. This may take up to 5 min." };
                responseQueue.Add(new BrokeredMessage(queueMessage) { SessionId = request.SessionId });

               // Send push notification
                var pushNotificationRequest = new SendPushNotificationRequest { Note = request.Note, Title = request.Title, Message = request.Message, UserToken = request.UserToken };
                var pushNotificationResponse = MobileService.SendPushNotification(pushNotificationRequest);
                log.WriteLine(pushNotificationResponse.ErrorMessage);
                if (pushNotificationResponse.IsSuccess) queueMessage = new BaseStatusModel { Message = "Push notification has been successfully sent.", IsProcessComplete = true };
                else queueMessage = new BaseStatusModel { Message = pushNotificationResponse.ErrorMessage, IsProcessComplete = true };
                responseQueue.Add(new BrokeredMessage(queueMessage) { SessionId = request.SessionId });
            }
            catch (Exception ex)
            {
                log.WriteLine("Unable to send push notification.");
                log.WriteLine(ex.Message);
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                queueMessage = new BaseStatusModel { Message = "Unable to send push notificatio, check logs for issues.", IsProcessComplete = true };
                responseQueue.Add(new BrokeredMessage(queueMessage) { SessionId = request.SessionId });
            }
        }
    }
}