using AutoMapper;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IMapperService
    {
        List<T> MapToList<U, T>(IQueryable<U> _source);
        T Map<U, T>(U source);
        T Map<U, T>(U source, T destination);
    }

    public class MapperService : IMapperService
    {
        public List<T> MapToList<U, T>(IQueryable<U> _source)
        {
            try
            {
                var lst = new List<T>();
                foreach (var item in _source)
                {
                    var model = Map<U, T>(item);
                    if (model != null) lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Map<U, T>(U source)
        {
            try
            {
                var mapper = CreateMap<U, T>();
                return mapper.Map<U, T>(source);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Map<U, T>(U source, T destination)
        {
            try
            {
                var mapper = CreateMap<U, T>();
                return mapper.Map<U, T>(source, destination);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IMapper CreateMap<U, T>()
        {
            // Account Mappings
            if (typeof(U) == typeof(Account) && typeof(T) == typeof(AccountModel))
                return MapAccountToAccountModel().CreateMapper();
            else if (typeof(U) == typeof(AccountModel) && typeof(T) == typeof(Account))
                return MapAccountModelToAccount().CreateMapper();

            // Company Mappings
            else if (typeof(U) == typeof(Company) && typeof(T) == typeof(CompanyModel))
                return MapCompanyToCompanyModel().CreateMapper();
            else if (typeof(U) == typeof(CompanyModel) && typeof(T) == typeof(Company))
                return MapCompanyModelToCompany().CreateMapper();
            else if (typeof(U) == typeof(CompanySite) && typeof(T) == typeof(CompanySiteModel))
                return MapCompanySiteToCompanySiteModel().CreateMapper();
            else if (typeof(U) == typeof(CompanySiteModel) && typeof(T) == typeof(CompanySite))
                return MapCompanySiteModelToCompanySite().CreateMapper();
            else if (typeof(U) == typeof(CompanySiteApplicationCrossLink) && typeof(T) == typeof(CompanySiteApplicationModel))
                return MapCompanySiteApplicationCrossLinkToCompanySiteApplicationModel().CreateMapper();

            // Customer Mappings
            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyCustomerModel) && typeof(T) == typeof(ShopifyCustomerModel))
                return MapShopifyCustomerModelToShopifyCustomerModel().CreateMapper();
            else if (typeof(U) == typeof(ShopifyCustomer) && typeof(T) == typeof(ShopifyCustomerModel))
                return MapShopifyCustomerToShopifyCustomerModel().CreateMapper();
            else if (typeof(U) == typeof(ShopifyCustomerModel) && typeof(T) == typeof(ShopifyCustomer))
                return MapShopifyCustomerModelToShopifyCustomer().CreateMapper();

            // Database Sync Mappings
            else if (typeof(U) == typeof(DatabaseSyncLog) && typeof(T) == typeof(DatabaseSyncModel))
                return MapDatabaseSyncLogToDatabaseSyncModel().CreateMapper();
            else if (typeof(U) == typeof(DatabaseSyncModel) && typeof(T) == typeof(DatabaseSyncLog))
                return MapDatabaseSyncModelToDatabaseSyncLog().CreateMapper();

            // Order mapping
            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyOrderModel) && typeof(T) == typeof(ShopifyOrderModel))
                return MapFromShopifyToShopifyOrderModel().CreateMapper();
            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyOrderTransactionModel) && typeof(T) == typeof(ShopifyOrderTransactionModel))
                return MapFromShopifyToShopifyOrderTransactionModel().CreateMapper();
            if (typeof(U) == typeof(ShopifyOrderModel) && typeof(T) == typeof(ShopifyOrder))
                return MapShopifyOrderModelToShopifyOrder().CreateMapper();
            if (typeof(U) == typeof(ShopifyOrderLineItemModel) && typeof(T) == typeof(ShopifyOrderLineItem))
                return MapShopifyOrderLineItemModelToShopifyOrderLineItem().CreateMapper();
            if (typeof(U) == typeof(ShopifyOrderTransactionModel) && typeof(T) == typeof(ShopifyOrderTransaction))
                return MapShopifyOrderTransactionModelToShopifyOrderTransaction().CreateMapper();

            // Product mapping
            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyProductModel) && typeof(T) == typeof(ShopifyProductModel))
                return MapShopifyProductModelToShopifyProductModel().CreateMapper();
            if (typeof(U) == typeof(ShopifyProductModel) && typeof(T) == typeof(ShopifyProduct))
                return MapShopifyProductModelToShopifyProduct().CreateMapper();
            if (typeof(U) == typeof(ShopifyProductVariantModel) && typeof(T) == typeof(ShopifyProductVariant))
                return MapShopifyProductVariantModelToShopifyProductVariant().CreateMapper();

            else
                throw new ApplicationException("No mapping configuration exists [" + typeof(T).ToString() + "]");
        }

        // Account mapping
        private static MapperConfiguration MapAccountToAccountModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Address, AddressModel>();
                config.CreateMap<Account, AccountModel>();
            });
        }
        private static MapperConfiguration MapAccountModelToAccount()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<AddressModel, Address>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.Address2 ?? ""))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<AccountModel, Account>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                      .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                      .ForMember(dest => dest.Password, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }

        // Company mapping
        private static MapperConfiguration MapCompanyToCompanyModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Company, CompanyModel>();
            });
        }
        private static MapperConfiguration MapCompanyModelToCompany()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanyModel, Company>();
                //.ForSourceMember(dest => dest.Id, opt => opt.())
                //.ForSourceMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapCompanySiteToCompanySiteModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanySite, CompanySiteModel>();
            });
        }
        private static MapperConfiguration MapCompanySiteModelToCompanySite()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanySiteModel, CompanySite>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapCompanySiteApplicationCrossLinkToCompanySiteApplicationModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanySiteApplicationCrossLink, CompanySiteApplicationModel>();
            });
        }
        private static MapperConfiguration MapCompanySiteSettingToCompanySiteSettingModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanySiteSetting, CompanySiteSettingModel>();
            });
        }
        private static MapperConfiguration MapCompanySiteSettingModelToCompanySiteSetting()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CompanySiteSettingModel, CompanySiteSetting>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }

        // Customer mapping
        private static MapperConfiguration MapShopifyCustomerModelToShopifyCustomerModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Common.Models.ShopifyModels.ShopifyCustomerModel, ShopifyCustomerModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyLastOrderId, opt => opt.MapFrom(src => src.LastOrderId))
                      .ForMember(dest => dest.Addresses, opt => opt.Ignore())
                      .ForMember(dest => dest.DefaultAddress, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyCustomerToShopifyCustomerModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Common.Models.ShopifyModels.ShopifyAddressModel, ShopifyAddressModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyCustomerId, opt => opt.MapFrom(src => src.CustomerId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<Common.Models.ShopifyModels.ShopifyCustomerModel, ShopifyCustomerModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyLastOrderId, opt => opt.MapFrom(src => src.LastOrderId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyCustomerModelToShopifyCustomer()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ShopifyCustomerModel, ShopifyCustomer>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }

        // Database Sync mapping
        private static MapperConfiguration MapDatabaseSyncLogToDatabaseSyncModel()
        {
            return new MapperConfiguration(config => config.CreateMap<DatabaseSyncLog, DatabaseSyncModel>());
        }
        private static MapperConfiguration MapDatabaseSyncModelToDatabaseSyncLog()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<DatabaseSyncModel, DatabaseSyncLog>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                      .ForMember(dest => dest.SyncedByAccountId, opt => opt.Ignore())
                      .ForMember(dest => dest.Entity, opt => opt.Ignore())
                      .ForMember(dest => dest.DateStarted, opt => opt.Ignore())
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }

        // Order mapping
        private static MapperConfiguration MapFromShopifyToShopifyOrderModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderLineItemModel, ShopifyOrderLineItemModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyProductId, opt => opt.MapFrom(src => src.ProductId))
                      .ForMember(dest => dest.OriginalShopifyProductVariantId, opt => opt.MapFrom(src => src.VariantId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<Common.Models.ShopifyModels.ShopifyAddressModel, ShopifyAddressModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyCustomerId, opt => opt.MapFrom(src => src.CustomerId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<Common.Models.ShopifyModels.ShopifyCustomerModel, ShopifyCustomerModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyLastOrderId, opt => opt.MapFrom(src => src.LastOrderId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderModel, ShopifyOrderModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapFromShopifyToShopifyOrderTransactionModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderTransactionModel, ShopifyOrderTransactionModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.MapFrom(src => src.OrderId))
                      .ForMember(dest => dest.OriginalShopifyGiftCardId, opt => opt.MapFrom(src => src.GiftCardId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyOrderModelToShopifyOrder()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ShopifyOrderModel, ShopifyOrder>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                        .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                        .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
                        .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyOrderLineItemModelToShopifyOrderLineItem()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ShopifyOrderLineItemModel, ShopifyOrderLineItem>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                        .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
                        .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                        .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.Ignore())
                        .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyOrderTransactionModelToShopifyOrderTransaction()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ShopifyOrderTransactionModel, ShopifyOrderTransaction>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                        .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
                        .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                        .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.Ignore())
                        .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }

        // Product mapping
        private static MapperConfiguration MapShopifyProductModelToShopifyProductModel()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Common.Models.ShopifyModels.ShopifyProductVariantModel, ShopifyProductVariantModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.ShopifyProductId, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.OriginalShopifyProductId, opt => opt.MapFrom(src => src.ProductId))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
                config.CreateMap<Common.Models.ShopifyModels.ShopifyProductModel, ShopifyProductModel>()
                      .ForMember(dest => dest.Id, opt => opt.Ignore())
                      .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
            });
        }
        private static MapperConfiguration MapShopifyProductModelToShopifyProduct()
        {
            return new MapperConfiguration(config => config.CreateMap<ShopifyProductModel, ShopifyProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore()));
        }
        private static MapperConfiguration MapShopifyProductVariantModelToShopifyProductVariant()
        {
            return new MapperConfiguration(config => config.CreateMap<ShopifyProductVariantModel, ShopifyProductVariant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
                .ForMember(dest => dest.ShopifyProductId, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalShopifyProductId, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore()));
        }
    }
}

