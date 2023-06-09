using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IShopifyUserService
    {
        SaveShopifyUserFromShopifyResponse SaveShopifyUserFromShopify(SaveShopifyUserFromShopifyRequest request);
    }

    public class ShopfiyUserService : BaseService, IShopifyUserService
    {
        /// <summary>
        ///     Adds and updates a user from shopify to the database
        /// </summary>
        /// <param name="request">SaveShopifyUserRequest</param>
        /// <returns>SaveShopifyUserResponse</returns>
        public SaveShopifyUserFromShopifyResponse SaveShopifyUserFromShopify(SaveShopifyUserFromShopifyRequest request)
        {
            try
            {
                //TODO: TREY: 10/27/2019 Fix Shopify User
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveShopifyUserFromShopifyResponse();
                using (var context = new FlavorFiEntities())
                {
                    var shopifyUser = context.ShopifyUsers.FirstOrDefault(_ => _.OriginalShopifyId.Equals(request.User.Id));
                    if (shopifyUser == null)
                    {
                        shopifyUser = new ShopifyUser
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            //AccountId = null,
                            OriginalShopifyId = request.User.Id,
                            DateCreated = now
                        };
                        context.ShopifyUsers.Add(shopifyUser);                  
                    }

                    //shopifyUser.FirstName = request.User.FirstName ?? "";
                    //shopifyUser.LastName = request.User.LastName ?? "";
                    shopifyUser.Email = request.User.Email ?? "";
                   // shopifyUser.PhoneNumber = request.User.PhoneNumber ?? "";
                   // shopifyUser.ScreenName = request.User.ScreenName ?? "";
                    //shopifyUser.UserType = request.User.UserType ?? "";
                    shopifyUser.IsAccountOwner = request.User.AccountOwner;
                    shopifyUser.ReceiveAnnouncements = request.User.ReceiveAnnouncements.Equals(1);
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveShopifyUserFromShopifyResponse { ErrorMessage = "Unable to save shopify user from shopify." };
            }
        }
    }
}