using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAdExplorerMobile.Models
{
    public class UserContext
    {
        public string Name { get; internal set; }
        public string UserIdentifier { get; internal set; }
        public bool IsLoggedOn { get; internal set; }
        public string UserPrincipalName { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string AccessToken { get; internal set; }
    }
}
