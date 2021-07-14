using Kaizen.Interfaces;
using Kaizen.Models;
using Kaizen.Services;
using Plugin.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kaizen.Core
{
    public static class Synchronization
    {
        private static IEnumerable<CallLogModel> LoadCallLog()
        {
            return DependencyService.Get<ICallLog>().GetCallLogs();
        }

        public async static Task RequestDevicePermissions()
        {
            await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
            await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
        }

        public async static Task<bool> CheckDevicePermissions()
        {
            var statusContact = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
            var statusPhone = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
            return (statusContact == Plugin.Permissions.Abstractions.PermissionStatus.Granted
                && statusPhone == Plugin.Permissions.Abstractions.PermissionStatus.Granted) ? true : false;            
        }

        public async static Task<SynchronizeResponse> SynchronizeTodayCalls()
        {

            try
            {
                var callsLog = LoadCallLog();
                if (callsLog.Count() > 0)
                {
                    var apiService = new ApiService();
                    await apiService.SynchronizeTodayCalls(callsLog);                   
                    
                    return new SynchronizeResponse { Response = true, Message = "Synchronization was successful" };
                }
                else
                {
                    return new SynchronizeResponse { Response = false, Message = "No calls to sync today" };                    
                }
            }
            catch
            {
                return new SynchronizeResponse { Response = false, Message = "An unexpected problem has occurred" };
            }            
        }        
    }
}
