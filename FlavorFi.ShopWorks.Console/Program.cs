using FlavorFi.ShopWorks.Services;
using System;

namespace FlavorFi.ShopWorks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var id = Guid.Parse("7866C5D7-425F-43A5-A1D5-CAEC13CC10E7"); // Hauff Sports Company Site Id.

                new ProductService(id).SyncProducts();
                //new ProductService(id).SyncProduct("BQ8759");
                //new CustomerService(id).SyncCustomers();


                // Test purposes    
                //var baseService = new BaseService(id);
                //baseService.GetDatabaseSchema("Data_Core", baseService.ShopWorksBaseService.FileMakerCoreConnection);
                //baseService.GetDatabaseSchema("Data_Company", baseService.ShopWorksBaseService.FileMakerCompanyConnection);
                //baseService.GetDatabaseSchema("Data_Inventory", baseService.ShopWorksBaseService.FileMakerInventoryConnection);
                //baseService.GetDatabaseSchema("Data_ODBCMapping", baseService.ShopWorksBaseService.FileMakerODBCMappingConnection);
                //baseService.GetDatabaseSchema("Data_Products", baseService.ShopWorksBaseService.FileMakerProductsConnection);
                //baseService.GetDatabaseSchema("Data_Thumbnails", baseService.ShopWorksBaseService.FileMakerThumbnailsConnection);
                //baseService.GetTableSchema("Ven", baseService.ShopWorksBaseService.FileMakerProductsConnection);
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); }
            finally { System.Console.ReadLine(); }
        }
    }
}