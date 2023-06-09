using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IStandaloneApplicationService: IBaseService
    {
        GetStandaloneApplicationResponse GetStandaloneApplication(GetStandaloneApplicationRequest request);
        GetStandaloneApplicationResponse GetStandaloneApplicationByName(GetStandaloneApplicationByNameRequest request);
    }

    public class StandaloneApplicationService : BaseService,IStandaloneApplicationService
    {
        public GetStandaloneApplicationResponse GetStandaloneApplication(GetStandaloneApplicationRequest request)
        {
            try
            {
                var response = new GetStandaloneApplicationResponse();
                using (var context = new FlavorFiEntities())
                {

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetStandaloneApplicationResponse { ErrorMessage = "Unable to get companies." };
            }
        }

        public GetStandaloneApplicationResponse GetStandaloneApplicationByName(GetStandaloneApplicationByNameRequest request)
        {
            try
            {
                var response = new GetStandaloneApplicationResponse();
                using (var context = new FlavorFiEntities())
                {

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetStandaloneApplicationResponse { ErrorMessage = "Unable to get companies." };
            }
        }
    }
}
