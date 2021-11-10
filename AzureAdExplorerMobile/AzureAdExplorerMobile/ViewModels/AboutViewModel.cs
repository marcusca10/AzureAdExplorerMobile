using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AzureAdExplorerMobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        string resultText;

        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-web-browsers"));
            PageAppearingCommand = new Command(OnPageAppearing);
        }

        public ICommand OpenWebCommand { get; }
        public ICommand PageAppearingCommand { get; }

        public string ResultText
        {
            get => resultText;
            set => SetProperty(ref resultText, value);
        }

        private void OnPageAppearing(object obj)
        {
            if (string.IsNullOrEmpty(App.LastExceptionMessage))
            {
                var user = this.AuthService.UserContext;
                this.ResultText = user.IsLoggedOn ? 
                    $"Authenticated user details:\r\n\r\nName: \t{user.Name}\r\nId: \t{user.UserIdentifier}\r\nUPN: \t{user.UserPrincipalName}\r\nEmail:{user.EmailAddress}"
                    : "No authenticated user.";

            }
            else
                this.ResultText = App.LastExceptionMessage;
        }
    }
}