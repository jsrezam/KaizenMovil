using Kaizen.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private async Task LoadCallLog()
        {
            try
            {
                var statusContact = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                var statusPhone = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
                if (statusContact != PermissionStatus.Granted || statusPhone != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                    {
                        await DisplayAlert(APP_NAME, "Permissions are required to access contacts", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts, Permission.Phone);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Contacts))
                        statusContact = results[Permission.Contacts];
                    if (results.ContainsKey(Permission.Phone))
                        statusPhone = results[Permission.Phone];
                }

                if (statusContact == PermissionStatus.Granted && statusPhone == PermissionStatus.Granted)
                {
                    //CallLogList.IsRefreshing = true;

                    var callsLog = DependencyService.Get<ICallLog>().GetCallLogs();
                    
                    //CallLogList.IsRefreshing = false;
                    //CallLogList.ItemsSource = Logg;
                }
                else if (statusContact != PermissionStatus.Unknown || statusPhone == PermissionStatus.Unknown)
                {
                    await DisplayAlert(APP_NAME, "Permission was denied. We cannot continue, please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                //activity_indicator.IsRunning = false;
                //activity_indicator.IsVisible = false;

                await DisplayAlert("Call Log", "A problem has occurred, contact customer support. Technical report: " + ex.Message, "OK");
            }
            finally
            {
                //activity_indicator.IsRunning = false;
                //activity_indicator.IsVisible = false;
            }
        }

        [Obsolete]
        private async void BtnSyncDateCalls_Clicked(object sender, EventArgs e)
        {
            await LoadCallLog();
        }
        
    }
}