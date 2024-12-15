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
    [Authorize(Roles ="Administrator")]
    public class AccountantsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public AccountantsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: Accountants
        public async Task<IActionResult> Index()
        {
              return _context.Accountants != null ? 
                          View(await _context.Accountants.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.Accountants'  is null.");
        }

        // GET: Accountants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accountants == null)
            {
                return NotFound();
            }

            var accountant = await _context.Accountants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountant == null)
            {
                return NotFound();
            }

            return View(accountant);
        }

        // GET: Accountants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accountants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] Accountant accountant)
        {
            if (ModelState.IsValid)
            {
                accountant.Password = BCrypt.Net.BCrypt.HashPassword(accountant.Password);
                _context.Add(accountant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountant);
        }

        // GET: Accountants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accountants == null)
            {
                return NotFound();
            }

            var accountant = await _context.Accountants.FindAsync(id);
            if (accountant == null)
            {
                return NotFound();
            }
            return View(accountant);
        }

        // POST: Accountants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] Accountant accountant)
        {
            if (id != accountant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    accountant.Password = BCrypt.Net.BCrypt.HashPassword(accountant.Password);
                    _context.Update(accountant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountantExists(accountant.Id))
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
            return View(accountant);
        }

        // GET: Accountants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accountants == null)
            {
                return NotFound();
            }

            var accountant = await _context.Accountants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountant == null)
            {
                return NotFound();
            }

            return View(accountant);
        }

        // POST: Accountants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accountants == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.Accountants'  is null.");
            }
            var accountant = await _context.Accountants.FindAsync(id);
            if (accountant != null)
            {
                _context.Accountants.Remove(accountant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountantExists(int id)
        {
          return (_context.Accountants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
