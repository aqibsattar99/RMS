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
    public class EqptstoreController : Controller
    {
        private readonly dbRMSContext _context;

        public EqptstoreController(dbRMSContext context)
        {
            _context = context;
        }

        // GET: Eqptstore
        public async Task<IActionResult> Index()
        {
            var data = await _context.Eqptstore
               .Include(e => e.Eqptname)
               .Where(e => e.active == true)
               .Select(e => new EqptstoreVM
               {
                   Id = e.Id,
                   date = e.date,
                   eqptname = e.Eqptname.name,
                   qty = e.qty,
               })
               .ToListAsync();

            return View(data);
           
        }

        // GET: Eqptstore/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptstore = await _context.Eqptstore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptstore == null)
            {
                return NotFound();
            }

            return View(eqptstore);
        }

        // GET: Eqptstore/Create
        public IActionResult Create()
        {
            ViewBag.Eqpt = _context.Eqptname.Where(x => x.active == true).ToList();

            return View();
        }

        // POST: Eqptstore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,date,eqptid,qty,active")] Eqptstore eqptstore)
        {
            if (ModelState.IsValid)
            {
                eqptstore.active = true;
                _context.Add(eqptstore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eqptstore);
        }

        // GET: Eqptstore/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptstore = await _context.Eqptstore.FindAsync(id);
            if (eqptstore == null)
            {
                return NotFound();
            }
            return View(eqptstore);
        }

        // POST: Eqptstore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,date,eqptid,qty,active")] Eqptstore eqptstore)
        {
            if (id != eqptstore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eqptstore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EqptstoreExists(eqptstore.Id))
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
            return View(eqptstore);
        }

        // GET: Eqptstore/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptstore = await _context.Eqptstore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptstore == null)
            {
                return NotFound();
            }

            return View(eqptstore);
        }

        // POST: Eqptstore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eqptstore = await _context.Eqptstore.FindAsync(id);
            if (eqptstore != null)
            {
                _context.Eqptstore.Remove(eqptstore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EqptstoreExists(int id)
        {
            return _context.Eqptstore.Any(e => e.Id == id);
        }
    }
}
