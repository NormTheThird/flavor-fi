using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;
using System.Linq;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IModuleService
    {
        GetModulesResponse GetModules(GetModulesRequest request);
        ChangeModuleStatusResponse ChangeModuleStatus(ChangeModuleStatusRequest request);
    }

    public class ModuleService : BaseService, IModuleService
    {
        public GetModulesResponse GetModules(GetModulesRequest request)
        {
            try
            {
                var response = new GetModulesResponse();
                using (var context = new FlavorFiEntities())
                {
                    var modules = context.Modules.AsNoTracking()
                                                  .Select(m => new ModuleModel
                                                  {
                                                      Id = m.Id,
                                                      Name = m.Name,
                                                      DisplayName = m.DisplayName,
                                                      Description = m.Description,
                                                      IsActive = m.IsActive,
                                                      DateCreated = m.DateCreated
                                                  });
                    response.Modules = modules.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetModulesResponse { ErrorMessage = "Unable to get modules." };
            }
        }

        public ChangeModuleStatusResponse ChangeModuleStatus(ChangeModuleStatusRequest request)
        {
            try
            {
                var response = new ChangeModuleStatusResponse();
                using (var context = new FlavorFiEntities())
                {
                    var module = context.Modules.FirstOrDefault(a => a.Id.Equals(request.ModuleId));
                    if (module == null) throw new ApplicationException($"Module does not exist for id {request.ModuleId}");
                    module.IsActive = !module.IsActive;
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new ChangeModuleStatusResponse { ErrorMessage = "Unable to get module." };
            }
        }
    }
}