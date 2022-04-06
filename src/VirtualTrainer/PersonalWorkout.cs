using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class PersonalWorkout
    {
        public PersonalWorkout()
        {
            ExerciseAssignments = new HashSet<ExerciseAssignment>();
        }

        public int PersWorkoutId { get; set; }
        public int UserId { get; set; }
        public string WorkoutName { get; set; }
        public string BodyGroup { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ExerciseAssignment> ExerciseAssignments { get; set; }
    }
}
