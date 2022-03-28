using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class ProgramType
    {
        public ProgramType()
        {
            TypeGroups = new HashSet<TypeGroup>();
            WorkTypes = new HashSet<WorkType>();
        }

        public int IdprogramType { get; set; }
        public string ProgramTypeName { get; set; }

        public virtual ICollection<TypeGroup> TypeGroups { get; set; }
        public virtual ICollection<WorkType> WorkTypes { get; set; }
    }
}