//public static class MobMapper
//{
//    public static List<T> MapToList<U, T>(List<U> _source)
//    {
//        try
//        {
//            var lst = new List<T>();
//            foreach (var item in _source)
//            {
//                var model = Map<U, T>(item, default(T));
//                if (model != null) lst.Add(model);
//            }
//            return lst;
//        }
//        catch (Exception ex)
//        {
//            LoggingService.LogError(new LogErrorRequest { ex = ex });
//            throw ex;
//        }
//    }

//    public static T Map<U, T>(U _source, T _destination)
//    {
//        try
//        {
//            IMapper mapper = null;

//            // Address mapping
//            if (typeof(U) == typeof(Account) && typeof(T) == typeof(AccountModel))
//                mapper = MapAccountToAccountModel().CreateMapper();
//            if (typeof(U) == typeof(AccountModel) && typeof(T) == typeof(Account))
//                mapper = MapAccountModelToAccount().CreateMapper();

//            // Address mapping
//            if (typeof(U) == typeof(ShopifyAddressModel) && typeof(T) == typeof(ShopifyAddress))
//                mapper = MapShopifyAddressModelToShopifyAddress().CreateMapper();

//            // Company mapping
//            if (typeof(U) == typeof(Company) && typeof(T) == typeof(CompanyModel))
//                mapper = MapCompanyToCompanyModel().CreateMapper();
//            if (typeof(U) == typeof(CompanySite) && typeof(T) == typeof(CompanySiteModel))
//                mapper = MapCompanySiteToCompanySiteModel().CreateMapper();
//            if (typeof(U) == typeof(CompanySiteSetting) && typeof(T) == typeof(CompanySiteSettingModel))
//                mapper = MapCompanySiteSettingToCompanySiteSettingModel().CreateMapper();
//            if (typeof(U) == typeof(CompanyModel) && typeof(T) == typeof(Company))
//                mapper = MapCompanyModelToCompany().CreateMapper();
//            if (typeof(U) == typeof(CompanySiteModel) && typeof(T) == typeof(CompanySite))
//                mapper = MapCompanySiteModelToCompanySite().CreateMapper();
//            if (typeof(U) == typeof(CompanySiteSettingModel) && typeof(T) == typeof(CompanySiteSetting))
//                mapper = MapCompanySiteSettingModelToCompanySiteSetting().CreateMapper();

