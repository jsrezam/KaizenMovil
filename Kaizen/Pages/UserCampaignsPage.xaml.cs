using Kaizen.Core;
using Kaizen.Enumerations;
using Kaizen.Models;
using Kaizen.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            var apiService = new ApiService();
            var queryResult = await apiService.GetAgentValidCampaigns();
            this.campaigns = new ObservableCollection<Campaign>();
            foreach (var item in queryResult.Items)
            {
                campaigns.Add(item);
            }
            this.campaignList.ItemsSource = this.campaigns;
        }              

        private async void CampaignList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var campaign = e.CurrentSelection[0] as Campaign;
            await Synchronization.RequestDevicePermissions();
            await Navigation.PushAsync(new CampaignDetailPage(campaign.Id));
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var campaign = ((Button)sender).CommandParameter as Campaign;
            
            if (campaign.CampaignDetails.Where(cd => cd.State.Equals(CampaignStatus.Uncalled.ToString())).Count() > 0)
            {
                await DisplayAlert(
                    title: "Kaizen",
                    message: "You cannot close the campaign, you still have offers in progress",
                    cancel: "OK");
                return;
            }
            
            var apiService = new ApiService();
            var response = await apiService.CloseCampaign(campaign);
            if (!response)
            {
                await DisplayAlert(
                    title: "Kaizen",
                    message: "Something went wrong closing the campaign !",
                    cancel: "OK");
                return;
            }
            PopulateCampaigns();
        }
    }
}