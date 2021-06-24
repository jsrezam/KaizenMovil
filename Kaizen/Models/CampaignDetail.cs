using System;

namespace Kaizen.Models
{
    public class CampaignDetail : BaseEntity
    {
        public Customer Customer { get; set; }
        public int TotalCallsNumber { get; set; }

        public string Offer
        {
            get
            {
                return $"Offer: {Id}";
            }
        }

        public string TotalCallsNumberFormat 
        { 
            get 
            {
                return $"Calls: {TotalCallsNumber}";
            } 
        }
        public string LastCallDuration { get; set; }
        public string LastCallDurationFormat
        {
            get
            {
                return $"Duration: {LastCallDuration}";
            }
        }
        public DateTime LastCallDate { get; set; }
        public string LastCallDateFormat
        {
            get
            {
                if (LastCallDate == DateTime.MinValue)
                    return "Date: Not available";
                else
                    return $"Date: {LastCallDate}";

            }
        }
        public string LastValidCallDuration { get; set; }

        public string LastValidCallDurationFormat 
        {
            get 
            {
                return $"Valid: {LastValidCallDuration}";
            }
        }
        public DateTime LastValidCallDate { get; set; }
        public string LastValidCallDateFormat
        {
            get
            {
                if (LastValidCallDate == DateTime.MinValue)
                    return "Valid: Not available";
                else
                    return $"Valid: {LastValidCallDate}";

            }
        }
        public string State { get; set; }
    }
}
