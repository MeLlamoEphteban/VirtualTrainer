using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirtualTrainer;

namespace VirtualTrainer.Controllers
{
    public class GuidesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PushPullLegsV1()
        {
            return View();
        }

        public IActionResult PushPullLegsV2()
        {
            return View();
        }

        public IActionResult OneDayGroup()
        {
            return View();
        }

        public IActionResult CompoundGroup()
        {
            return View();
        }
    }
}
