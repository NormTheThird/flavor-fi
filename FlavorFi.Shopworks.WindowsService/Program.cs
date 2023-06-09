using System.ServiceProcess;

namespace FlavorFi.Shopworks.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new ShopworksWindowsService(args) };
            ServiceBase.Run(ServicesToRun);
        }
    }
}