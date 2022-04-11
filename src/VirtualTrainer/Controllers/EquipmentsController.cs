using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualTrainer;
using VirtualTrainer.Models.ViewModels;

namespace VirtualTrainer.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly SaliFitnessContext _context;

        public EquipmentsController(SaliFitnessContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StockSortParm"] = sortOrder == "Stock" ? "stock_desc" : "Stock";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var equipment = from e in _context.Equipment
                           select e;
            if(!String.IsNullOrEmpty(searchString))
            {
                equipment = equipment.Where(e => e.EquipmentName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    equipment = equipment.OrderByDescending(s => s.EquipmentName);
                    break;
                case "Stock":
                    equipment = equipment.OrderBy(s => s.Stock);
                    break;
                case "stock_desc":
                    equipment = equipment.OrderByDescending(s => s.Stock);
                    break;
                default:
                    equipment = equipment.OrderBy(s => s.EquipmentName);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Equipment>.CreateAsync(equipment.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var equipment = await _context.Equipment.Where(e => e.Idequipment == id)
                .FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound($"The equipment {id} does not exist in database!");
            }

            var ExercisesIDs = await _context.EquipmentExercises.Where(ex => ex.Idequipment == id)
                .ToArrayAsync();
            if(ExercisesIDs.Length == 0)
            {
                return NotFound($"The equipment {id} has no exercises created!");
            }

            var IDExercises = ExercisesIDs.Select(ex => ex.Idexercise)
                .ToArray();
            var Exercises = await _context.Exercises.Where(ex => IDExercises.Contains(ex.Idexercise))
                .ToArrayAsync();
            var ee = new EqEx();
            ee.equipment = equipment;
            ee.exercises = Exercises;

            return View(ee);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EquipmentName,Stock")] Equipment equipment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(equipment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var equipmentToUpdate = await _context.Equipment.FirstOrDefaultAsync(s => s.Idequipment == id);
            if (await TryUpdateModelAsync<Equipment>(
                equipmentToUpdate,
                "",
                s => s.EquipmentName, s => s.Stock, s => s.Details))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(equipmentToUpdate);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Idequipment == id);
            if (equipment == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
            
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Idequipment == id);
        }
    }
}
