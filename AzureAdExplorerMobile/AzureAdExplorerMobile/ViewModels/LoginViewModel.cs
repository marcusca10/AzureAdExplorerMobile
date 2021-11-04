using AzureAdExplorerMobile.Models;
using AzureAdExplorerMobile.Services;
using AzureAdExplorerMobile.Views;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AzureAdExplorerMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string loginText;

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            UpdateSignInState();

            LoginCommand = new Command(OnLoginClicked);
        }

        public string LoginText
        {
            get => loginText;
            set => SetProperty(ref loginText, value);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                if (!this.AuthService.UserContext.IsLoggedOn)
                {
                    await this.AuthService.SignInAsync();
                    UpdateSignInState();
                }
                else
                {
                    await this.AuthService.SignOutAsync();
                    UpdateSignInState();
                }
            }
            catch (Exception ex)
            {
                // Alert if any exception excluding user canceling sign-in dialog
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
                    Debug.WriteLine($"Exception:", ex.ToString(), "Dismiss");
            }


            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        void UpdateSignInState()
        {
            LoginText = AuthService.UserContext.IsLoggedOn ? "Logout" : "Login";
        }
    }
}
