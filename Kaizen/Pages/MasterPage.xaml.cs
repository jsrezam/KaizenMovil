using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Obsolete]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
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