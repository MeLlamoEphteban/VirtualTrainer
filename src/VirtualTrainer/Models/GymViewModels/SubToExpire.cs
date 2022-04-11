using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualTrainer.Models.GymViewModels
{
    public class SubToExpire
    {
        [DataType(DataType.Date)]
        public DateTime?[] EndDate { get; set; }
        public string[] SubName { get; set; }
        public string[] UserName { get; set; }
    }
}
