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
        {

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

               

                eqptissue.Active = true;
                _context.Update(eqptissue);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
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
