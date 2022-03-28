using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class User
    {
        public User()
        {
            ProgramUsers = new HashSet<ProgramUser>();
            UsersExercises = new HashSet<UsersExercise>();
        }

        public int Iduser { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Cnp { get; set; }

        public virtual UserSubscription UserSubscription { get; set; }
        public virtual ICollection<ProgramUser> ProgramUsers { get; set; }
        public virtual ICollection<UsersExercise> UsersExercises { get; set; }
    }
}
