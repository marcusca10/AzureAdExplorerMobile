using AzureAdExplorerMobile.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AzureAdExplorerMobile.Services
{
    public class AzureAdAuthenticationService: IAuthenticationService
    {
        private readonly IPublicClientApplication _pca;

        public AzureAdAuthenticationService()
        {
            // Initialize User context
            UserContext = new UserContext()
            {
                IsLoggedOn = false
            };

            // default redirectURI; each platform specific project will have to override it with its own
            var builder = PublicClientApplicationBuilder.Create(AzureAdConstants.ClientID)
                .WithAuthority(AzureAdConstants.Authority)
                .WithIosKeychainSecurityGroup(AzureAdConstants.IOSKeyChainGroup)
                .WithRedirectUri(this.RedirectUri);

            // Android implementation is based on https://github.com/jamesmontemagno/CurrentActivityPlugin
            // iOS implementation would require to expose the current ViewControler - not currently implemented as it is not required
            // UWP does not require this
            var windowLocatorService = DependencyService.Get<IParentWindowLocatorService>();

            if (windowLocatorService != null)
            {
                builder = builder.WithParentActivityOrWindow(() => windowLocatorService?.GetCurrentParentWindow());
            }

            _pca = builder.Build();
        }

        public UserContext UserContext { get; set; }

        public async Task SignInAsync()
        {
            UserContext newContext;
            try
            {
                // acquire token silent
                newContext = await AcquireTokenSilent();
            }
            catch (MsalUiRequiredException)
            {
                // acquire token interactive
                newContext = await SignInInteractively();
            }
            
            UserContext = newContext;
        }

        private async Task<UserContext> AcquireTokenSilent()
        {
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
            AuthenticationResult authResult = await _pca.AcquireTokenSilent(AzureAdConstants.Scopes, accounts.FirstOrDefault()).ExecuteAsync();

            var newContext = UpdateUserInfo(authResult);
            return newContext;
        }

        private async Task<UserContext> SignInInteractively()
        {
            AuthenticationResult authResult = await _pca.AcquireTokenInteractive(AzureAdConstants.Scopes).ExecuteAsync();

            var newContext = UpdateUserInfo(authResult);
            return newContext;
        }

        public async Task SignOutAsync()
        {

            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await _pca.RemoveAsync(accounts.FirstOrDefault());
                accounts = await _pca.GetAccountsAsync();
            }

            // Clear our access token from secure storage.
            SecureStorage.Remove("AccessToken");

            var signedOutContext = new UserContext();
            signedOutContext.IsLoggedOn = false;
            
            UserContext = signedOutContext;
        }

        private string RedirectUri
        {
            get
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    return AzureAdConstants.AndroidRedirectUri;
                else if (DeviceInfo.Platform == DevicePlatform.iOS)
                    return AzureAdConstants.IOSRedirectUri;
                else if (DeviceInfo.Platform == DevicePlatform.UWP)
                    return AzureAdConstants.UWPRedirectUri;

                return string.Empty;
            }
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        private UserContext UpdateUserInfo(AuthenticationResult ar)
        {
            var newContext = new UserContext();
            newContext.IsLoggedOn = false;
            JObject user = ParseIdToken(ar.IdToken);

            newContext.AccessToken = ar.AccessToken;
            newContext.Name = user["name"]?.ToString();
            newContext.UserIdentifier = user["oid"]?.ToString();

            newContext.UserPrincipalName = user["preferred_username"]?.ToString();

            var emails = user["emails"] as JArray;
            if (emails != null)
            {
                newContext.EmailAddress = emails[0].ToString();
            }
            newContext.IsLoggedOn = true;

            return newContext;
        }

        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }
    }
}
