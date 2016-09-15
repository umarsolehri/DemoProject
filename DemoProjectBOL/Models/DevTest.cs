using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectBOL.Models
{
    public class DevTest
    {
        public int ID { get; set; }
        [StringLength(maximumLength: 255)]
        public string CampaignName { get; set; }
        public DateTime Date { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Impressions { get; set; }
        public string AffiliateName { get; set; }
    }
}
