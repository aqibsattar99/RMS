using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using RMS.Models;
using Rotativa.AspNetCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RMS.Controllers
{
    public class EqptissueController : Controller
    {
        private readonly dbRMSContext _context;
        public EqptissueController(dbRMSContext context)
        {

            _context = context;
        }

        // Index Page
        public async Task<IActionResult> Index()
        {
            //Check session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpttype = _context.Eqpttype.Where(x => x.Active == true).ToList();
            ViewBag.Status = _context.Status.ToList();

            var data = await _context.Eqptissue
                 .Include(e => e.Branch)
                .Include(e => e.Eqpttype)
                .Include(e => e.Status)
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
                    Status = e.Status.Name,
                    Issuevoucher = e.Issuevoucher,
                    Condition = e.Eqptcondition.Condition,
                    Details = e.Details,
                    Active = e.Active
                })
                .ToListAsync();

            return View(data);
        }











        // Single Eqptissue Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptissue = await _context.Eqptissue
          .Include(e => e.Branch)
                .Include(e => e.Eqpttype)
                .Include(e => e.Status)
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
                    Status = e.Status.Name,
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











        // Single Eqptissue History
        public async Task<IActionResult> History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptissues = await _context.Eqptissuehistory
                .Where(e => e.issueid == id) // Filter by issueid and active status
                .ToListAsync(); // Get all matching records as a list

            if (eqptissues == null)
            {
                return NotFound();
            }

            return View(eqptissues);
        }











        // Eqptissue Create Page
        public IActionResult Create()
        {
            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();
            ViewBag.Status = _context.Status.ToList();

            return View();
        }

        // Eqptissue Add Data
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
                    Branchid = request.Branchid, // Static value from parent

                    //Branchid = int.TryParse(item.Branchid, out var branchId) ? branchId : (int?)null,
                    Conditionid = int.TryParse(item.Conditionid, out var conditionId) ? conditionId : (int?)null,
                    EqptId = int.TryParse(item.EqptId, out var eqptId) ? eqptId : (int?)null,
                    // 1. Issued
                    // 2. Stocked
                    // 3. Condenmation
                    StatusId = 1,
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











        // Eqptissue Edit Page View Data
        public async Task<IActionResult> Edit(int? id)
        {

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();
            ViewBag.Status = _context.Status.ToList();
            if (id == null)
            {
                return NotFound();
            }

            // Include the related Eqptname entity
            var Eqptissue = await _context.Eqptissue
                                            .Include(e => e.Eqpttype)
                                            .Include(e => e.Branch)
                                            .Include(e => e.Status)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (Eqptissue == null)
            {
                return NotFound();
            }

            return View(Eqptissue);
        }

        // Eqptissue Edit Data Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Eqptissue eqptissue)
        {

            ViewBag.Branch = _context.Branch.Where(x => x.Active == true).ToList();
            ViewBag.Eqptconditions = _context.Eqptcondition.ToList();
            ViewBag.Eqpt = _context.Eqpttype.Where(x => x.Active == true).ToList();
            ViewBag.Status = _context.Status.ToList();

            
                // Retrieve existing data from the database before updating
                var existingEqptissue = await _context.Eqptissue
                                                        .Include(e => e.Eqptcondition)
                                                        .Include(e => e.Branch)
                                                        .Include(e => e.Eqpttype)
                                                        .Include(e => e.Status)
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
                        Status = existingEqptissue.Status?.Name ?? "null",

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
                    existingEqptissue.StatusId = eqptissue.StatusId;
                    existingEqptissue.Active = true; // Set Active status to true
                    existingEqptissue.Conditionid = eqptissue.Conditionid; // Set Active status to true


                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));


                }
           

            return View(eqptissue);





        }







        public async Task<IActionResult> PrintV(string? id) // here id is string and its getting voucher number instead of ID
        {
            var query = _context.Eqptissue
                        .Include(e => e.Eqpttype)
                        .Include(e => e.Branch)
                        .Include(e => e.Eqptcondition)
                        .Include(e => e.Status)
                        .AsQueryable();

            query = query.Where(e => e.Issuevoucher == id);

            var eqptIssues = query.ToList();

            // Extracting relevant details for the header
            var issue = eqptIssues.FirstOrDefault();
            var model = new
            {
                
                IVNo = issue?.Issuevoucher ?? "N/A",
                RVNo = "", // Populate as needed
                IssuingBranch = "IT Dte",
                ReceivingBranch = issue?.Branch.Name ?? "N/A", // Modify as needed
                DateIssue = issue?.Date?.ToString("dd-MMM-yyyy") ?? "N/A",

                DateReceived = "", // Populate as needed
                EquipmentData = eqptIssues.Select(e => new
                {
                    e.Id,
                    e.Date,
                    Branch = e.Branch?.Name,
                    Eqptname = e.Eqpttype?.Name,
                    e.Issueto,
                    e.Qty,
                    e.Issuevoucher,
                    Status = e.Status?.Name,
                    Condition = e.Eqptcondition?.Condition,
                    e.Details
                }).ToList()
            };

            return new ViewAsPdf("~/Views/Reports/Singlevoucher.cshtml", model)
            {
                CustomSwitches = "--disable-smart-shrinking",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };
        }


        // Eqptissue Print Data Submission

        [HttpGet]
        public IActionResult PrintEqpt(int Branchid, int EqptId, int Conditionid,int Statusid, DateTime Datefrom, DateTime Dateto)
        {
            var query = _context.Eqptissue
                                .Include(e => e.Eqpttype)
                                .Include(e => e.Branch)
                                .Include(e => e.Eqptcondition)
                                .Include(e => e.Status)
                                .AsQueryable();

            // Filter logic
            if (Branchid != 0) query = query.Where(e => e.Branchid == Branchid);
            if (EqptId != 0) query = query.Where(e => e.EqptId == EqptId);
            if (Conditionid != 0) query = query.Where(e => e.Conditionid == Conditionid);
            if (Statusid != 0) query = query.Where(e => e.StatusId == Statusid);
            if (Datefrom != DateTime.MinValue && Dateto != DateTime.MinValue)
                query = query.Where(e => e.Date >= Datefrom && e.Date <= Dateto);

            var eqptIssues = query.ToList();

            // Fallback to "All" if filters are empty
            string branchName = Branchid == 0 ? "All" : _context.Branch.FirstOrDefault(b => b.Id == Branchid)?.Name ?? "Unknown";
            string eqptName = EqptId == 0 ? "All" : _context.Eqpttype.FirstOrDefault(e => e.Id == EqptId)?.Name ?? "Unknown";
            string statusName = Statusid == 0 ? "All" : _context.Status.FirstOrDefault(e => e.Id == Statusid)?.Name ?? "Unknown";
            string conditionName = Conditionid == 0 ? "All" : _context.Eqptcondition.FirstOrDefault(c => c.Id == Conditionid)?.Condition ?? "Unknown";
            string reportPeriod = Datefrom != DateTime.MinValue && Dateto != DateTime.MinValue
                                  ? $"{Datefrom:dd-MMM-yyyy} to {Dateto:dd-MMM-yyyy}"
                                  : "All Time";
            int Totaleqpt = query.ToList().Count();
            int TotalQty = eqptIssues.Sum(e => e.Qty ?? 0);
            // Map data to the dynamic model
            var model = new
            {
                ReportTitle = "Equipment Report",
                BranchName = branchName,
                EquipmentName = eqptName,
                StatusName = statusName,
                Totaleqpt = Totaleqpt,
                TotalQty = TotalQty,
                ConditionName = conditionName,
                ReportPeriod = reportPeriod,
                EquipmentData = eqptIssues.Select(e => new
                {
                    e.Id,
                    e.Date,
                    Branch = e.Branch?.Name,
                    Eqptname = e.Eqpttype?.Name,
                    e.Issueto,
                    e.Qty,
                    e.Issuevoucher,
                    Status = e.Status?.Name,
                    Condition = e.Eqptcondition?.Condition,
                    e.Details
                }).ToList()
            };
            
            //return View("~/Views/Reports/equipmentreport.cshtml", model);


            return new ViewAsPdf("~/Views/Reports/Equipmentreport.cshtml", model)
            {
                CustomSwitches = "--disable-smart-shrinking",
                PageSize = Rotativa.AspNetCore.Options.Size.Legal,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };



            // return Json(new { url = Url.Action("equipmentreport", "Reports") });
        }



          // Eqptissue Print Data Submission










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
