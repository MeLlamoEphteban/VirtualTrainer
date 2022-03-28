using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class WorkProgram
    {
        public WorkProgram()
        {
            ProgramUsers = new HashSet<ProgramUser>();
            WorkTypes = new HashSet<WorkType>();
        }

        public int IdworkProgram { get; set; }
        public string ProgramName { get; set; }

        public virtual ICollection<ProgramUser> ProgramUsers { get; set; }
        public virtual ICollection<WorkType> WorkTypes { get; set; }
    }
}
