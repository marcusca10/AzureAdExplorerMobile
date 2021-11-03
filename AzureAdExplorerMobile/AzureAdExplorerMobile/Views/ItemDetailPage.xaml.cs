using AzureAdExplorerMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AzureAdExplorerMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}