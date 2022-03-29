using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualTrainer;
using VirtualTrainer.Authorization;
using VirtualTrainer.Models.ViewModels;

namespace VirtualTrainer.Controllers
{
    public class UsersController : Controller
    {
        private readonly SaliFitnessContext _context;

        public UsersController(SaliFitnessContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            var isAuthorized = User.IsInRole(Constants.ContactAdministratorsRole);
            if (!isAuthorized)
            {
                return Unauthorized();
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "Surname" ? "Surname_desc" : "Surname";

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            IQueryable<User> users = _context.Users.Include(x => x.UserSubscription.IdsubscriptionNavigation);

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Name.Contains(searchString) || u.Surname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "Surname":
                    users = users.OrderBy(s => s.Surname);
                    break;
                case "Surname_desc":
                    users = users.OrderByDescending(s => s.Surname);
                    break;
                default:
                    users = users.OrderBy(s => s.Email);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound($"The user {id} does not exist!");
            }
            var user = await _context.Users.Where(u => u.Iduser == id)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return NotFound($"The user {id} does not exist in database.");
            }

            var WorkProgramIDs = await _context.ProgramUsers.Where(vp => vp.Iduser == id)
                .ToArrayAsync();
            if (WorkProgramIDs.Length == 0)
            {
                return NotFound($"The user {id} has no work programs!");
            }
            var IDWorkProgram = WorkProgramIDs.Select(it => it.IdworkProgram)
                .ToArray();
            var WorkPrograms = await _context.WorkPrograms.Where(wp => IDWorkProgram.Contains(wp.IdworkProgram))
                .ToArrayAsync();
            var du = new DetailsUser();
            du.user = user;
            du.workPrograms = WorkPrograms;

            return View(du);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            PopulateSubscriptionsDropDownList();
            var x = new UserViewModelAdd();
            return View(x);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Name,Surname,Address,Phone,Cnp,Idsubscription,startDate")] UserViewModelAdd user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userNew = new User();
                    userNew.Email = user.Email;
                    userNew.Name = user.Name;
                    userNew.Surname = user.Surname;
                    userNew.Address = user.Address;
                    userNew.Phone = user.Phone;
                    userNew.Cnp = user.Cnp;
                    _context.Add(userNew);

                    userNew.UserSubscription = new UserSubscription();
                    //userNew.UserSubscription.StartDate = DateTime.Now;
                    userNew.UserSubscription.StartDate = user.startDate;
                    userNew.UserSubscription.EndDate = userNew.UserSubscription.StartDate.AddMonths(1);
                    userNew.UserSubscription.Idsubscription = user.Idsubscription;
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
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Iduser == id);
            if (await TryUpdateModelAsync<User>(
                userToUpdate,
                "",
                u => u.Email, u => u.Name, u => u.Surname, u => u.Address, u => u.Phone, u => u.Cnp))
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
            return View(userToUpdate);
        }

        private void PopulateSubscriptionsDropDownList()
        {
            //var subscriptionQuery = from d in _context.UserSubscriptions
            //                       orderby d.IdsubscriptionNavigation.SubName
            //                       select d;
            var userSub = _context.Subscriptions.ToArray().Select(it => new SelectListItem(it.SubName, it.Idsubscription.ToString()));
            ViewBag.SubscriptionID = userSub.ToArray();
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Iduser == id);
            if (user == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
