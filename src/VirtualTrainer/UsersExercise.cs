using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class UsersExercise
    {
        public int Id { get; set; }
        public int Iduser { get; set; }
        public int Idexercise { get; set; }

        public virtual Exercise IdexerciseNavigation { get; set; }
        public virtual User IduserNavigation { get; set; }
    }
}
