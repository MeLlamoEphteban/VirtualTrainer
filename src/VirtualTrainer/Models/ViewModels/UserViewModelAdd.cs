using System;
using System.ComponentModel.DataAnnotations;
namespace VirtualTrainer.Models.ViewModels
{
    public class UserViewModelAdd
    {
        public int Idsubscription { get; set; }
        public int Iduser { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Cnp { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        public string Password { get; set; }
    }
}
