using FlavorFi.ShopWorks.Services;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace FlavorFi.Shopworks.WindowsService
{
    public partial class ShopworksWindowsService : ServiceBase
    {
        public int ProductTimerIntervalInHours { get; private set; }

        public ShopworksWindowsService(string[] args)
        {
            InitializeComponent();
            this.ProductTimerIntervalInHours = 12;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                EventLog.WriteEntry(".NET Runtime", "Starting ShopWorks DataSync", EventLogEntryType.Information, 1000);
                var timer = new Timer { Interval = this.ProductTimerIntervalInHours * 3600000 };
                timer.Elapsed += new ElapsedEventHandler(this.DataSync);
                timer.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(".NET Runtime", ex.Message, EventLogEntryType.Information, 1000);
                throw ex;
            }
        }

        protected override void OnStop()
        {
        }

        public void DataSync(object sender, ElapsedEventArgs args)
        {
            try
            {
                var companySiteId = Guid.Parse("7866C5D7-425F-43A5-A1D5-CAEC13CC10E7");
                EventLog.WriteEntry(".NET Runtime", "Product DataSync Starting for company site " + companySiteId.ToString(), EventLogEntryType.Information, 1000);
                new ProductService(companySiteId).SyncProducts();
                EventLog.WriteEntry(".NET Runtime", "Product DataSync Ended", EventLogEntryType.Information, 1000);
            }
            catch (Exception ex) { EventLog.WriteEntry(".NET Runtime", ex.Message + (ex.InnerException == null ? "" : ex.InnerException.Message), EventLogEntryType.Information, 1000); }
        }
    }
}
