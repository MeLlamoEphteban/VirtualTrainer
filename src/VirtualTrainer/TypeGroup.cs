using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class TypeGroup
    {
        public int Id { get; set; }
        public int IdbodyGroup { get; set; }
        public int IdprogramType { get; set; }

        public virtual BodyGroup IdbodyGroupNavigation { get; set; }
        public virtual ProgramType IdprogramTypeNavigation { get; set; }
    }
}
