using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VirtualTrainer.Models.GymViewModels
{
    public class GymGroup
    {
        [DataType(DataType.Date)]
        public DateTime? SubscriptionDate { get; set; }
        public int UserCount { get; set; }
    }
}
