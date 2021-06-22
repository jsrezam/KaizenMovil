using Kaizen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignDetailPage : ContentPage
    {
        public ObservableCollection<CampaignDetail> campaignDetail;
        private IEnumerable<CampaignDetail> _campaignDetail;
        public CampaignDetailPage(IEnumerable<CampaignDetail> campaignDetail)
        {
            InitializeComponent();
            _campaignDetail = campaignDetail;
            PopulateCampaignDetail(null);

        }

        private void PopulateCampaignDetail(string filter)
        {
            this.campaignDetail = new ObservableCollection<CampaignDetail>();
            foreach (var item in _campaignDetail)
            {
                campaignDetail.Add(item);
            }
           if (string.IsNullOrEmpty(filter))
            {
                this.detailCampaigList.ItemsSource = this.campaignDetail
                .OrderByDescending(cd => cd.State)
                .ThenBy(cd => cd.Customer.LastName);
            }
            else 
            {
                this.detailCampaigList.ItemsSource = this.campaignDetail
                .Where(dc => dc.Customer.FullName.ToUpper().StartsWith(filter))
                    .OrderByDescending(cd => cd.State)
                .ThenBy(cd => cd.Customer.LastName);
            }
            
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = ((ImageButton)sender).CommandParameter as CampaignDetail;
            PhoneDialer.Open(item.Customer.CellPhone);
        }

        private void detailCampaigList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.detailCampaigList.SelectedItem = null;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.PopulateCampaignDetail(e.NewTextValue.Trim().ToUpper());
        }
    }
}