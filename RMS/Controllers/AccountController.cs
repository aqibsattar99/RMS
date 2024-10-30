using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Models;


namespace RMS.Controllers
{
    public class AccountController : Controller
    {

        private readonly dbRMSContext _context;

        public AccountController(dbRMSContext context)
        {
            _context = context;
        }


        // GET: BranchUserController
        public ActionResult Index()
        {  
            return View();
        }


        [HttpPost]

        public ActionResult Index(BranchUsers User)
        {
            if (ModelState.IsValid)
            {
                var UserData = _context.BranchUsers.Include(x=>x.Roles).Where(x => x.Username == User.Username && x.Password == User.Password).FirstOrDefault();

                if(UserData != null)
                {
                    // Set the username in session
                    HttpContext.Session.SetString("Name", UserData.Name);
                    HttpContext.Session.SetString("Designation", UserData.Designation);
                    HttpContext.Session.SetString("Role", UserData.Roles.Role);


                    // Redirect to a protected page or home page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }

            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");

        }

            // GET: BranchUserController/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BranchUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BranchUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BranchUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BranchUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BranchUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BranchUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
