using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Exercise
    {
        public Exercise()
        {
            EquipmentExercises = new HashSet<EquipmentExercise>();
            ExerciseAssignments = new HashSet<ExerciseAssignment>();
            UsersExercises = new HashSet<UsersExercise>();
        }

        public int Idexercise { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
        public string Instructions { get; set; }

        public virtual ICollection<EquipmentExercise> EquipmentExercises { get; set; }
        public virtual ICollection<ExerciseAssignment> ExerciseAssignments { get; set; }
        public virtual ICollection<UsersExercise> UsersExercises { get; set; }
    }
}
