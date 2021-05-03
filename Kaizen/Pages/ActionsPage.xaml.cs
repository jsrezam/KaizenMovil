using Acr.UserDialogs;
using Kaizen.Interfaces;
using Kaizen.Models;
using Kaizen.Services;
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
        private readonly ApiService apiService;
        public ActionsPage()
        {
            InitializeComponent();
            this.apiService = new ApiService();
        }

        [Obsolete]
        private IEnumerable<CallLogModel> LoadCallLog()
        {
            return DependencyService.Get<ICallLog>().GetCallLogs();
        }

        [Obsolete]
        private async void SynchronizeTodayCalls_Tapped(object sender, EventArgs e)
        {
            try
            {
                var statusContact = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                var statusPhone = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
                if (statusContact != PermissionStatus.Granted || statusPhone != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                    {
                        await DisplayAlert(title: APP_NAME,
                            message: "Permissions are required to access contacts",
                            cancel: "OK");
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
                    var callsLog = LoadCallLog();
                    if (callsLog.Count() > 0)
                    {
                        UserDialogs.Instance.ShowLoading(title: "Processing");
                        await apiService.SynchronizeTodayCalls(callsLog);
                        UserDialogs.Instance.HideLoading();

                        await DisplayAlert(title: "Synchronize Today Calls",
                            message: "Synchronization was successful",
                            cancel: "Ok");
                    }
                    else
                    {
                        await DisplayAlert(title: "Synchronize Today Calls",
                            message: "No calls to sync",
                            cancel: "Ok");
                    }
                }
                else if (statusContact != PermissionStatus.Unknown || statusPhone == PermissionStatus.Unknown)
                {
                    await DisplayAlert(title: APP_NAME,
                        message: "Permission was denied. We cannot continue, please try again.",
                        cancel: "OK");
                }
            }
            catch
            {
                await DisplayAlert(title: "Synchronize Today Calls",
                    message: "An unexpected problem has occurred",
                    cancel: "OK");
            }
        }
    }
}