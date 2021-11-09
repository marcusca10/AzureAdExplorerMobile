using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureAdExplorerMobile.Droid
{
    [Activity]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "Enter_the_Package_Name",
        DataScheme = "msauth",
        DataPath = "/Enter_the_Signature_Hash")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}