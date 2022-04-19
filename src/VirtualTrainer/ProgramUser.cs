using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class ProgramUser
    {
        public int Id { get; set; }
        public int IdworkProgram { get; set; }
        public int Iduser { get; set; }
        public int IdpersWorkout { get; set; }

        public virtual PersonalWorkout IdpersWorkoutNavigation { get; set; }
        public virtual User IduserNavigation { get; set; }
        public virtual WorkProgram IdworkProgramNavigation { get; set; }
    }
}