//            // Customer mapping
//            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyCustomerModel) && typeof(T) == typeof(ShopifyCustomerModel))
//                mapper = MapFromShopifyToShopifyCustomerModel().CreateMapper();
//            if (typeof(U) == typeof(ShopifyCustomerModel) && typeof(T) == typeof(ShopifyCustomer))
//                mapper = MapShopifyCustomerModelToShopifyCustomer().CreateMapper();

//            // Gift Card mapping
//            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyGiftCardModel) && typeof(T) == typeof(ShopifyGiftCardModel))
//                mapper = MapFromShopifyToShopifGiftCardModel().CreateMapper();
//            if (typeof(U) == typeof(ShopifyGiftCardModel) && typeof(T) == typeof(ShopifyGiftCard))
//                mapper = MapShopifyGiftCardModelToShopifyGiftCard().CreateMapper();

//            // Log mapping
//            if (typeof(U) == typeof(DatabaseSyncLogModel) && typeof(T) == typeof(DatabaseSyncLog))
//                mapper = MapShopifyOrderModelToShopifyOrder().CreateMapper();

//            // Metafield mapping
//            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyMetafieldModel) && typeof(T) == typeof(ShopifyMetafieldModel))
//                mapper = MapFromShopifyToShopifyMetafieldModel().CreateMapper();
//            if (typeof(U) == typeof(ShopifyMetafieldModel) && typeof(T) == typeof(ShopifyMetafield))
//                mapper = MapShopifyMetafieldModelToShopifyMetafield().CreateMapper();

