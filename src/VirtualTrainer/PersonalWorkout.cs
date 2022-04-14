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
        public int BodyGroupId { get; set; }
        public int WorkProgramId { get; set; }
        public int ProgramTypeId { get; set; }
        public string Bgname { get; set; }
        public string Wpname { get; set; }
        public string Ptname { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ExerciseAssignment> ExerciseAssignments { get; set; }
    }
}
