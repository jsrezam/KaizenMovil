using Kaizen.Core;
using Kaizen.Enumerations;
using Kaizen.Models;
using Kaizen.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignDetailPage : ContentPage
    {
        public ObservableCollection<CampaignDetail> campaignDetail;
        private int campaignId;
        
        public CampaignDetailPage(int campaignId)
        {
            InitializeComponent();
            this.campaignId = campaignId;            
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            this.searchBar.Text = string.Empty;
            var response = await Synchronization.CheckDevicePermissions();
            if (response) await Synchronization.SynchronizeTodayCalls();
            PopulateCampaignDetail();
        }

        private async void PopulateCampaignDetail(string filter = null)
        {
            this.campaignDetail = new ObservableCollection<CampaignDetail>();
            var apiService = new ApiService();
            var response = (await apiService.GetCampaignDetail(campaignId.ToString())).Items;
            campaignDetail.Clear();

            foreach (var item in response)
            {
                if(!item.State.Equals(CampaignStatus.Earned.ToString()))
                    campaignDetail.Add(item);
            }

            this.searchBar.IsVisible = (campaignDetail.Count > 0) ?  true : false;

            if (!string.IsNullOrEmpty(filter))
            {
                this.detailCampaigList.ItemsSource = this.campaignDetail
                .Where(dc => dc.Customer.FullName.ToUpper().StartsWith(filter))
                .OrderByDescending(cd => cd.State)
                    .ThenBy(cd => cd.Customer.LastName);
                return;                
            }

            this.detailCampaigList.ItemsSource = this.campaignDetail
                .OrderByDescending(cd => cd.State)
                    .ThenBy(cd => cd.Customer.LastName);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = ((ImageButton)sender).CommandParameter as CampaignDetail;
            PhoneDialer.Open(item.Customer.CellPhone);
        }        

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.PopulateCampaignDetail(e.NewTextValue.Trim().ToUpper());
        }               

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                var response = await Synchronization.CheckDevicePermissions();
                if (response) await Synchronization.SynchronizeTodayCalls();
                this.searchBar.Text = string.Empty;
                PopulateCampaignDetail();
            }
            finally
            {
                this.refreshView.IsRefreshing = false;
            }
        }
    }
}