//            // Order mapping
//            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyOrderModel) && typeof(T) == typeof(ShopifyOrderModel))
//                mapper = MapFromShopifyToShopifyOrderModel().CreateMapper();
//            if (typeof(U) == typeof(Common.Models.ShopifyModels.ShopifyOrderTransactionModel) && typeof(T) == typeof(ShopifyOrderTransactionModel))
//                mapper = MapFromShopifyToShopifyOrderTransactionModel().CreateMapper();
//            if (typeof(U) == typeof(ShopifyOrderModel) && typeof(T) == typeof(ShopifyOrder))
//                mapper = MapShopifyOrderModelToShopifyOrder().CreateMapper();
//            if (typeof(U) == typeof(ShopifyOrderLineItemModel) && typeof(T) == typeof(ShopifyOrderLineItem))
//                mapper = MapShopifyOrderLineItemModelToShopifyOrderLineItem().CreateMapper();
//            if (typeof(U) == typeof(ShopifyOrderTransactionModel) && typeof(T) == typeof(ShopifyOrderTransaction))
//                mapper = MapShopifyOrderTransactionModelToShopifyOrderTransaction().CreateMapper();


//            // Error
//            if (mapper == null)
//                throw new ApplicationException("No mapping congiuration exists [" + typeof(T).ToString() + "]");

//            return mapper.Map<U, T>(_source, _destination);
//        }
//        catch (Exception ex)
//        {
//            LoggingService.LogError(new LogErrorRequest { ex = ex });
//            throw ex;
//        }
//    }

//    // Account mapping
//    private static MapperConfiguration MapAccountToAccountModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<Account, AccountModel>();
//        });
//    }
//    private static MapperConfiguration MapAccountModelToAccount()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<AccountModel, Account>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }

//    // Address mapping
//    private static MapperConfiguration MapShopifyAddressModelToShopifyAddress()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyAddressModel, ShopifyAddress>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyCustomerId, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }

