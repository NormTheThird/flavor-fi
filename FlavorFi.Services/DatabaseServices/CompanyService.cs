using FlavorFi.Common.Enums;
using FlavorFi.Common.Helpers;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface ICompanyService : IBaseService
    {
        GetCompaniesResponse GetCompanies(GetCompaniesRequest request);
        GetCompanyResponse GetCompany(GetCompanyRequest request);
        SaveCompanyResponse SaveCompany(SaveCompanyRequest request);

        GetCompanySitesResponse GetCompanySites(GetCompanySitesRequest request);
        GetCompanySiteResponse GetCompanySite(GetCompanySiteRequest request);
        GetCompanySiteResponse GetCompanySiteByDomain(GetCompanySiteByDomainRequest request);

        GetCompanySiteApplicationsResponse GetCompanySiteApplications(GetCompanySiteApplicationsRequest request);
        GetCompanySiteApplicationResponse GetCompanySiteApplication(GetCompanySiteApplicationRequest request);
        SaveCompanySiteApplicationResponse SaveCompanySiteApplication(SaveCompanySiteApplicationRequest request);

        GetCompanySiteSettingsResponse GetCompanySiteSettings(GetCompanySiteSettingsRequest request);
        GetCompanySiteSettingResponse GetCompanySiteSetting(GetCompanySiteSettingRequest request);
        SaveCompanySiteSettingResponse SaveCompanySiteSetting(SaveCompanySiteSettingRequest request);
    }

    public class CompanyService : BaseService, ICompanyService
    {
        public GetCompaniesResponse GetCompanies(GetCompaniesRequest request)
        {
            try
            {
                var response = new GetCompaniesResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companies = context.Companies.AsNoTracking()
                                           .Where(c => request.GetActiveAndInactive ? (c.IsActive || !c.IsActive) : c.IsActive)
                                           .Select(c => new CompanyModel
                                           {
                                               Id = c.Id,
                                               Name = c.Name,
                                               Address1 = c.Address1,
                                               Address2 = c.Address2,
                                               City = c.City,
                                               State = c.State,
                                               ZipCode = c.ZipCode,
                                               IsActive = c.IsActive,
                                               DateCreated = c.DateCreated
                                           });
                    response.Companies = companies.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompaniesResponse { ErrorMessage = "Unable to get companies." };
            }
        }

        public GetCompanyResponse GetCompany(GetCompanyRequest request)
        {
            try
            {
                var response = new GetCompanyResponse();
                using (var context = new FlavorFiEntities())
                {
                    var company = context.Companies.AsNoTracking().FirstOrDefault(c => c.Id == request.CompanyId);
                    if (company == null) throw new ApplicationException($"Unable to find company for id {request.CompanyId}");
                    response.Company = MapperService.Map<Company, CompanyModel>(company);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanyResponse { ErrorMessage = "Unable to get company." };
            }
        }

        public SaveCompanyResponse SaveCompany(SaveCompanyRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveCompanyResponse();
                using (var context = new FlavorFiEntities())
                {
                    var company = context.Companies.FirstOrDefault(c => c.Id == request.Company.Id);
                    if (company == null)
                    {
                        company = new Company { Id = Guid.NewGuid(), DateCreated = now };
                        context.Companies.Add(company);
                    }

                    // TODO: TREY: 11/28/2018 This is not working. Changes are not saving.
                    //company = MapperService.Map<CompanyModel, Company>(request.Company);
                    company.Name = request.Company.Name;
                    company.Address1 = request.Company.Address1;
                    company.Address2 = request.Company.Address2;
                    company.City = request.Company.City;
                    company.State = request.Company.State;
                    company.ZipCode = request.Company.ZipCode;
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveCompanyResponse { ErrorMessage = "Unable to save company." };
            }
        }


        public GetCompanySitesResponse GetCompanySites(GetCompanySitesRequest request)
        {
            try
            {
                var response = new GetCompanySitesResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySites = context.CompanySites.AsNoTracking()
                                             .Where(cs => cs.CompanyId.Equals(request.CompanyId) && (request.GetActiveAndInactive
                                                 ? (cs.IsActive || !cs.IsActive) : (cs.IsActive)))
                                             .Select(cs => new CompanySiteModel
                                             {
                                                 Id = cs.Id,
                                                 CompanyId = cs.CompanyId,
                                                 Name = cs.Name,
                                                 ShopifyUrl = cs.ShopifyUrl,
                                                 ShopifyWebhookUrl = cs.ShopifyWebhookUrl,
                                                 ShopifyDomain = cs.ShopifyDomain,
                                                 ShopifyApiPublicKey = cs.ShopifyApiPublicKey,
                                                 ShopifyApiSecretKey = cs.ShopifyApiSecretKey,
                                                 ShopifySharedSecret = cs.ShopifySharedSecret,
                                                 IsActive = cs.IsActive,
                                                 DateCreated = cs.DateCreated
                                             });

                    response.CompanySites = companySites.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySitesResponse { ErrorMessage = "Unable to get company sites." };
            }
        }

        public GetCompanySiteResponse GetCompanySite(GetCompanySiteRequest request)
        {
            try
            {
                var response = new GetCompanySiteResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySite = context.CompanySites.AsNoTracking().FirstOrDefault(cs => cs.Id == request.CompanySiteId);
                    if (companySite == null) throw new ApplicationException($"Unable to find company site for id {request.CompanySiteId}");
                    response.CompanySite = MapperService.Map<CompanySite, CompanySiteModel>(companySite);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteResponse { ErrorMessage = "Unable to get company site." };
            }
        }

        public GetCompanySiteResponse GetCompanySiteByDomain(GetCompanySiteByDomainRequest request)
        {
            try
            {
                var response = new GetCompanySiteResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySite = context.CompanySites.AsNoTracking().FirstOrDefault(cs => cs.ShopifyDomain.Equals(request.Domain, StringComparison.CurrentCultureIgnoreCase));
                    if (companySite == null) throw new ApplicationException($"Unable to find company site for domain {request.Domain}");
                    response.CompanySite = MapperService.Map<CompanySite, CompanySiteModel>(companySite);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteResponse { ErrorMessage = "Unable to get company site by domain." };
            }
        }


        public GetCompanySiteApplicationsResponse GetCompanySiteApplications(GetCompanySiteApplicationsRequest request)
        {
            try
            {
                var response = new GetCompanySiteApplicationsResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySiteApplications = context.CompanySiteApplicationCrossLinks.AsNoTracking()
                                                         .Where(ca => ca.CompanySiteId.Equals(request.CompanySiteId))
                                                         .Select(ca => new CompanySiteApplicationModel
                                                         {
                                                             Id = ca.Id,
                                                             CompanySiteId = ca.CompanySiteId,
                                                             ApplicationId = ca.ApplicationId,
                                                             ApplicationName = ca.Application.Name,
                                                             AppApiPublicKey = ca.AppApiPublicKey,
                                                             AppApiSecretKey = ca.AppApiSecretKey,
                                                             AppSharedSecret = ca.AppSharedSecret,
                                                             Locations = ca.Locations,
                                                             IsEnabled = ca.IsEnabled,
                                                             DateCreated = ca.DateCreated
                                                         });

                    response.CompanySiteApplications = companySiteApplications.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteApplicationsResponse { ErrorMessage = "Unable to get company site applications." };
            }
        }

        public GetCompanySiteApplicationResponse GetCompanySiteApplication(GetCompanySiteApplicationRequest request)
        {
            try
            {
                var response = new GetCompanySiteApplicationResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySiteApplication = context.CompanySiteApplicationCrossLinks.AsNoTracking().FirstOrDefault(cs => cs.Id == request.CompanySiteApplicationId);
                    if (companySiteApplication == null) throw new ApplicationException($"Unable to find company site application for id {request.CompanySiteApplicationId}");
                    response.CompanySiteApplication = MapperService.Map<CompanySiteApplicationCrossLink, CompanySiteApplicationModel>(companySiteApplication);
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteApplicationResponse { ErrorMessage = "Unable to get company site application." };
            }
        }

        public SaveCompanySiteApplicationResponse SaveCompanySiteApplication(SaveCompanySiteApplicationRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveCompanySiteApplicationResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySiteApplication = context.CompanySiteApplicationCrossLinks.FirstOrDefault(c => c.Id == request.CompanySiteApplication.Id);
                    if (companySiteApplication == null)
                    {
                        companySiteApplication = new CompanySiteApplicationCrossLink
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            ApplicationId = request.CompanySiteApplication.ApplicationId,
                            DateCreated = now
                        };
                        context.CompanySiteApplicationCrossLinks.Add(companySiteApplication);
                    }

                    companySiteApplication.AppApiPublicKey = request.CompanySiteApplication.AppApiPublicKey;
                    companySiteApplication.AppApiSecretKey = request.CompanySiteApplication.AppApiSecretKey;
                    companySiteApplication.AppSharedSecret = request.CompanySiteApplication.AppSharedSecret;
                    companySiteApplication.Locations = request.CompanySiteApplication.Locations;
                    companySiteApplication.IsEnabled = request.CompanySiteApplication.IsEnabled;
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveCompanySiteApplicationResponse { ErrorMessage = "Unable to save company stie application." };
            }
        }


        public SaveCompanySiteEmailResponse SaveCompanySiteEmail(SaveCompanySiteEmailRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveCompanySiteEmailResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySiteEmail = context.CompanySiteEmails.FirstOrDefault(c => c.Id == request.CompanySiteEmail.Id);
                    if (companySiteEmail == null)
                    {
                        companySiteEmail = new CompanySiteEmail
                        {
                            Id = Guid.NewGuid(),
                            CompanySiteId = request.CompanySiteId,
                            DateCreated = now
                        };
                        context.CompanySiteEmails.Add(companySiteEmail);
                    }

                    companySiteEmail.Category = request.CompanySiteEmail.Category;
                    companySiteEmail.Name = request.CompanySiteEmail.Name;
                    companySiteEmail.Email = request.CompanySiteEmail.Email;
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveCompanySiteEmailResponse { ErrorMessage = "Unable to save company site email." };
            }
        }


        public GetCompanySiteSettingsResponse GetCompanySiteSettings(GetCompanySiteSettingsRequest request)
        {
            try
            {
                var response = new GetCompanySiteSettingsResponse();
                using (var context = new FlavorFiEntities())
                {
                    var companySiteSettings = context.CompanySiteSettings.AsNoTracking()
                                                     .Where(cs => cs.CompanySiteId == request.CompanySiteId)
                                                     .Select(cs => new CompanySiteSettingModel
                                                     {
                                                         Id = cs.Id,
                                                         Key = cs.Key,
                                                         Value = cs.Value,
                                                         DateCreated = cs.DateCreated
                                                     });

                    response.CompanySiteSettings = companySiteSettings.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteSettingsResponse { ErrorMessage = "Unable to get company site settings." };
            }
        }

        public GetCompanySiteSettingResponse GetCompanySiteSetting(GetCompanySiteSettingRequest request)
        {
            try
            {
                var response = new GetCompanySiteSettingResponse();
                using (var context = new FlavorFiEntities())
                {
                    // TODO: TREY: 4/17/2019 Fix this service call
                    throw new NotImplementedException();

                    //var companySiteSettings = context.CompanySiteSettings.AsNoTracking().FirstOrDefault(cs => cs.Id == request.ComapnySettingId);
                    //if (setting == null) throw new ApplicationException("Unable to find company setting [CompanySiteSettingId: " + request.ComapnySettingId.ToString() + "]");
                    //Mappers.MobMapper.Map(setting, response.CompanySiteSetting);
                    //response.IsSuccess = true;
                    //return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetCompanySiteSettingResponse { ErrorMessage = "Unable to get company site setting." };
            }
        }

        public SaveCompanySiteSettingResponse SaveCompanySiteSetting(SaveCompanySiteSettingRequest request)
        {
            try
            {
                // TODO: TREY: 4/17/2019 Fix this service call
                throw new NotImplementedException();

                //var user = Security.DecryptUserToken(request.UserToken);
                //if (user == null) return new SaveCompanySiteSettingResponse { IsTokenBad = true, ErrorMessage = "Not Authenticated!" };

                //var now = DateTimeConvert.GetTimeZoneDateTimeOffset(TimeZoneInfoId.CentralStandardTime);
                //var response = new SaveCompanySiteSettingResponse();
                //using (var context = new FlavorFiEntities())
                //{
                //    var setting = context.CompanySiteSettings.FirstOrDefault(cs => cs.Id == request.CompanySiteSetting.Id);
                //    if (setting == null)
                //    {
                //        setting = new CompanySiteSetting { Id = Guid.NewGuid(), DateCreated = now };
                //        context.CompanySiteSettings.Add(setting);
                //    }
                //    Mappers.MobMapper.Map(request.CompanySiteSetting, setting);
                //    context.SaveChanges();

                //    response.IsSuccess = true;
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SaveCompanySiteSettingResponse { ErrorMessage = "Unable to save company site setting." };
            }
        }
    }
}