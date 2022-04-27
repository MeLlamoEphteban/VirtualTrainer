using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class User
    {
        public User()
        {
            Invoices = new HashSet<Invoice>();
            PersonalWorkouts = new HashSet<PersonalWorkout>();
            ProgramUsers = new HashSet<ProgramUser>();
            UsersExercises = new HashSet<UsersExercise>();
        }

        public int Iduser { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cnp { get; set; }
        public string UserAspNet { get; set; }

        public virtual AspNetUser UserAspNetNavigation { get; set; }
        public virtual UserSubscription UserSubscription { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PersonalWorkout> PersonalWorkouts { get; set; }
        public virtual ICollection<ProgramUser> ProgramUsers { get; set; }
        public virtual ICollection<UsersExercise> UsersExercises { get; set; }
    }
}
