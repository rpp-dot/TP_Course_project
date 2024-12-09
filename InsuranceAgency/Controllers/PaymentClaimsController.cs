﻿using System;
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
    public class PaymentClaimsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public PaymentClaimsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: PaymentClaims
        public async Task<IActionResult> Index()
        {
              return _context.PaymentClaims != null ? 
                          View(await _context.PaymentClaims.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.PaymentClaims'  is null.");
        }

        // GET: PaymentClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaymentClaims == null)
            {
                return NotFound();
            }

            var paymentClaim = await _context.PaymentClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentClaim == null)
            {
                return NotFound();
            }

            return View(paymentClaim);
        }

        // GET: PaymentClaims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentClaims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimAmount,ClientBankAccount,PolicyId,ClientId,Id,Description,ClaimDate,ClaimStatus")] PaymentClaim paymentClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentClaim);
        }

        // GET: PaymentClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PaymentClaims == null)
            {
                return NotFound();
            }

            var paymentClaim = await _context.PaymentClaims.FindAsync(id);
            if (paymentClaim == null)
            {
                return NotFound();
            }
            return View(paymentClaim);
        }

        // POST: PaymentClaims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimAmount,ClientBankAccount,PolicyId,ClientId,Id,Description,ClaimDate,ClaimStatus")] PaymentClaim paymentClaim)
        {
            if (id != paymentClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentClaimExists(paymentClaim.Id))
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
            return View(paymentClaim);
        }

        // GET: PaymentClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PaymentClaims == null)
            {
                return NotFound();
            }

            var paymentClaim = await _context.PaymentClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentClaim == null)
            {
                return NotFound();
            }

            return View(paymentClaim);
        }

        // POST: PaymentClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PaymentClaims == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.PaymentClaims'  is null.");
            }
            var paymentClaim = await _context.PaymentClaims.FindAsync(id);
            if (paymentClaim != null)
            {
                _context.PaymentClaims.Remove(paymentClaim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentClaimExists(int id)
        {
          return (_context.PaymentClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
