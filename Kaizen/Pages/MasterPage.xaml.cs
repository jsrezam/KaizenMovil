using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Obsolete]
    public partial class MasterPage : MasterDetailPage
    {
        public string Username { get; set; }
        public MasterPage(string username)
        {
            InitializeComponent();
            this.Username = username;
            BindingContext = this;
        }

        private void TapHome_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        private void TapMyCampaigns_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UserCampaignsPage());
            IsPresented = false;
        }
        private void TapActions_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ActionsPage());
            IsPresented = false;
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        
    }
}