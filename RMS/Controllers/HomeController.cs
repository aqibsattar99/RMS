using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Models;
using System.Diagnostics;

namespace RMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly dbRMSContext _context;

        public HomeController(dbRMSContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            // Check session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Index", "Account");
            }

            // Get Values Using Session
            var name = HttpContext.Session.GetString("Name");
            var desig = HttpContext.Session.GetString("Designation");
            ViewData["name"] = name;
            ViewData["desig"] = desig;




            ViewBag.Branches = _context.Branch
                    .Where(x => x.Active == true)
                    .Count();


            ViewBag.Eqpttypes = _context.Eqpttype
                   .Where(x => x.Active == true)
                   .Count();


            ViewBag.PTasks = _context.Tasks
                    .Where(x => x.Status == false)
                   .Count();



            var TaskList =  _context.Tasks.Include(t => t.Branch) // Include Branch relationship
                       .Include(t => t.BranchUsers) // Include BranchUsers relationship
                     .Where(t => t.Active == true && t.Status == false)
                       .Select(t => new TasksVM
                       {
                           Id = t.Id,
                           Branch = t.Branch != null ? t.Branch.Name : "", // Handle null Branch
                           Eqptrepair = t.Eqptrepair,
                           Assigned = t.BranchUsers != null ? t.BranchUsers.Name : "", // Handle null BranchUsers
                           Assigndate = t.Assigndate,
                           Problem = t.Problem,
                           Status = t.Status,
                           Ionno = t.Ionno
                       })
                       .ToList();

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            

            return View(TaskList);

        }

    }
}
