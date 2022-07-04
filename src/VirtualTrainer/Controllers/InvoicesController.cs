using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace VirtualTrainer.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly SaliFitnessContext _context;
        public InvoicesController(SaliFitnessContext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            var saliFitnessContext = _context.Invoices;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "Date" ? "Date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            IQueryable<Invoice> invoices = _context.Invoices;
            if (!String.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(u => u.UserName.Contains(searchString));
                if(invoices.Count() == 0)
                {
                    DateTime dDate;
                    if(DateTime.TryParse(searchString, out dDate))
                    {
                        String.Format("{0:yyyy-MM-dd}", dDate);
                        //parse the string to datetime then search DB for proper data
                        var parsedDate = DateTime.Parse(searchString);
                        invoices = _context.Invoices.Where(u => u.IssuedDate.Equals(parsedDate));

                        //total value for the selected day
                        var inv = _context.Invoices.Where(u => u.IssuedDate.Equals(parsedDate)).ToArray();
                        var totalAmount = 0;
                        foreach (Invoice invoice in inv)
                        {
                            totalAmount += Int32.Parse(invoice.Value);
                        }
                        ViewBag.TotalAmount = totalAmount;
                    }
                    else
                    {
                        ViewBag.DateMessage = "Incorrect date format. Try: year-month-day";
                    }
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    invoices = invoices.OrderByDescending(s => s.UserName);
                    break;
                case "Date":
                    invoices = invoices.OrderBy(s => s.IssuedDate);
                    break;
                case "Date_desc":
                    invoices = invoices.OrderByDescending(s => s.IssuedDate);
                    break;
                default:
                    invoices = invoices.OrderBy(s => s.UserName);
                    break;
            }
            int pageSize = 10;

            return View(await PaginatedList<Invoice>.CreateAsync(invoices.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(it => it.IdUserNavigation)
                .Include(it => it.IdSubscriptionNavigation)
                .FirstOrDefaultAsync(m => m.IdInvoice == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
    }
}
