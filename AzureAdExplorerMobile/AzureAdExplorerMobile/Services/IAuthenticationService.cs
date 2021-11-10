using AzureAdExplorerMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdExplorerMobile.Services
{
    public interface IAuthenticationService
    {
        UserContext UserContext { get; set; }
        bool UseBroker { get; set; }
        Task SignInAsync(bool useWebView = false, bool useIwa = false);
        Task SignOutAsync();

    }
}
