using System;

namespace Kaizen.Models
{
    public class CampaignDetail : BaseEntity
    {
        public Customer Customer { get; set; }
        public int TotalCallsNumber { get; set; }

        public string TotalCallsNumberFormat 
        { 
            get 
            {
                return $"Calls: {TotalCallsNumber}";
            } 
        }
        public string LastCallDuration { get; set; }
        public DateTime LastCallDate { get; set; }
        public string LastValidCallDuration { get; set; }

        public string LastValidCallDurationFormat 
        {
            get 
            {
                return $"Duration: {LastValidCallDuration}";
            }
        }
        public DateTime LastValidCallDate { get; set; }
        public string LastValidCallDateFormat
        {
            get
            {
                if (LastValidCallDate == DateTime.MinValue)
                    return "Date: Not available";
                else
                    return $"Date: {LastValidCallDate}";

            }
        }
        public string State { get; set; }
    }
}
