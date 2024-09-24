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
                    Id = e.Id,
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
            // In your controller action, prepare the ViewBag for the dropdown
            ViewBag.Eqpt = _context.Eqptstore
                                         .Include(e => e.Eqptname)
                                         .Where(x => x.active == true)
                                         .Select(e => new {
                                             e.eqptid, // Get the equipment ID
                                             EqptName = e.Eqptname.name, // Get the equipment name
                                             e.qty // Available quantity
                                         })
                                         .ToList();
            return View();
        }

      



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,date,branchid,EqptnameId,qty,remarks,active")] Eqptissue eqptissue)
        {
            ViewBag.Branch = _context.Branch.Where(x => x.active == true).ToList();
            ViewBag.Eqpt = _context.Eqptname.Where(x => x.active == true).ToList();
            if (ModelState.IsValid)
            {

                // Start a transaction to ensure both the issue and the qty update happen together
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Mark the new equipment issue as active
                        eqptissue.active = true;
                        _context.Add(eqptissue);

                        // Find the corresponding Eqptstore by EqptnameId (which is eqptid in Eqptstore)
                        var eqptstore = await _context.Eqptstore
                                                      .FirstOrDefaultAsync(e => e.eqptid == eqptissue.EqptnameId);

                        if (eqptstore != null)
                        {
                            // Reduce the qty in Eqptstore by the qty issued
                            eqptstore.qty -= eqptissue.qty;

                            // Ensure qty does not go below zero
                            if (eqptstore.qty < 0)
                            {
                                // Optionally, handle the case where there's not enough quantity
                                ModelState.AddModelError(string.Empty, "Not enough quantity in store to issue.");

                                ViewBag.Branch = _context.Branch.Where(x => x.active == true).ToList();
                                ViewBag.Eqpt = _context.Eqptname.Where(x => x.active == true).ToList();

                                await transaction.RollbackAsync();
                                return View(eqptissue);
                            }

                            // Update the Eqptstore entity
                            _context.Update(eqptstore);

                            // Save all changes (both the issue and the store update)
                            await _context.SaveChangesAsync();

                            // Commit the transaction
                            await transaction.CommitAsync();

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // Handle the case where the equipment is not found in the store
                            ModelState.AddModelError(string.Empty, "Equipment not found in store.");
                            await transaction.RollbackAsync();
                        }
                    }
                    catch (Exception)
                    {
                        // If any error occurs, roll back the transaction
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            // If we reach here, something failed; redisplay the form
            return View(eqptissue);
        }













        // GET: Eqptissue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include the related Eqptname entity
            var Eqptissue = await _context.Eqptissue
                               
                                          .Include(e => e.Eqptname)
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
                date = Eqptissue.date,
                branch = Eqptissue.Branch?.name,
                eqpt = Eqptissue.Eqptname?.name, // Assuming Eqptname has a property 'Name'
                qty = Eqptissue.qty,
                remarks = Eqptissue.remarks
            };

            return View(eqptissuevm);
        }


        // POST: Eqptissue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,date,branchid,eqptid,qty,remarks,active")] Eqptissue eqptissue)
        //{
        //    if (id != eqptissue.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(eqptissue);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EqptissueExists(eqptissue.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["branchid"] = new SelectList(_context.Branch, "Id", "Id", eqptissue.branchid);
        //    return View(eqptissue);
        //}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,eqptid,qty,remarks")] EqptissueVM eqptissue)
        {
           
            if (id != eqptissue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the original Eqptissue record
                    var originalIssue = await _context.Eqptissue.AsNoTracking()
                                                .FirstOrDefaultAsync(e => e.Id == id);

                    if (originalIssue == null)
                    {
                        return NotFound();
                    }

                    // Retrieve the corresponding Eqptstore record
                    var eqptstore = await _context.Eqptstore
                                                  .FirstOrDefaultAsync(e => e.eqptid == originalIssue.EqptnameId);

                    if (eqptstore != null)
                    {
                        // Check if qty is being updated
                        if (eqptissue.qty != originalIssue.qty)
                        {
                            int qtyDifference = originalIssue.qty.GetValueOrDefault() - eqptissue.qty.GetValueOrDefault();

                            // Check if we are decreasing the qty and if it's enough
                            if (qtyDifference < 0) // If increasing the quantity
                            {
                                int availableQty = eqptstore.qty.GetValueOrDefault();
                                int qtyToIssue = Math.Abs(qtyDifference); // Convert to positive for comparison

                                if (availableQty < qtyToIssue)
                                {
                                    // Set error message and refresh the page
                                    ViewBag.ErrorMessage = "Quantity not available in the store.";
                                    ViewBag.Refresh = true; // Flag for page refresh
                                    
                                    
                                    var  valreturn = _context.Eqptissue.Include(e => e.Eqptname)
                              .Include(e => e.Branch).FirstOrDefault(e => e.Id == id);
                                    // Return the view model with current values
                                    var errorViewModel = new EqptissueVM
                                    {
                                        Id = valreturn.Id,
                                        date = valreturn.date,
                                        branch = valreturn.Branch?.name,
                                        eqpt = valreturn.Eqptname?.name,
                                        qty = valreturn.qty,
                                        remarks = valreturn.remarks
                                    };

                                    return View(errorViewModel); // Return the current view with the error view model
                                }
                                // Decrease from the eqptstore
                                eqptstore.qty -= qtyToIssue;
                            }
                            else if (qtyDifference > 0) // If decreasing the quantity
                            {
                                eqptstore.qty += qtyDifference;
                            }

                            // Update the Eqptstore in the context
                            _context.Update(eqptstore);
                        }
                    }

                    // Update only the changed fields of Eqptissue
                    originalIssue.qty = eqptissue.qty; // Update qty
                    originalIssue.remarks = eqptissue.remarks; // Update remarks if provided

                    // Update the modified issue back to the context
                    _context.Update(originalIssue);
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
