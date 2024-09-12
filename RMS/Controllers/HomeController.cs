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

            ViewBag.Branches = _context.Branch.Where(x => x.active == true).Count();
            ViewBag.Eqpt = _context.Eqptissue
                     .Where(x => x.active == true)
                     .Sum(x => x.qty);

            ViewBag.Eqptstore = _context.Eqptstore
                     .Where(x => x.active == true)
                     .Sum(x => x.qty);


            return View();
        }

    }
}
