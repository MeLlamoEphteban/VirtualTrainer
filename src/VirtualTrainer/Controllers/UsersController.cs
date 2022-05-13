using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            //var isAuthorized = User.IsInRole(Constants.ContactAdministratorsRole);
            //if (!isAuthorized)
            //{
            //    return Unauthorized();
            //}
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SurnameSortParam"] = sortOrder == "Surname" ? "Surname_desc" : "Surname";

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

            int pageSize = 10;
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
                //return NotFound();($"The user {id} has no work programs!");
                TempData["ErrorMessage"] = $"The user {user.Name} {user.Surname} has no personal workouts created yet!";
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
        public async Task<IActionResult> Create([Bind("Email,Name,Surname,Address,Phone,Cnp,Idsubscription,startDate,Password")] UserViewModelAdd userView, [FromServices] IUserStore<IdentityUser> _userStore, [FromServices] UserManager<IdentityUser> _userManager)
        {
            try
            {
                IUserEmailStore<IdentityUser> _emailStore = GetEmailStore(_userManager, _userStore);
                if (ModelState.IsValid)
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, userView.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, userView.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, userView.Password);
                    var userId = await _userManager.GetUserIdAsync(user);

                    var userNew = new User();
                    userNew.Email = userView.Email;
                    userNew.Name = userView.Name;
                    userNew.Surname = userView.Surname;
                    userNew.Address = userView.Address;
                    userNew.Phone = userView.Phone;
                    userNew.Cnp = userView.Cnp;
                    userNew.UserAspNet = userId;
                    _context.Add(userNew);

                    userNew.UserSubscription = new UserSubscription();
                    //userNew.UserSubscription.StartDate = DateTime.Now;
                    userNew.UserSubscription.StartDate = userView.startDate;
                    userNew.UserSubscription.EndDate = userNew.UserSubscription.StartDate.AddMonths(1);
                    userNew.UserSubscription.Idsubscription = userView.Idsubscription;

                    await _context.SaveChangesAsync();

                    var newInv = new Invoice();
                    var userS = await _context.Users.Where(u => u.UserAspNet == userId).FirstOrDefaultAsync();
                    newInv.IdUser = userS.Iduser;
                    newInv.UserName = userNew.Name;
                    newInv.IdSubscription = userView.Idsubscription;
                    var subscriptionId = await _context.Subscriptions.Where(it => it.Idsubscription == userView.Idsubscription).FirstOrDefaultAsync();
                    newInv.SubName = subscriptionId.SubName;
                    newInv.IssuedDate = DateTime.Now;
                    _context.Add(newInv);

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
            PopulateSubscriptionsDropDownList();
            return View(userView);
        }

        // GET
        public async Task<IActionResult> Renew(int? id)
        {
            PopulateSubscriptionsDropDownList();
            if (id == null)
            {
                return NotFound();
            }
            var x = new RenewSub();
            var user = await _context.Users.Where(it => it.Iduser == id)
                .Include(it => it.UserSubscription).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            x.Iduser = user.Iduser;

            return View(x);
        }

        [HttpPost, ActionName("Renew")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(int? id, RenewSub renewSub)
        {
            //delete old subscription entry
            var oldSub = _context.UserSubscriptions.Where(it => it.Iduser == id).FirstOrDefault();

            _context.UserSubscriptions.Remove(oldSub);
            await _context.SaveChangesAsync();

            //take the new subscription data from form and add the entry
            var newSub = new UserSubscription();
            newSub.Iduser = (int)id;
            newSub.StartDate = renewSub.startDate;
            newSub.EndDate = newSub.StartDate.AddMonths(1);
            newSub.Idsubscription = renewSub.Idsubscription;
            _context.Add(newSub);

            //use the above data to create a new invoice
            var newInv = new Invoice();
            newInv.IdUser = (int)id;
            var userId = await _context.Users.Where(it => it.Iduser == id).FirstOrDefaultAsync();
            newInv.UserName = userId.Name;
            newInv.IdSubscription = renewSub.Idsubscription;
            var subscriptionId = await _context.Subscriptions.Where(it => it.Idsubscription == renewSub.Idsubscription).FirstOrDefaultAsync();
            newInv.SubName = subscriptionId.SubName;
            newInv.IssuedDate = DateTime.Now;
            _context.Add(newInv);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private IUserEmailStore<IdentityUser> GetEmailStore(UserManager<IdentityUser>  _userManager, IUserStore<IdentityUser> _userStore)
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
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
            catch (DbUpdateException /*ex*/ )
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
