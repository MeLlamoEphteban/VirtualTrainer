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
            PopulateDropDownLists();
            var persWorkout = new PersonalWorkout();
            persWorkout.ExerciseAssignments = new List<ExerciseAssignment>();
            PopulateAssignedExerciseData(persWorkout);
            return View(persWorkout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersWorkoutId,UserId,WorkoutName,BodyGroupId,WorkProgramId,ProgramTypeId,Bgname,Ptname,Wpname")] PersonalWorkout personalWorkout, [FromServices] UserManager<IdentityUser> _userManager, string[] selectedExercises)
        {
            if(selectedExercises != null)
            {
                personalWorkout.ExerciseAssignments = new List<ExerciseAssignment>();
                foreach(var ex in selectedExercises)
                {
                    var exerciseToAdd = new ExerciseAssignment { PersonalWorkoutId = personalWorkout.PersWorkoutId, ExerciseId = int.Parse(ex) };
                    personalWorkout.ExerciseAssignments.Add(exerciseToAdd);
                }
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var userStart = await _context.Users.Where(u => u.UserAspNet == userId).ToArrayAsync();
            personalWorkout.UserId = userStart[0].Iduser;

            var bgID = personalWorkout.BodyGroupId;
            var bgName = await _context.BodyGroups.Where(i => i.IdbodyGroup == bgID).FirstOrDefaultAsync();
            personalWorkout.Bgname = bgName.GroupName;

            var wpID = personalWorkout.WorkProgramId;
            var wpName = await _context.WorkPrograms.Where(i => i.IdworkProgram == wpID).FirstOrDefaultAsync();
            personalWorkout.Wpname = wpName.ProgramName;

            var ptID = personalWorkout.ProgramTypeId;
            var ptName = await _context.ProgramTypes.Where(i => i.IdprogramType == ptID).FirstOrDefaultAsync();
            personalWorkout.Ptname = ptName.ProgramTypeName;

            //PopulateExercisesList();
            if (ModelState.IsValid)
            {
                _context.Add(personalWorkout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists();
            PopulateAssignedExerciseData(personalWorkout);
            return View(personalWorkout);
        }

        private void PopulateDropDownLists()
        {
            var wPrograms = _context.WorkPrograms.ToArray().Select(it => new SelectListItem(it.ProgramName, it.IdworkProgram.ToString()));
            ViewBag.WorkPrograms = wPrograms.ToArray();
            var tPrograms = _context.ProgramTypes.ToArray().Select(it => new SelectListItem(it.ProgramTypeName, it.IdprogramType.ToString()));
            ViewBag.ProgramTypes = tPrograms.ToArray();
            var bGroup = _context.BodyGroups.ToArray().Select(it => new SelectListItem(it.GroupName, it.IdbodyGroup.ToString()));
            ViewBag.BodyGroups = bGroup.ToArray();
        }

        private void PopulateAssignedExerciseData(PersonalWorkout persWorkout)
        {
            var allExercises = _context.Exercises.ToArray();
            var workoutExercises = new HashSet<int>(persWorkout.ExerciseAssignments.Select(it => it.ExerciseId).ToArray());
            var viewModel = new List<UserAddsProgram>();

            foreach (var exercise in allExercises)
    	{
                viewModel.Add(new UserAddsProgram
                {
                    ExerciseID = exercise.Idexercise,
                    Name = exercise.ExerciseName,
                    Assigned = workoutExercises.Contains(exercise.Idexercise)
                });
            }
            ViewData["Exercises"] = viewModel;
        }

        private void UpdateWorkoutExercises(string[] selectedExercises, PersonalWorkout workoutToUpdate)
        {
            if (selectedExercises == null)
            {
                workoutToUpdate.ExerciseAssignments = new List<ExerciseAssignment>();
                return;
            }

            var selectedExercisesHS = new HashSet<string>(selectedExercises);
            var workoutExercises = new HashSet<int>(workoutToUpdate.ExerciseAssignments.Select(c => c.ExerciseId).ToArray());
        }

        private void DeleteExAssignments (PersonalWorkout workoutToUpdate)
        {
            var exercises = _context.ExerciseAssignments.Where(it => it.PersonalWorkoutId == workoutToUpdate.PersWorkoutId).ToArray();
            foreach (var exercise in exercises)
            {
                _context.ExerciseAssignments.Remove(exercise);
            }
            _context.SaveChanges();
        }

        // GET: PersonalWorkouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalWorkout = await _context.PersonalWorkouts.Include(it => it.ExerciseAssignments).Where(it => it.PersWorkoutId == id).FirstOrDefaultAsync();
            if (personalWorkout == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            PopulateAssignedExerciseData(personalWorkout);
            return View(personalWorkout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedExercises)
        {
            if(id == null)
            {
                return NotFound();
            }

            var workoutToUpdate = await _context.PersonalWorkouts
                .FirstOrDefaultAsync(m => m.PersWorkoutId == id);

            DeleteExAssignments(workoutToUpdate);

            if(await TryUpdateModelAsync<PersonalWorkout>(workoutToUpdate, "", i => i.WorkoutName, i => i.BodyGroupId, i => i.WorkProgramId, i => i.ProgramTypeId, i => i.Bgname, i => i.Wpname, i => i.Ptname))
            {
                try
                {
                    foreach (var item in selectedExercises)
                    {
                        var idExercise = int.Parse(item);
                        var exAssignment = new ExerciseAssignment();
                        exAssignment.ExerciseId = idExercise;
                        exAssignment.PersonalWorkoutId = workoutToUpdate.PersWorkoutId;
                        _context.ExerciseAssignments.Add(exAssignment);
                    }
                    var bgID = workoutToUpdate.BodyGroupId;
                    var bgName = await _context.BodyGroups.Where(i => i.IdbodyGroup == bgID).FirstOrDefaultAsync();
                    workoutToUpdate.Bgname = bgName.GroupName;

                    var wpID = workoutToUpdate.WorkProgramId;
                    var wpName = await _context.WorkPrograms.Where(i => i.IdworkProgram == wpID).FirstOrDefaultAsync();
                    workoutToUpdate.Wpname = wpName.ProgramName;

                    var ptID = workoutToUpdate.ProgramTypeId;
                    var ptName = await _context.ProgramTypes.Where(i => i.IdprogramType == ptID).FirstOrDefaultAsync();
                    workoutToUpdate.Ptname = ptName.ProgramTypeName;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists();
            PopulateAssignedExerciseData(workoutToUpdate);
            return View(workoutToUpdate);
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
