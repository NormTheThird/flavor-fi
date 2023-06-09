using FlavorFi.Common.Commands;
using FlavorFi.ShopWorks.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlavorFi.ShopWorks.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {

        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.SetProperty<string>(ref this.message, value, "Message"); }
        }

        private string partNumber;
        public string PartNumber
        {
            get { return this.partNumber; }
            set { this.SetProperty<string>(ref this.partNumber, value, "PartNumber"); }
        }

        // Commands
        private DelegateCommand<string> applicationCommand;
        public ICommand ApplicationCommand
        {
            get
            {
                if (this.applicationCommand == null)
                    this.applicationCommand = new DelegateCommand<string>(this.ApplicationClick, this.CanApplicationClick);

                return this.applicationCommand;
            }
        }
        private bool CanApplicationClick(string _commandParameter)
        {
            return true;
        }
        private void ApplicationClick(string _commandParameter)
        {
            if (_commandParameter == "EXIT")
                App.Current.Shutdown();
            else if (_commandParameter == "SYNC")
            {
                //Task.Factory.StartNew(() => this.ProductSync()).ContinueWith(t => this.Message = t.Result, TaskScheduler.FromCurrentSynchronizationContext());
                this.CallProductSync();
                this.Message = "Syncing product, please wait ...";
            }
            else if (_commandParameter == "MIN")
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        private async void CallProductSync()
        {
            var result = await ProductSyncAsync();
            this.Message = result;
        }

        private Task<string> ProductSyncAsync()
        {
            return Task.Factory.StartNew(() => this.ProductSync());
        }

        private string ProductSync()
        {
            try
            {           
                var companySiteId = Guid.Parse(ConfigurationManager.AppSettings["CompanySiteId"]);
                new ProductService(companySiteId).SyncProduct(this.PartNumber);
                return this.PartNumber + " was synced successfully.";
            }
            catch (Exception ex)
            {
                return "Unable to sync product. " + ex.Message;
            }
        }
    }
}