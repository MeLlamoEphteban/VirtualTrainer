using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class ExerciseAssignment
    {
        public int Id { get; set; }
        public int PersonalWorkoutId { get; set; }
        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual PersonalWorkout PersonalWorkout { get; set; }
    }
}
