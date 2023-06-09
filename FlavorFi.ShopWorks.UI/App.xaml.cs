using FlavorFi.ShopWorks.UI.ViewModels;
using System.Windows;

namespace FlavorFi.ShopWorks.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainViewModel MainViewModel { get; set; }
        public new MainWindow MainWindow { get; set; }

        /// <summary>
        ///     Overides the applications OnStartup method
        /// </summary>
        /// <param name="e">The StartupEventArgs being passed in</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Declare a new properties
            this.MainWindow = new MainWindow();
            this.MainViewModel = new MainViewModel();

            // Set MainWindow data context to MainViewModel
            this.MainWindow.DataContext = this.MainViewModel;
            this.MainWindow.Show();
        }
    }
}