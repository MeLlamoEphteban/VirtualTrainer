using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Equipment
    {
        public Equipment()
        {
            EquipmentExercises = new HashSet<EquipmentExercise>();
            GroupEquipments = new HashSet<GroupEquipment>();
        }

        public int Idequipment { get; set; }
        public string EquipmentName { get; set; }
        public int Stock { get; set; }
        public string Details { get; set; }

        public virtual ICollection<EquipmentExercise> EquipmentExercises { get; set; }
        public virtual ICollection<GroupEquipment> GroupEquipments { get; set; }
    }
}
