using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualTrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class UsersControllerNew : Controller
    {
        private readonly SaliFitnessContext _context;

        public UsersControllerNew(SaliFitnessContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<PaginatedList<User>> GetUsersRaw()
        {
            int? pageNumber = 1;
            int pageSize = 15;
            var users = _context.Users.Include(x => x.UserSubscription.IdsubscriptionNavigation);
            //var users = from e in _context.Users
            //            select e;
            var result = await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize);
            foreach (var user in result)
            {
                user.UserSubscription.IdsubscriptionNavigation.UserSubscriptions = null;
                user.UserSubscription.IduserNavigation = null;
            }
            return result;
        }

        [HttpGet("{id}")]
        public async Task<User> GetUserId(int? id)
        {
            if (id == null) return null;

            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return user;
        }

        [HttpPut]
        public async Task<User> SaveUser([FromBody] UserPost userId)
        {
            if (userId == null) return null;

            var user = await _context.Users.FindAsync(userId.Iduser);
            if (user == null) return null;

            user.Name = userId.Name;
            user.Email = userId.Email;
            user.Phone = userId.Phone;
            user.Surname = userId.Surname;

            _context.SaveChanges();

            return user;
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser(int? id)
        {
            if (id == null) return;

            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DbUpdateException("Selected user couldn't be deleted.");
            }
        }

        [HttpGet]
        public async Task<IQueryable<Subscription>> PopulateSubscriptionList()
        {
            var userSub = from s in _context.Subscriptions
                          select s;
            return userSub;
        }

        [HttpPost]
        public async Task<User> CreateUser([FromBody] UserPost userID, [FromServices] IUserStore<IdentityUser> _userStore, [FromServices] UserManager<IdentityUser> _userManager)
        {
            IUserEmailStore<IdentityUser> _emailStore = GetEmailStore(_userManager, _userStore);
            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, userID.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, userID.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, userID.Password);
            var userId = await _userManager.GetUserIdAsync(user);

            var userNew = new User();
            userNew.Email = userID.Email;
            userNew.Name = userID.Name;
            userNew.Surname = userID.Surname;
            userNew.Phone = userID.Phone;
            userNew.Address = "AdresaDeTest";
            userNew.Cnp = "5739275843295";
            userNew.UserAspNet = userId;
            _context.Add(userNew);

            userNew.UserSubscription = new UserSubscription();
            userNew.UserSubscription.StartDate = DateTime.Now;
            userNew.UserSubscription.EndDate = userNew.UserSubscription.StartDate.AddMonths(1);
            //userNew.UserSubscription.Idsubscription = userNew.Idsubscription;
            userNew.UserSubscription.Idsubscription = 1;

            await _context.SaveChangesAsync();

            var newInv = new Invoice();
            var userS = await _context.Users.Where(u => u.UserAspNet == userId).FirstOrDefaultAsync();
            newInv.IdUser = userS.Iduser;
            newInv.UserName = userNew.Name;
            newInv.IdSubscription = 1;
            var subscriptionId = await _context.Subscriptions.Where(it => it.Idsubscription == 1).FirstOrDefaultAsync();
            newInv.SubName = subscriptionId.SubName;
            newInv.IssuedDate = DateTime.Now;
            newInv.Value = subscriptionId.Price.ToString();
            _context.Add(newInv);

            await _context.SaveChangesAsync();
            userNew.UserSubscription = null;
            userNew.UserAspNetNavigation = null;
            userNew.Invoices = null;
            userNew.PersonalWorkouts = null;
            userNew.ProgramUsers = null;
            userNew.UsersExercises = null;
            return userNew;
        }

        private IUserEmailStore<IdentityUser> GetEmailStore(UserManager<IdentityUser> _userManager, IUserStore<IdentityUser> _userStore)
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
    }
}
