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
    public class PolicyClaimsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public PolicyClaimsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: PolicyClaims
        public async Task<IActionResult> Index()
        {
              return _context.PolicyClaims != null ? 
                          View(await _context.PolicyClaims.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.PolicyClaims'  is null.");
        }

        // GET: PolicyClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PolicyClaims == null)
            {
                return NotFound();
            }

            var policyClaim = await _context.PolicyClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyClaim == null)
            {
                return NotFound();
            }

            return View(policyClaim);
        }

        // GET: PolicyClaims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PolicyClaims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,InsuranceObjectId,ClientId,Id,Description,ClaimDate,ClaimStatus")] PolicyClaim policyClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policyClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(policyClaim);
        }

        // GET: PolicyClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PolicyClaims == null)
            {
                return NotFound();
            }

            var policyClaim = await _context.PolicyClaims.FindAsync(id);
            if (policyClaim == null)
            {
                return NotFound();
            }
            return View(policyClaim);
        }

        // POST: PolicyClaims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,InsuranceObjectId,ClientId,Id,Description,ClaimDate,ClaimStatus")] PolicyClaim policyClaim)
        {
            if (id != policyClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policyClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyClaimExists(policyClaim.Id))
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
            return View(policyClaim);
        }

        // GET: PolicyClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PolicyClaims == null)
            {
                return NotFound();
            }

            var policyClaim = await _context.PolicyClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyClaim == null)
            {
                return NotFound();
            }

            return View(policyClaim);
        }

        // POST: PolicyClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PolicyClaims == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.PolicyClaims'  is null.");
            }
            var policyClaim = await _context.PolicyClaims.FindAsync(id);
            if (policyClaim != null)
            {
                _context.PolicyClaims.Remove(policyClaim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyClaimExists(int id)
        {
          return (_context.PolicyClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
