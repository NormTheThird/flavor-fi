using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Web.Http;

namespace FlavorFi.API.Shopify.Controllers
{
    public class InfoModel
    {
        public string Environment { get; set; }
        public string Database { get; set; }
    }

    [RoutePrefix("api/Information")]
    public class InformationController : ApiController
    {
        [HttpGet]
        [Route("Settings")]
        public IHttpActionResult Settings()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FlavorFiEntities"].ConnectionString;
                var entityConnectionStringBuilder = new EntityConnectionStringBuilder(connectionString);
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(entityConnectionStringBuilder.ProviderConnectionString);
                var info = new InfoModel { Environment = ConfigurationManager.AppSettings["Environment"], Database = sqlConnectionStringBuilder.InitialCatalog };
                return Json(info);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

        }
    }
}