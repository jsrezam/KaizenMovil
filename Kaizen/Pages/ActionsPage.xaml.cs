using Acr.UserDialogs;
using Kaizen.Core;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionsPage : ContentPage
    {
        private const string APP_NAME = "Kaizen";
        
        public ActionsPage()
        {
            InitializeComponent();            
        }        

        [Obsolete]
        private async void SynchronizeTodayCalls_Tapped(object sender, EventArgs e)
        {
            var response = await Synchronization.CheckDevicePermissions();
            if (response)
            {
                UserDialogs.Instance.ShowLoading(title: "Processing");
                var syncResponse = await Synchronization.SynchronizeTodayCalls();
                await DisplayAlert(title: APP_NAME, message: syncResponse.Message,cancel: "OK");
                UserDialogs.Instance.HideLoading();
            }
            else 
            {
                await DisplayAlert(title: APP_NAME,
                        message: "Permission are required to syncronize calls",
                        cancel: "OK");
            }
        }
    }
}