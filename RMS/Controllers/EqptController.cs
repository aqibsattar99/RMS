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
    public class EqptController : Controller
    {
        private readonly dbRMSContext _context;

        public EqptController(dbRMSContext context)
        {
            _context = context;
        }

        // GET: Eqpt
        public async Task<IActionResult> Index()
        {
            return View(await _context.Eqptname.Where(x=>x.active == true).ToListAsync());
        }

        // GET: Eqpt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptname = await _context.Eqptname
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptname == null)
            {
                return NotFound();
            }

            return View(eqptname);
        }

        // GET: Eqpt/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eqpt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,active")] Eqptname eqptname)
        {
            if (ModelState.IsValid)
            {
                eqptname.active = true;
                _context.Add(eqptname);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eqptname);
        }

        // GET: Eqpt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptname = await _context.Eqptname.FindAsync(id);
            if (eqptname == null)
            {
                return NotFound();
            }
            return View(eqptname);
        }

        // POST: Eqpt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,active")] Eqptname eqptname)
        {
            if (id != eqptname.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    eqptname.active = true;
                    _context.Update(eqptname);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EqptnameExists(eqptname.Id))
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
            return View(eqptname);
        }

        // GET: Eqpt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptname = await _context.Eqptname
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eqptname == null)
            {
                return NotFound();
            }

            return View(eqptname);
        }

        // POST: Eqpt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eqptname = await _context.Eqptname.FindAsync(id);
            if (eqptname != null)
            {
                _context.Eqptname.Remove(eqptname);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EqptnameExists(int id)
        {
            return _context.Eqptname.Any(e => e.Id == id);
        }
    }
}
