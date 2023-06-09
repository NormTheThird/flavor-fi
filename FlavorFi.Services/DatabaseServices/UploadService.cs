using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Data;
using System;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IUploadService
    {
        GetLatestsUploadDateResponse GetLatestsUploadDate(GetLatestsUploadDateRequest request);
    }

    public class UploadService : BaseService, IUploadService
    {
        public GetLatestsUploadDateResponse GetLatestsUploadDate(GetLatestsUploadDateRequest request)
        {
            try
            {
                var response = new GetLatestsUploadDateResponse();
                using (var context = new FlavorFiEntities())
                {
                    //TODO: TREY: 1/3/2018 Get real data from stored proc
                    response.LatestUploadDate = new DateTimeOffset(new DateTime(2018,12,25), new TimeSpan());
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetLatestsUploadDateResponse { ErrorMessage = "unable to get latest upload date" };
            }
        }
    }
}