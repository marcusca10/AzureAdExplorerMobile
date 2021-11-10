using AzureAdExplorerMobile.Models;
using AzureAdExplorerMobile.Services;
using AzureAdExplorerMobile.Views;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AzureAdExplorerMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string loginText;
        int selectedAuthMode;
        bool authModeVisible;
        List<string> authModeList;

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            if (DeviceInfo.Platform == DevicePlatform.UWP)
                authModeList = new List<string>() { "Embedded", "Integrated", "WAM" };
            else
                authModeList = new List<string>() { "System", "Embedded", "Broker" };

            selectedAuthMode = 0;

            UpdateSignInState();

            LoginCommand = new Command(OnLoginClicked);
        }

        public string LoginText
        {
            get => loginText;
            set => SetProperty(ref loginText, value);
        }

        public string SelectedAuthMode
        {
            get => authModeList[selectedAuthMode];
            set => SetProperty(ref selectedAuthMode, authModeList.IndexOf(value));
        }

        public bool AuthModeVisible
        {
            get => authModeVisible;
            set => SetProperty(ref authModeVisible, value);
        }

        public List<string> AuthModeList
        {
            get => authModeList;
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                if (!this.AuthService.UserContext.IsLoggedOn)
                {
                    if (selectedAuthMode == 2)
                        this.AuthService.UseBroker = true;
                    else
                        this.AuthService.UseBroker = false;

                    if (selectedAuthMode == 1)
                        await this.AuthService.SignInAsync(true);
                    else
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
                    Debug.WriteLine($"Exception: {ex.ToString()}");
            }


            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        void UpdateSignInState()
        {
            LoginText = AuthService.UserContext.IsLoggedOn ? "Logout" : "Login";
            AuthModeVisible = AuthService.UserContext.IsLoggedOn ? false : true;
        }
    }
}
