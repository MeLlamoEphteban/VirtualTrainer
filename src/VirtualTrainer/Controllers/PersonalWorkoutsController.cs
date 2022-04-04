using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index()
        {
            var saliFitnessContext = _context.PersonalWorkouts.Include(p => p.User);
            return View(await saliFitnessContext.ToArrayAsync());
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
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersWorkoutId,UserId,WorkoutName,BodyGroup,Exercises")] PersonalWorkout personalWorkout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalWorkout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Email", personalWorkout.UserId);
            return View(personalWorkout);
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
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Email", personalWorkout.UserId);
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
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Email", personalWorkout.UserId);
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
