using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kaizen.Interfaces;
using Kaizen.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Kaizen.Droid.Interfaces.CallLogger))]
namespace Kaizen.Droid.Interfaces
{
    public class CallLogger : ICallLog
    {
        public IEnumerable<CallLogModel> GetCallLogs()
        {
            var phoneContacts = new List<CallLogModel>();
            // filter in desc order limit by no
            string querySorter = String.Format("{0} desc ", CallLog.Calls.Date);
            using (var phones = Android.App.Application.Context.ContentResolver.Query(CallLog.Calls.ContentUri, null, null, null, querySorter))
            {
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string callNumber = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Number));
                            string callDuration = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Duration));
                            long callDate = phones.GetLong(phones.GetColumnIndex(CallLog.Calls.Date));
                            string callName = phones.GetString(phones.GetColumnIndex(CallLog.Calls.CachedName));

                            int callTypeInt = phones.GetInt(phones.GetColumnIndex(CallLog.Calls.Type));
                            string callType = Enum.GetName(typeof(CallType), callTypeInt);

                            var log = new CallLogModel
                            {
                                CallName = callName,
                                CallNumber = callNumber,
                                CallDuration = callDuration,
                                CallDateTick = callDate,
                                CallType = callType
                            };

                            phoneContacts.Add(log);
                        }
                        catch (Exception ex)
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    phones.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
            }

            return OrganizeCalls(phoneContacts);
        }

        public static IEnumerable<CallLogModel> OrganizeCalls(IEnumerable<CallLogModel> calls)
        {
            var todayCallsRepetitions =
                        calls
                        .Where(cl => cl.CallDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) && cl.CallType.Equals("Outgoing"))
                        .GroupBy(cl => cl.CallNumberFormatted, StringComparer.OrdinalIgnoreCase)
                        .Where(cl => cl.Count() >= 1)
                        .Select(cl => new { CallNumberFormatted = cl.Key, TotalCallsNumber = cl.Count() }).ToList();

            var todayCalls =
                        calls.Where(cl => cl.CallDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) && cl.CallType.Equals("Outgoing"))
                        .OrderByDescending(cl => cl.CallDate)
                        .GroupBy(cl => cl.CallNumberFormatted, StringComparer.OrdinalIgnoreCase)
                        .Select(cl => cl.First())
                        .ToList();

            return (from tcr in todayCallsRepetitions
                    join tc in todayCalls on tcr.CallNumberFormatted equals tc.CallNumberFormatted
                    select new CallLogModel
                    {
                        CallDateTick = tc.CallDateTick,
                        CallDuration = tc.CallDuration,
                        CallName = tc.CallName,
                        CallType = tc.CallType,
                        CallNumber = tc.CallNumber,
                        TotalCallsNumber = tcr.TotalCallsNumber
                    }).ToList();
        }
    }
}