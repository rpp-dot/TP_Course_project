using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceAgency.Data;
using InsuranceAgency.Models;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceAgency.Controllers
{
    [Authorize(Roles = "Administrator, InsuranceAgent")]
    public class InsuranceObjectsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public InsuranceObjectsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: InsuranceObjects
        public async Task<IActionResult> Index()
        {
              return _context.InsuranceObjects != null ? 
                          View(await _context.InsuranceObjects.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.InsuranceObjects'  is null.");
        }

        // GET: InsuranceObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InsuranceObjects == null)
            {
                return NotFound();
            }

            var insuranceObject = await _context.InsuranceObjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceObject == null)
            {
                return NotFound();
            }

            return View(insuranceObject);
        }

        // GET: InsuranceObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceObjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Type")] InsuranceObject insuranceObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuranceObject);
        }

        // GET: InsuranceObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsuranceObjects == null)
            {
                return NotFound();
            }

            var insuranceObject = await _context.InsuranceObjects.FindAsync(id);
            if (insuranceObject == null)
            {
                return NotFound();
            }
            return View(insuranceObject);
        }

        // POST: InsuranceObjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Type")] InsuranceObject insuranceObject)
        {
            if (id != insuranceObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceObjectExists(insuranceObject.Id))
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
            return View(insuranceObject);
        }

        [Authorize(Roles = "Administrator")]
        // GET: InsuranceObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InsuranceObjects == null)
            {
                return NotFound();
            }

            var insuranceObject = await _context.InsuranceObjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceObject == null)
            {
                return NotFound();
            }

            return View(insuranceObject);
        }

        [Authorize(Roles = "Administrator")]
        // POST: InsuranceObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InsuranceObjects == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.InsuranceObjects'  is null.");
            }
            var insuranceObject = await _context.InsuranceObjects.FindAsync(id);
            if (insuranceObject != null)
            {
                _context.InsuranceObjects.Remove(insuranceObject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceObjectExists(int id)
        {
          return (_context.InsuranceObjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
