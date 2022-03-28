using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class EquipmentExercise
    {
        public int Id { get; set; }
        public int Idequipment { get; set; }
        public int Idexercise { get; set; }

        public virtual Equipment IdequipmentNavigation { get; set; }
        public virtual Exercise IdexerciseNavigation { get; set; }
    }
}
