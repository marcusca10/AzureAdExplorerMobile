using AzureAdExplorerMobile.Services;
using AzureAdExplorerMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureAdExplorerMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<AzureAdAuthenticationService>();
            DependencyService.Register<MockDataStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
