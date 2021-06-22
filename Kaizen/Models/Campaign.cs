using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kaizen.Models
{
    public class Campaign: BaseEntity
    {
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Progress { get; set; }

        public string ProgressFormat {
            get 
            {
                return $"{Progress}%";
            }
        }

        public decimal BarProgress
        {
            get
            {
                return Progress/100;
            }
        }

        public ICollection<CampaignDetail> CampaignDetails { get; set; }

        public Campaign()
        {
            this.CampaignDetails = new Collection<CampaignDetail>();
        }
    }
}
