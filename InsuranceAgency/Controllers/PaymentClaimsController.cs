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
using System.Security.Claims;

namespace InsuranceAgency.Controllers
{
    [Authorize(Roles = "Administrator, InsuranceAgent, Accountant")]
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
            var insuranceAgencyDbContext = _context.PaymentClaims.Include(p => p.Client).Include(p => p.Policy);
            return View(await insuranceAgencyDbContext.ToListAsync());
        }

        // GET: PaymentClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaymentClaims == null)
            {
                return NotFound();
            }

            var paymentClaim = await _context.PaymentClaims
                .Include(p => p.Client)
                .Include(p => p.Policy)
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["PolicyId"] = new SelectList(_context.Policies, "Id", "Type");
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", paymentClaim.ClientId);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "Id", "Type", paymentClaim.PolicyId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", paymentClaim.ClientId);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "Id", "Type", paymentClaim.PolicyId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", paymentClaim.ClientId);
            ViewData["PolicyId"] = new SelectList(_context.Policies, "Id", "Type", paymentClaim.PolicyId);
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
                .Include(p => p.Client)
                .Include(p => p.Policy)
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

        // Новый метод для AJAX-фильтрации
        [HttpGet]
        public async Task<IActionResult> Filter(string status, string search)
        {
            var claimsQuery = _context.PaymentClaims
                .Include(p => p.Client)
                .Include(p => p.Policy)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse(status, out ClaimStatus claimStatus))
                {
                    claimsQuery = claimsQuery.Where(c => c.ClaimStatus == claimStatus);
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                claimsQuery = claimsQuery.Where(c =>
                    EF.Functions.Like(c.Client.Email, $"%{search}%") ||
                    EF.Functions.Like(c.Client.Name + " " + c.Client.Surname + " " + c.Client.Patronymic, $"%{search}%"));
            }

            var filteredClaims = await claimsQuery.ToListAsync();
            return PartialView("_ClaimTable", filteredClaims);
        }

        // POST: PaymentClaims/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, ClaimStatus newStatus)
        {
            var claim = await _context.PaymentClaims.Include(c => c.Policy).Include(c => c.Client).Include(c => c.Policy.InsuranceObject).FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null) return NotFound();

            // Check and handle status changes
            switch (newStatus)
            {
                case ClaimStatus.Одобрена:
                    claim.ClaimStatus = newStatus;
                    break;
                case ClaimStatus.Отклонена:
                    claim.ClaimStatus = newStatus;
                    break;
                case ClaimStatus.Обрабатывается:
                    claim.ClaimStatus = newStatus;
                    break;
                case ClaimStatus.Завершена:
                    claim.ClaimStatus = newStatus;
                    break;
                case ClaimStatus.Оплачена:
                    claim.ClaimStatus = newStatus;
                    break;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
