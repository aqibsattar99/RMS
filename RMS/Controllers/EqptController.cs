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
        {  // Check session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Name")))
            {
                return RedirectToAction("Index", "Account");
            }
            return View(await _context.Eqpttype.Where(x=>x.Active == true).ToListAsync());
        }

        // GET: Eqpt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eqptname = await _context.Eqpttype
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
        public async Task<IActionResult> Create([Bind("Id,Name,active")] Eqpttype eqptname)
        {
            if (ModelState.IsValid)
            {
                eqptname.Active = true;
                _context.Add(eqptname);
                await _context.SaveChangesAsync();
              


                // Now add the record to the Eqptstore table
                var eqptstore = new Eqptstore
                {
                    Eqptid = eqptname.Id, 
                    Qty = 0,
                    Date = DateTime.Now,
                    Active = true,
                    Updatedon = DateTime.Now,   
                };

                // Add the eqptstore entity to the context
                _context.Add(eqptstore);

                // Save the changes to the database
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

            var eqptname = await _context.Eqpttype.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,active")] Eqpttype eqptname)
        {
            if (id != eqptname.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    eqptname.Active = true;
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

            var eqptname = await _context.Eqpttype
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
            var eqptname = await _context.Eqpttype.FindAsync(id);
            if (eqptname != null)
            {
              
                eqptname.Active = false;
                _context.Eqpttype.Update(eqptname);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EqptnameExists(int id)
        {
            return _context.Eqpttype.Any(e => e.Id == id);
        }
    }
}
