using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class ExercisePost
    {
        public int Idexercise { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
        public string Instructions { get; set; }
    }
}
