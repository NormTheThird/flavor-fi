using FlavorFi.ShopWorks.Service.Services;
using System;

namespace FlavorFi.ShopWorks.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ProductService().SyncProducts();
                //new ProductService().SyncProduct();
                //new CustomerService().SyncCustomers();

                // Test purposes
                //var baseService = new BaseService();
                //baseService.GetDatabaseSchema("Data_Core", baseService.ShopWorksBaseService.FileMakerCoreConnection);
                //baseService.GetDatabaseSchema("Data_Inventory", baseService.ShopWorksBaseService.FileMakerInventoryConnection);
                //baseService.GetDatabaseSchema("Data_Products", baseService.ShopWorksBaseService.FileMakerProductsConnection);
                //baseService.GetDatabaseSchema("Data_Thumbnails", baseService.ShopWorksBaseService.FileMakerThumbnailsConnection);
                //baseService.GetTableSchema("Ven", baseService.ShopWorksBaseService.FileMakerProductsConnection);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { Console.ReadLine(); }
        }
    }
}