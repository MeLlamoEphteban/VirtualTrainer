using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualTrainer.Models;

namespace VirtualTrainer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SaliFitnessContext context)
        {
            context.Database.EnsureCreated();

            ////Look for users
            //if (context.Exercises.Any())
            //{
            //    return; //DB seed
            //}

            ////exercises

            //var exercises = new Exercise[]
            //{
            //    new Exercise{ExerciseName="Decline Triceps Extension",Sets=3, Reps=12,Weight=5},
            //    new Exercise{ExerciseName="Lying Dumbbell Triceps Extension",Sets=3, Reps=12,Weight=5},
            //    new Exercise{ExerciseName="Band Triceps Extension",Sets=3, Reps=12,Weight=5}
            //};
            //foreach (Exercise exercise in exercises)
            //{
            //    context.Exercises.Add(exercise);
            //}
            //context.SaveChanges();
        }
    }
}
