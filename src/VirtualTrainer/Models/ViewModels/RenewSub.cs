using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualTrainer.Models.ViewModels
{
    public class RenewSub
    {
        public int Iduser { get; set; }
        public int Idsubscription { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }
    }
}
