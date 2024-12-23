using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using RMS.Models;
using Rotativa.AspNetCore;

namespace RMS.Controllers
{
    public class TasksController : Controller
    {
        private readonly dbRMSContext _context;

        public TasksController(dbRMSContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {  // Check session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Index", "Account");
            }
            var data = await _context.Tasks.Include(t => t.Branch) // Include Branch relationship
                    .Include(t => t.BranchUsers) // Include BranchUsers relationship
                  .Where(t=> t.Active == true)
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
                    .ToListAsync();

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();

            return View(data);
        }



        [HttpGet]
        public async Task<IActionResult> Complete(int? id)
        {
            var taskscomplete = await _context.Tasks.Include(e => e.Branch).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (taskscomplete == null)
            {
                return NotFound();
            }
            return View(taskscomplete);

        }

        [HttpPost]
        public async Task<IActionResult> Complete(Tasks tasksdata)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity from the database
                    var existingdata = await  _context.Tasks.FindAsync(tasksdata.Id);
                    if (existingdata == null)
                    {
                        return NotFound();
                    }

                    // Update only the fields that are included in the bind attribute

                    existingdata.Status = tasksdata.Status;
                    existingdata.Remarks = tasksdata.Remarks;
                    existingdata.Completiondate = DateTime.Now;
                   

                    // Save the changes
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Tasks.Any(e => e.Id == tasksdata.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
         
        }





        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

        

            var tasksdata = await _context.Tasks.Include(t => t.Branch) // Include Branch relationship
                   .Include(t => t.BranchUsers) // Include BranchUsers relationship
                   .Where(t=>t.Id == id)
                   .Select(t => new TasksVM
                   {
                       Id = t.Id,
                       Branch = t.Branch != null ? t.Branch.Name : "", // Handle null Branch
                       Eqptrepair = t.Eqptrepair,
                       Assigned = t.BranchUsers != null ? t.BranchUsers.Name : "", // Handle null BranchUsers
                       Assigndate = t.Assigndate,
                       Problem = t.Problem,
                       Status = t.Status,
                       Ionno = t.Ionno,
                       Remarks = t.Remarks,
                       Completiondate = t.Completiondate
                       
                   })
                   .FirstOrDefaultAsync();

            if (tasksdata == null)
            {
                return NotFound();
            }

            return View(tasksdata);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.BranchUsers = _context.BranchUsers.Where(x => x.Active == true).ToList();
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                tasks.Status = false;
                tasks.Active = true;

                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.BranchUsers = _context.BranchUsers.Where(x => x.Active == true).ToList();
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.Include(e => e.Branch).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tasks tasks)
        {
            if (id != tasks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity from the database
                    var existingTask = await _context.Tasks.FindAsync(id);
                    if (existingTask == null)
                    {
                        return NotFound();
                    }

                    // Update only the fields that are included in the bind attribute

                    existingTask.Branchid = tasks.Branchid;
                    existingTask.Eqptrepair = tasks.Eqptrepair;
                    existingTask.Assignedid = tasks.Assignedid;
                    existingTask.Assigndate = tasks.Assigndate;
                    existingTask.Problem = tasks.Problem;
                    existingTask.Ionno = tasks.Ionno;
                    //existingTask.Status = tasks.Status;

                    // Save the changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Tasks.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }




        //[HttpGet]
        //public IActionResult PrintTasks(int Branchid, bool Status,DateTime Datefrom, DateTime Dateto)
        //{
        //    var query = _context.Tasks
        //                        .Include(e => e.Branch)
                                
        //                        .AsQueryable();

        //    // Filter logic
        //    if (Branchid != 0) query = query.Where(e => e.Branchid == Branchid);
           
        //    if (Status != false) query = query.Where(e => e.Status == Status);
        //    if (Datefrom != DateTime.MinValue && Dateto != DateTime.MinValue)
        //        query = query.Where(e => e.Assigndate.HasValue >= Datefrom && e.Assigndate.HasValue <= Dateto);

        //    var eqptIssues = query.ToList();

        //    //// Fallback to "All" if filters are empty
        //    //string branchName = Branchid == 0 ? "All" : _context.Branch.FirstOrDefault(b => b.Id == Branchid)?.Name ?? "Unknown";
        //    //string eqptName = EqptId == 0 ? "All" : _context.Eqpttype.FirstOrDefault(e => e.Id == EqptId)?.Name ?? "Unknown";
        //    //string statusName = Statusid == 0 ? "All" : _context.Status.FirstOrDefault(e => e.Id == Statusid)?.Name ?? "Unknown";
        //    //string conditionName = Conditionid == 0 ? "All" : _context.Eqptcondition.FirstOrDefault(c => c.Id == Conditionid)?.Condition ?? "Unknown";
        //    //string reportPeriod = Datefrom != DateTime.MinValue && Dateto != DateTime.MinValue
        //    //                      ? $"{Datefrom:dd-MMM-yyyy} to {Dateto:dd-MMM-yyyy}"
        //    //                      : "All Time";
        //    //int Totaleqpt = query.ToList().Count();
        //    //int TotalQty = eqptIssues.Sum(e => e.Qty ?? 0);
        //    //// Map data to the dynamic model
        //    //var model = new
        //    //{
        //    //    ReportTitle = "Equipment Report",
        //    //    BranchName = branchName,
        //    //    EquipmentName = eqptName,
        //    //    StatusName = statusName,
        //    //    Totaleqpt = Totaleqpt,
        //    //    TotalQty = TotalQty,
        //    //    ConditionName = conditionName,
        //    //    ReportPeriod = reportPeriod,
        //    //    EquipmentData = eqptIssues.Select(e => new
        //    //    {
        //    //        e.Id,
        //    //        e.Date,
        //    //        Branch = e.Branch?.Name,
        //    //        Eqptname = e.Eqpttype?.Name,
        //    //        e.Issueto,
        //    //        e.Qty,
        //    //        e.Issuevoucher,
        //    //        Status = e.Status?.Name,
        //    //        Condition = e.Eqptcondition?.Condition,
        //    //        e.Details
        //    //    }).ToList()
        //    //};

        //    ////return View("~/Views/Reports/equipmentreport.cshtml", model);


        //    //return new ViewAsPdf("~/Views/Reports/equipmentreport.cshtml", model)
        //    //{
        //    //    CustomSwitches = "--disable-smart-shrinking",
        //    //    PageSize = Rotativa.AspNetCore.Options.Size.Legal,
        //    //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        //    //};



        //    // return Json(new { url = Url.Action("equipmentreport", "Reports") });
        //}

















        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks != null)
            {
                tasks.Active = false;
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
