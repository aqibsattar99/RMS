using System;
using System.Collections.Generic;
using System.Globalization;
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
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            //{
            //    return RedirectToAction("Index", "Account");
            //}

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

    .Where(e => e.issueid == id) // Filter by issueid and active status
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






        //[HttpPost]
        //public IActionResult Add([FromBody] EquipmentIssueRequest request)
        //{

        //    //public async Task<IActionResult> SaveEquipmentIssues([FromBody] List<EqptissueVM> equipmentIssuesVM)
        //    //{
        //        if (request == null || !request.Any())
        //        {
        //            return BadRequest("No data provided.");
        //        }

        //        for (int i = 0; i < request.Count; i++)
        //        {
        //            var equipment = new EquipmentIssue
        //            {
        //                Date = equipmentIssuesVM[i].Date,
        //                Branch = equipmentIssuesVM[i].Branch,
        //                Eqpttypename = equipmentIssuesVM[i].Eqpttypename,
        //                Eqptname = equipmentIssuesVM[i].Eqptname,
        //                Qty = equipmentIssuesVM[i].Qty,
        //                Issueto = equipmentIssuesVM[i].Issueto,
        //                Condition = equipmentIssuesVM[i].Condition,
        //                Issuevoucher = equipmentIssuesVM[i].Issuevoucher,
        //                Details = equipmentIssuesVM[i].Details,
        //                Active = equipmentIssuesVM[i].Active ?? true // Default to true
        //            };

        //            _context.EquipmentIssues.Add(equipment);
        //        }

        //        // Save all items in the database
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = "Data saved successfully!" });



        //        return Ok(new { message = "Data saved successfully!" });
        //}


   
        public async Task<IActionResult> Add([FromBody] EquipmentIssueRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Invalid or empty data.");
            }

            // Attempt to parse Date once for all items
            DateTime parsedDate;
            if (!DateTime.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return BadRequest("Invalid date format. Please use 'yyyy-MM-dd'.");
            }


            // Loop through each item and save it
            foreach (var item in request.Items)
            {
                var Eqptissues = new Eqptissue // Assuming your entity name
                {
                    Date = parsedDate,               // Static value from parent
                    Issuevoucher = request.Issuevoucher, // Static value from parent

                    Branchid = int.TryParse(item.Branchid, out var branchId) ? branchId : (int?)null,
                    Conditionid = int.TryParse(item.Conditionid, out var conditionId) ? conditionId : (int?)null,
                    EqptId = int.TryParse(item.EqptId, out var eqptId) ? eqptId : (int?)null,

                    Eqptname = item.Eqptname,
                    Qty = int.TryParse(item.Qty, out var qty) ? qty : 0, // Convert to integer
                    Issueto = item.Issueto,
                    Details = item.Details,
                    Active = true // Assuming Active flag
                };

                _context.Eqptissue.Add(Eqptissues);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data saved successfully!" });
        }



        public async Task<IActionResult> PrintEqpt(EquipmrnPrintVM EPV)
        {


            return Ok(new { message = "Data saved successfully!" });
        }

    }


}