//    // Company mapping
//    private static MapperConfiguration MapCompanyToCompanyModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySite, CompanySiteModel>();
//            config.CreateMap<Company, CompanyModel>();
//        });
//    }
//    private static MapperConfiguration MapCompanySiteToCompanySiteModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySite, CompanySiteModel>();
//        });
//    }
//    private static MapperConfiguration MapCompanySiteSettingToCompanySiteSettingModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySiteSetting, CompanySiteSettingModel>();
//        });
//    }
//    private static MapperConfiguration MapCompanyModelToCompany()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySiteModel, CompanySite>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//            config.CreateMap<CompanyModel, Company>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapCompanySiteModelToCompanySite()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySiteModel, CompanySite>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapCompanySiteSettingModelToCompanySiteSetting()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<CompanySiteSettingModel, CompanySiteSetting>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }



//    // Gift Card mapping
//    private static MapperConfiguration MapFromShopifyToShopifGiftCardModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyGiftCardModel, ShopifyGiftCardModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyApiClientId, opt => opt.MapFrom(src => src.ApiClientId))
//                  .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.MapFrom(src => src.OrderId))
//                  .ForMember(dest => dest.OriginalShopifyCustomerId, opt => opt.MapFrom(src => src.CustomerId))
//                  .ForMember(dest => dest.OriginalShopifyLineItemId, opt => opt.MapFrom(src => src.LineItemId))
//                  .ForMember(dest => dest.OriginalShopifyUserId, opt => opt.MapFrom(src => src.UserId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapShopifyGiftCardModelToShopifyGiftCard()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyGiftCardModel, ShopifyGiftCard>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }

//    // Log mapping
//    private static MapperConfiguration MapDatabaseSyncLogModelToDatabaseSyncLog()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<DatabaseSyncLogModel, DatabaseSyncLog>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }

//    // Metafield mapping
//    private static MapperConfiguration MapFromShopifyToShopifyMetafieldModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyMetafieldModel, ShopifyMetafieldModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyOwnerId, opt => opt.MapFrom(src => src.OwnerId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapShopifyMetafieldModelToShopifyMetafield()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyMetafieldModel, ShopifyMetafield>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyCustomCollectionId, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyProductId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyOwnerId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OwnerResource, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }

//    // Order mapping
//    private static MapperConfiguration MapFromShopifyToShopifyOrderModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderLineItemModel, ShopifyOrderLineItemModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyProductId, opt => opt.MapFrom(src => src.ProductId))
//                  .ForMember(dest => dest.OriginalShopifyProductVariantId, opt => opt.MapFrom(src => src.VariantId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyAddressModel, ShopifyAddressModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyCustomerId, opt => opt.MapFrom(src => src.CustomerId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyCustomerModel, ShopifyCustomerModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.LastOrderId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyLastOrderId, opt => opt.MapFrom(src => src.LastOrderId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderModel, ShopifyOrderModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapFromShopifyToShopifyOrderTransactionModel()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<Common.Models.ShopifyModels.ShopifyOrderTransactionModel, ShopifyOrderTransactionModel>()
//                  .ForMember(dest => dest.Id, opt => opt.Ignore())
//                  .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                  .ForMember(dest => dest.OriginalShopifyId, opt => opt.MapFrom(src => src.Id))
//                  .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.MapFrom(src => src.OrderId))
//                  .ForMember(dest => dest.OriginalShopifyGiftCardId, opt => opt.MapFrom(src => src.GiftCardId))
//                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapShopifyOrderModelToShopifyOrder()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyOrderModel, ShopifyOrder>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyCustomerId, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapShopifyOrderLineItemModelToShopifyOrderLineItem()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyOrderLineItemModel, ShopifyOrderLineItem>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }
//    private static MapperConfiguration MapShopifyOrderTransactionModelToShopifyOrderTransaction()
//    {
//        return new MapperConfiguration(config =>
//        {
//            config.CreateMap<ShopifyOrderTransactionModel, ShopifyOrderTransaction>()
//                    .ForMember(dest => dest.Id, opt => opt.Ignore())
//                    .ForMember(dest => dest.CompanySiteId, opt => opt.Ignore())
//                    .ForMember(dest => dest.ShopifyOrderId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyId, opt => opt.Ignore())
//                    .ForMember(dest => dest.OriginalShopifyOrderId, opt => opt.Ignore())
//                    .ForMember(dest => dest.DateCreated, opt => opt.Ignore());
//        });
//    }