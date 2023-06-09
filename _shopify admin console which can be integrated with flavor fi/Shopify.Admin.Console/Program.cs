using Shopify.Admin.Console.Models;
using System;

namespace Shopify.Admin.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fillydev = new FillyFlair("fillyflairdev.myshopify.com", "60480498225b5edd03ad9b4a24489a57");
                System.Console.WriteLine("Connected To Filly Flair development site. " + DateTime.Now.ToLongTimeString());
                fillydev.LoadOnline();

                //var fillyStore = new FillyFlair("filly-flair-inc.myshopify.com", "e89e7cb43c810f8cbe4020d51bc44eaf");
                //System.Console.WriteLine("Connected To Filly Flair store site.");
                //fillyStore.LoadStore();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                    System.Console.WriteLine("InnerMessage: " + ex.InnerException.Message);
            }
            finally
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Data load is now finished.");
                System.Console.ReadKey();
            }         
        }
    }
}
