using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class ProgramExercise
    {
        public int IdworkProgram { get; set; }
        public int Idexercise { get; set; }
        public int Id { get; set; }

        public virtual Exercise IdexerciseNavigation { get; set; }
    }
}
