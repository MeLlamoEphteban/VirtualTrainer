using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class BodyGroup
    {
        public BodyGroup()
        {
            GroupEquipments = new HashSet<GroupEquipment>();
            TypeGroups = new HashSet<TypeGroup>();
        }

        public int IdbodyGroup { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<GroupEquipment> GroupEquipments { get; set; }
        public virtual ICollection<TypeGroup> TypeGroups { get; set; }
    }
}
