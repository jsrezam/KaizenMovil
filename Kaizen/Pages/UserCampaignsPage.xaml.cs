using Acr.UserDialogs;
using Kaizen.Models;
using Kaizen.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCampaignsPage : ContentPage
    {
        public ObservableCollection<Campaign> campaigns;

        public UserCampaignsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateCampaigns();
        }

        private async void PopulateCampaigns() 
        {
            ApiService apiService = new ApiService();
            var queryResult = await apiService.GetAgentValidCampaigns();
            this.campaigns = new ObservableCollection<Campaign>();
            foreach (var item in queryResult.Items)
            {
                campaigns.Add(item);
            }
            this.campaignList.ItemsSource = this.campaigns;
        }

        //private void CampaignList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var campaignDetail = (e.SelectedItem as Campaign).CampaignDetails as IEnumerable<CampaignDetail>;
        //    Navigation.PushAsync(new CampaignDetailPage(campaignDetail));
        //}       

        private void campaignList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var campaignDetail = (e.CurrentSelection[0] as Campaign).CampaignDetails as IEnumerable<CampaignDetail>;
            Navigation.PushAsync(new CampaignDetailPage(campaignDetail));
        }

        private void refreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                PopulateCampaigns();
            }
            finally 
            {
                this.refreshView.IsRefreshing = false;
            }            
        }
    }
}