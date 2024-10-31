using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RMS.Models;

namespace RMS.Controllers
{
    public class EqptissueController : Controller
    {
        private readonly dbRMSContext _context;
        public EqptissueController(dbRMSContext context)
        {
           
            _context = context;
        }

        // GET: Eqptissue
        public async Task<IActionResult> Index()
        {  // Check session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpttype = _context.Eqpttype.Where(x => x.Active == true).ToList();

            var data = await _context.Eqptissue
                 .Include(e => e.Branch)
                .Include(e => e.Eqpttype)
                .Where(e => e.Active == true)
                .Select(e => new EqptissueVM
                {
                    Id = e.Id,
                    Date = e.Date,
                    DateFormat = e.Date.ToString(),
                    Branch = e.Branch.Name,
                    Eqpttypename = e.Eqpttype.Name,
                    Eqptname = e.Eqptname,
                    Qty = e.Qty,
                    Issueto = e.Issueto,
                    Issuevoucher = e.Issuevoucher,
                    Condition = e.Eqptcondition.Condition,
                    Details = e.Details,
                    Active = e.Active
                })
                .ToListAsync();

            return View(data);
        }


        // GET: Eqptissue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptissue = await _context.Eqptissue
          .Include(e => e.Branch)
                .Include(e => e.Eqpttype)
                .Where(e => e.Active == true)
                .Select(e => new EqptissueVM
                {
                    Id = e.Id,
                    Date = e.Date,
                    Branch = e.Branch.Name,
                    Eqpttypename = e.Eqpttype.Name,
                    Eqptname = e.Eqptname,
                    Qty = e.Qty,
                    Issueto = e.Issueto,
                    Issuevoucher = e.Issuevoucher,
                    Condition = e.Eqptcondition.Condition,
                    Details = e.Details,
                    Active = e.Active
                })
          
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptissue == null)
            {
                return NotFound();
            }

            return View(eqptissue);
        }


        // GET: Eqptissue/Details/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

          //  var eqptissue = await _context.Eqptissue
          //.Include(e => e.Branch)
          //      .Include(e => e.Eqpttype)
          //      .Where(e => e.Active == true)
          //      .Select(e => new EqptissueVM
          //      {
          //          Id = e.Id,
          //          Date = e.Date,
          //          Branch = e.Branch.Name,
          //          Eqpttypename = e.Eqpttype.Name,
          //          Eqptname = e.Eqptname,
          //          Qty = e.Qty,
          //          Issueto = e.Issueto,
          //          Issuevoucher = e.Issuevoucher,
          //          Condition = e.Eqptcondition.Condition,
          //          Details = e.Details,
          //          Active = e.Active
          //      })

          //      .FirstOrDefaultAsync(m => m.Id == id);


            var eqptissues = await _context.Eqptissuehistory
   
    .Where(e=>e.issueid == id) // Filter by issueid and active status
    .ToListAsync(); // Get all matching records as a list




            if (eqptissues == null)
            {
                return NotFound();
            }

            return View(eqptissues);
        }




        // GET: Eqptissue/Create
        public IActionResult Create()
        {
            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();

       
            return View();
        }

      



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Eqptissue eqptissue)
        {
            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();

            if (ModelState.IsValid)
            {

                    eqptissue.Active = true;
                    _context.Add(eqptissue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
              
            }

            // If we reach here, something failed; redisplay the form
            return View(eqptissue);
        }


        // GET: Eqptissue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();

            if (id == null)
            {
                return NotFound();
            }

            // Include the related Eqptname entity
            var Eqptissue = await _context.Eqptissue                            
                                            .Include(e => e.Eqpttype)
                                            .Include(e => e.Branch)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (Eqptissue == null)
            {
                return NotFound();
            }

            // Map the entity to the ViewModel
            var eqptissuevm = new EqptissueVM
            {
                Id = Eqptissue.Id,
                Date = Eqptissue.Date,
                Branch = Eqptissue.Branch?.Name,
                Eqptname = Eqptissue.Eqpttype?.Name, // Assuming Eqptname has a property 'Name'
                Qty = Eqptissue.Qty,
                Details = Eqptissue.Details
            };

            return View(Eqptissue);
        }


   


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Eqptissue eqptissue)
        {
           
                if (ModelState.IsValid)
                {
                // Retrieve existing data from the database before updating
                var existingEqptissue = await _context.Eqptissue
                                                        .Include(e => e.Eqptcondition)
                                                        .Include(e => e.Branch)
                                                        .Include(e => e.Eqpttype)
                                                        .FirstOrDefaultAsync(e => e.Id == eqptissue.Id);

                if (existingEqptissue != null)
                    {
                    // Create a new instance of EqptissueHistory and copy data from existingEqptissue
                    var eqptissueHistory = new Eqptissuehistory
                    {
                        issueid = existingEqptissue.Id,
                        Date = existingEqptissue.Date,

                        Eqpttype = existingEqptissue.Eqpttype?.Name, // Branch might be null, so using ?. to safely access Name
                        Branch = existingEqptissue.Branch?.Name, // Branch might be null, so using ?. to safely access Name
                        Eqptname = existingEqptissue.Eqptname,
                        Qty = existingEqptissue.Qty,
                        Issueto = existingEqptissue.Issueto,
                        Condition = existingEqptissue.Eqptcondition?.Condition, // Check Eqptcondition is not null before accessing Condition
                        Issuevoucher = existingEqptissue.Issuevoucher,
                        Details = existingEqptissue.Details,
                        updatedon = DateTime.UtcNow,
                    };

                    // Add the history record to the EqptissueHistory table
                    _context.Eqptissuehistory.Add(eqptissueHistory);


                    // Update fields in the original eqptissue record with new data
                    existingEqptissue.Date = eqptissue.Date;
                    existingEqptissue.Branchid = eqptissue.Branchid;
                    existingEqptissue.EqptId = eqptissue.EqptId;
                    existingEqptissue.Eqptname = eqptissue.Eqptname;
                    existingEqptissue.Qty = eqptissue.Qty;
                    existingEqptissue.Issueto = eqptissue.Issueto;
                    existingEqptissue.Issuevoucher = eqptissue.Issuevoucher;
                    existingEqptissue.Details = eqptissue.Details;
                    existingEqptissue.Active = true; // Set Active status to true
                    existingEqptissue.Conditionid = eqptissue.Conditionid; // Set Active status to true


                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));


                    }
                }

                return View(eqptissue);
         




        }





        // GET: Eqptissue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptissue = await _context.Eqptissue
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptissue == null)
            {
                return NotFound();
            }

            return View(eqptissue);
        }

        // POST: Eqptissue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eqptissue = await _context.Eqptissue.FindAsync(id);
            if (eqptissue != null)
            {
                _context.Eqptissue.Remove(eqptissue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EqptissueExists(int id)
        {
            return _context.Eqptissue.Any(e => e.Id == id);
        }
    }
}
