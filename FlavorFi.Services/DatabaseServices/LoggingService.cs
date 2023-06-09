using System;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System.Linq;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Enums;

namespace FlavorFi.Services.DatabaseServices
{
    public interface ILoggingService
    {
        LogErrorResponse LogError(LogErrorRequest request);
        LogFacebookWebhookResponse LogFacebookWebhook(LogFacebookWebhookRequest request);
        LogShopifyWebhookResponse LogShopifyWebhook(LogShopifyWebhookRequest request);
        LogShopifyWebhookActivityResponse LogShopifyWebhookActivity(LogShopifyWebhookActivityRequest request);
    }

    public class LoggingService : ILoggingService
    {
        public LogErrorResponse LogError(LogErrorRequest request)
        {
            try
            {
                var response = new LogErrorResponse();
                var innerExMessage = "";
                var entityError = "";
                if (request.ex.InnerException != null) innerExMessage = request.ex.InnerException.Message;
                if (request.ex.GetType().FullName.Equals("System.Data.Entity.Validation.DbEntityValidationException"))
                {
                    foreach (var entity in ((System.Data.Entity.Validation.DbEntityValidationException)request.ex)
                        .EntityValidationErrors)
                    {
                        entityError += "[Entity: " + entity.Entry.Entity.ToString() + " ";
                        entityError = entity.ValidationErrors.Aggregate(entityError,
                            (current, error) =>
                                current + ("[PropertyName: " + error.PropertyName + "][ErrorMessage: " +
                                           error.ErrorMessage + "]"));
                        entityError += "]";
                    }
                }

                var stackTrace = request.ex.StackTrace;
                if (stackTrace.Length > 3000)
                    stackTrace = stackTrace.Substring(0, 3000);

                using (var context = new FlavorFiEntities())
                {
                    var error = new ErrorLog
                    {
                        Id = Guid.NewGuid(),
                        HResult = request.ex.HResult,
                        Source = request.ex.Source,
                        ExceptionType = request.ex.GetType().Name,
                        ExceptionMessage = request.ex.Message + " " + entityError,
                        InnerExceptionMessage = innerExMessage,
                        StackTrace = stackTrace,
                        Parameters = "",
                        ReviewedByAccountId = Guid.Parse("203E40D9-9122-4C42-A417-C065DC9F2A21"),
                        ReviewedComments = "",
                        IsReviewed = false,
                        DateReviewed = null,
                        DateCreated = DateTimeOffset.Now,
                       
                    };

                    context.ErrorLogs.Add(error);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new LogErrorResponse { ErrorMessage = ex.Message };
            }
        }

        public LogFacebookWebhookResponse LogFacebookWebhook(LogFacebookWebhookRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new LogFacebookWebhookResponse();
                using (var context = new FlavorFiEntities())
                {
                    var log = new FacebookWebhookLog
                    {
                        Id = Guid.NewGuid(),
                        CompanySiteId = request.CompanySiteId,
                        EntryId = request.FacebookWebhookLog.EntryId,
                        Entry = request.FacebookWebhookLog.Entry,
                        Type = request.FacebookWebhookLog.Type,
                        TimeSent = request.FacebookWebhookLog.TimeSent,
                        DateCreated = DateTimeOffset.Now
                    };
                    context.FacebookWebhookLogs.Add(log);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogError(new LogErrorRequest { ex = ex });
                return new LogFacebookWebhookResponse { ErrorMessage = ex.Message };
            }
        }

        public LogShopifyWebhookResponse LogShopifyWebhook(LogShopifyWebhookRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new LogShopifyWebhookResponse();
                using (var context = new FlavorFiEntities())
                {
                    var log = new ShopifyWebhookLog
                    {
                        Id = Guid.NewGuid(),
                        CompanySiteId = request.CompanySiteId,
                        ShopifyWebhookId = request.ShopifyWebhookId,
                        RecordId = request.RecordId,
                        Verified = true,
                        DateCreated = now
                    };

                    context.ShopifyWebhookLogs.Add(log);
                    context.SaveChanges();

                    response.ShopifyWebhookLogId = log.Id;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogError(new LogErrorRequest { ex = ex });
                return new LogShopifyWebhookResponse { ErrorMessage = ex.Message };
            }
        }

        public LogShopifyWebhookActivityResponse LogShopifyWebhookActivity(LogShopifyWebhookActivityRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new LogShopifyWebhookActivityResponse();
                using (var context = new FlavorFiEntities())
                {
                    var log = new ShopifyWebhookActivityLog
                    {
                        Id = Guid.NewGuid(),
                        ShopifyWebhookLogId = request.ShopifyWebhookLogId,
                        Acivity = request.Activity,
                        DateCreated = now
                    };

                    context.ShopifyWebhookActivityLogs.Add(log);
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogError(new LogErrorRequest { ex = ex });
                return new LogShopifyWebhookActivityResponse { ErrorMessage = ex.Message };
            }
        }

        //public static SaveDatabaseSyncLogResponse LogDatabaseSync(SaveDatabaseSyncLogRequest request)
        //{
        //    try
        //    {
        //        var user = Security.DecryptUserToken(request.UserToken);
        //        if (user == null) throw new ApplicationException("Not Authenticated!");

        //        var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
        //        var response = new SaveDatabaseSyncLogResponse();
        //        using (var context = new FlavorFiEntities())
        //        {
        //            var log = context.DatabaseSyncLogs.FirstOrDefault(l => l.Resource.Equals(request.Log.Resource.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //            if (log == null)
        //            {
        //                log = new DatabaseSyncLog { Id = Guid.NewGuid(), CompanySiteId = request.CompanySiteId, DateCreated = now };
        //                context.DatabaseSyncLogs.Add(log);
        //            }
        //            Mappers.MobMapper.Map(request.Log, log);
        //            context.SaveChanges();
        //            response.IsSuccess = true;
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(new LogErrorRequest { ex = ex });
        //        return new SaveDatabaseSyncLogResponse { ErrorMessage = ex.Message };
        //    }
        //}
    }
}