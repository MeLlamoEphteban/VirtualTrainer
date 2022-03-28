using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class GroupEquipment
    {
        public int Id { get; set; }
        public int IdbodyGroup { get; set; }
        public int Idequipment { get; set; }

        public virtual BodyGroup IdbodyGroupNavigation { get; set; }
        public virtual Equipment IdequipmentNavigation { get; set; }
    }
}
