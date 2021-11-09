using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAdExplorerMobile.Services
{
    public static class AzureAdConstants
    {
        // Azure AD Coordinates
        public static string Tenant = "Enter_the_Tenant_Id_Here";
        public static string ClientID = "Enter_the_Application_Id_Here";

        public static string[] Scopes = { "User.Read" };

        public static string Authority = $"https://login.microsoftonline.com/{Tenant}/";
        public static string IOSKeyChainGroup = "com.microsoft.adalcache";
        public static string AndroidRedirectUri = "msauth://Enter_the_Package_Name/Enter_the_Signature_Hash";
        public static string IOSRedirectUri = "msauth.Enter_the_Bundle_Id_Here://auth";
        public static string UWPRedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";
    }
}
