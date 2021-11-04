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
        Task SignInAsync();
        Task SignOutAsync();

    }
}
