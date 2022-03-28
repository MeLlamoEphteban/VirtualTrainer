using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class WorkType
    {
        public int Id { get; set; }
        public int IdworkProgram { get; set; }
        public int IdprogramType { get; set; }

        public virtual ProgramType IdprogramTypeNavigation { get; set; }
        public virtual WorkProgram IdworkProgramNavigation { get; set; }
    }
}
