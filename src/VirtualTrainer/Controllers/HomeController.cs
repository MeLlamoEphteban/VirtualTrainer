using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VirtualTrainer.Models;
using Microsoft.EntityFrameworkCore;
using VirtualTrainer.Data;
using VirtualTrainer.Models.GymViewModels;

namespace VirtualTrainer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SaliFitnessContext _context;

        public HomeController(ILogger<HomeController> logger, SaliFitnessContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> UserSubStats()
        {
            IQueryable<GymGroup> data =
                from user in _context.Users
                group user by user.UserSubscription.StartDate into dateGroup
                //group user by starting date into dateGroup
                select new GymGroup()
                {
                    SubscriptionDate = dateGroup.Key,
                    UserCount = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public async Task<ActionResult> NrUserOnSub()
        {
            IQueryable<NrUserOnSub> data = from user in _context.Users
                                           group user by user.UserSubscription.IdsubscriptionNavigation.SubName into nameGroup
                                           select new NrUserOnSub()
                                           {
                                               subName = nameGroup.Key,
                                               UserCount = nameGroup.Count()
                                           };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}
