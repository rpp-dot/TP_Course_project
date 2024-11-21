using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceAgency.Data;
using InsuranceAgency.Models;

namespace InsuranceAgency.Controllers
{
    public class AdministratorsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public AdministratorsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
              return _context.Administrators != null ? 
                          View(await _context.Administrators.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.Administrators'  is null.");
        }

        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Administrators == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // GET: Administrators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrator);
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Administrators == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministratorExists(administrator.Id))
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
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Administrators == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Administrators == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.Administrators'  is null.");
            }
            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator != null)
            {
                _context.Administrators.Remove(administrator);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministratorExists(int id)
        {
          return (_context.Administrators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
