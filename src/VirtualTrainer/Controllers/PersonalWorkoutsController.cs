using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using VirtualTrainer.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace VirtualTrainer.Controllers
{
    public class PersonalWorkoutsController : Controller
    {
        private readonly SaliFitnessContext _context;

        public PersonalWorkoutsController(SaliFitnessContext context)
        {
            _context = context;
        }

        //GET items
        public async Task<IActionResult> Index([FromServices] UserManager<IdentityUser> _userManager)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var userStart = await _context.Users.Where(u => u.UserAspNet == userId).ToArrayAsync();
            var userFinal = userStart[0].Iduser; //find the exact ID, example 27
            var PersonalWorkoutID = _context.PersonalWorkouts.Where(u => u.UserId == userFinal).ToArray(); //in table, get all entries where id=27
            var IDPersonalWorkout = PersonalWorkoutID.Select(it => it.PersWorkoutId).ToArray(); //in the above var, select all workout ids
            var PersonalWorkouts = await _context.PersonalWorkouts.Where(wp => IDPersonalWorkout.Contains(wp.PersWorkoutId)).ToArrayAsync();

            var indexSelector = new IndexSelector();
            indexSelector.personalWorkouts = PersonalWorkouts;

            return View(indexSelector); //return just the users entries
        }

        //GET details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalWorkout = await _context.PersonalWorkouts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PersWorkoutId == id);
            if (personalWorkout == null)
            {
                return NotFound();
            }

            return View(personalWorkout);
        }

        //GET create
        public IActionResult Create()
        {
            PopulateExercisesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutName, BodyGroup, Exercises")] UserAddsProgram userAddsProgram, [FromServices] UserManager<IdentityUser> _userManager)
        {
            PopulateExercisesList();
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var userStart = await _context.Users.Where(u => u.UserAspNet == userId).ToArrayAsync();
                if(userStart == null)
                {
                    return NotFound("No user found with id");
                }

                var userWorkout = new PersonalWorkout();
                userWorkout.UserId = userStart[0].Iduser;
                userWorkout.WorkoutName = userAddsProgram.WorkoutName;
                userWorkout.BodyGroup = userAddsProgram.BodyGroup;
                userWorkout.Exercises = userAddsProgram.Exercises.ToString();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userAddsProgram);
        }

        private void PopulateExercisesList()
        {
            var allExercises = _context.Exercises;
            var viewModel = new List<ExbodyList>();
            foreach (var exercise in allExercises)
            {
                viewModel.Add(new ExbodyList
                {
                    ExerciseID = exercise.Idexercise,
                    ExerciseName = exercise.ExerciseName
                });
            }
            ViewData["Exercises"] = viewModel;
        }

        // GET: PersonalWorkouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalWorkout = await _context.PersonalWorkouts.FindAsync(id);
            if (personalWorkout == null)
            {
                return NotFound();
            }
            return View(personalWorkout);
        }

        // POST: PersonalWorkouts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersWorkoutId,UserId,WorkoutName,BodyGroup,Exercises")] PersonalWorkout personalWorkout)
        {
            if (id != personalWorkout.PersWorkoutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalWorkout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalWorkoutExists(personalWorkout.PersWorkoutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personalWorkout);
        }

        // GET: PersonalWorkouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalWorkout = await _context.PersonalWorkouts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PersWorkoutId == id);
            if (personalWorkout == null)
            {
                return NotFound();
            }
            return View(personalWorkout);
        }

        // POST: PersonalWorkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalWorkout = await _context.PersonalWorkouts.FindAsync(id);
            _context.PersonalWorkouts.Remove(personalWorkout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalWorkoutExists(int id)
        {
            return _context.PersonalWorkouts.Any(e => e.PersWorkoutId == id);
        }
    }
}
