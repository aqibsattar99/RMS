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
            var data = await _context.Eqptissue
                 .Include(e => e.Branch)
                .Include(e => e.Eqptname)
                .Where(e => e.active == true)
                .Select(e => new EqptissueVM
                {
                    id = e.Id,
                    date = e.date,
                    branch = e.Branch.name,
                    eqpt = e.Eqptname.name,
                    qty = e.qty,
                    remarks = e.remarks,
                    active = e.active
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
            ViewBag.Branch = _context.Branch.Where(x => x.active == true).ToList();
            ViewBag.Eqpt = _context.Eqptname.Where(x => x.active == true).ToList();
            return View();
        }

        // POST: Eqptissue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,date,branchid,EqptnameId,qty,remarks,active")] Eqptissue eqptissue)
        {
            if (ModelState.IsValid)
            {
                eqptissue.active = true;
                _context.Add(eqptissue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        // GET: Eqptissue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptissue = await _context.Eqptissue.FindAsync(id);
            if (eqptissue == null)
            {
                return NotFound();
            }

            ViewBag.Branch = _context.Branch.Where(x => x.active == true).ToList();
            ViewBag.Eqpt = _context.Eqptname.Where(x => x.active == true).ToList();

            return View(eqptissue);
        }

        // POST: Eqptissue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,date,branchid,eqptid,qty,remarks,active")] Eqptissue eqptissue)
        {
            if (id != eqptissue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eqptissue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EqptissueExists(eqptissue.Id))
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
            ViewData["branchid"] = new SelectList(_context.Branch, "Id", "Id", eqptissue.branchid);
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
