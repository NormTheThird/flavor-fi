using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FlavorFi.Services.ShopWorksServices
{
    public class ShopWorksBaseService
    {      
        public OdbcConnection FileMakerCoreConnection { get; private set; }
        public OdbcConnection FileMakerCompanyConnection { get; private set; }
        public OdbcConnection FileMakerInventoryConnection { get; private set; }
        public OdbcConnection FileMakerODBCMappingConnection { get; private set; }
        public OdbcConnection FileMakerProductsConnection { get; private set; }
        public OdbcConnection FileMakerThumbnailsConnection { get; private set; }
        public string OnSiteImagePath { get; private set; }

        public ShopWorksBaseService(Guid companyId)
        {
            try
            {
                var companySettingsResponse = CompanyService.GetCompanySettings(new GetCompanySettingsRequest { ComapnyId = companyId });
                if (!companySettingsResponse.IsSuccess) throw new ApplicationException(companySettingsResponse.ErrorMessage);

                var fileMakerConnectionStringSetting = companySettingsResponse.CompanySettings.FirstOrDefault(cs => cs.Key.Equals("FileMakerConnectionString", StringComparison.CurrentCultureIgnoreCase));
                if (fileMakerConnectionStringSetting == null) throw new ApplicationException("Company setting not found [CompanyId: " + companyId.ToString() + "][Key: FileMakerConnectionString]");

                //fileMakerConnectionStringSetting.Value = "Server=24.220.210.194;UID=extro;PWD=extro";       //Testing

                var onSiteImagePathSetting = companySettingsResponse.CompanySettings.FirstOrDefault(cs => cs.Key.Equals("OnSiteImagePath", StringComparison.CurrentCultureIgnoreCase));
                if (onSiteImagePathSetting == null) throw new ApplicationException("Company setting not found [CompanyId: " + companyId.ToString() + "][Key: OnSiteImagePath]");
                this.OnSiteImagePath = onSiteImagePathSetting.Value;

                this.FileMakerCoreConnection = new OdbcConnection("DSN=FileMaker_Data_Core;" + fileMakerConnectionStringSetting.Value);
                this.FileMakerCompanyConnection = new OdbcConnection("DSN=FileMaker_Data_Company;" + fileMakerConnectionStringSetting.Value);
                this.FileMakerInventoryConnection = new OdbcConnection("DSN=FileMaker_Data_Inventory;" + fileMakerConnectionStringSetting.Value);
                this.FileMakerProductsConnection = new OdbcConnection("DSN=FileMaker_Data_Products;" + fileMakerConnectionStringSetting.Value);
                this.FileMakerThumbnailsConnection = new OdbcConnection("DSN=FileMaker_Data_Thumbnails;" + fileMakerConnectionStringSetting.Value);
            }
            catch (Exception ex) { throw ex; }
        }

        public string ConvertImageToBase64String(string name)
        {
            try
            {
                var img = Image.FromFile(OnSiteImagePath + name);
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex) {
                Console.WriteLine("-- ConvertImageToBase64String Error: " + ex.Message);
                return null;
            }
        }
    }
}