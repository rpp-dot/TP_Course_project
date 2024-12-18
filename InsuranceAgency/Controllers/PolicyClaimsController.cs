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
    [Authorize(Roles = "Administrator, InsuranceAgent")]
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
            var insuranceAgencyDbContext = _context.PolicyClaims.Include(p => p.Client).Include(p => p.InsuranceObject).Include(p => p.Service);
            return View(await insuranceAgencyDbContext.ToListAsync());
        }

        // GET: PolicyClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PolicyClaims == null)
            {
                return NotFound();
            }

            var policyClaim = await _context.PolicyClaims
                .Include(p => p.Client)
                .Include(p => p.InsuranceObject)
                .Include(p => p.Service)
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "InsuranceObjectType");
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policyClaim.ClientId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policyClaim.InsuranceObjectId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "InsuranceObjectType", policyClaim.ServiceId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policyClaim.ClientId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policyClaim.InsuranceObjectId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "InsuranceObjectType", policyClaim.ServiceId);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policyClaim.ClientId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policyClaim.InsuranceObjectId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "InsuranceObjectType", policyClaim.ServiceId);
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
                .Include(p => p.Client)
                .Include(p => p.InsuranceObject)
                .Include(p => p.Service)
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

        // AJAX Filter: Search by ClaimStatus
        [HttpGet]
        public async Task<IActionResult> FilterByStatus(ClaimStatus? status, string searchQuery)
        {
            var filteredClaims = _context.PolicyClaims
                .Include(p => p.Client)
                .Include(p => p.InsuranceObject)
                .Include(p => p.Service)
                .AsQueryable();

            if (status.HasValue)
            {
                filteredClaims = filteredClaims.Where(c => c.ClaimStatus == status.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filteredClaims = filteredClaims.Where(c =>
                    c.Client.Name.Contains(searchQuery) ||
                    c.Client.Surname.Contains(searchQuery) ||
                    c.Client.Patronymic.Contains(searchQuery) ||
                    c.Client.Email.Contains(searchQuery));
            }

            return PartialView("_PolicyClaimsTable", await filteredClaims.ToListAsync());
        }

        // POST: PolicyClaims/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, ClaimStatus newStatus)
        {
            var claim = await _context.PolicyClaims.Include(c => c.Service).Include(c => c.InsuranceObject).FirstOrDefaultAsync(c => c.Id == id);
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
                    if (claim.ClaimStatus == ClaimStatus.Оплачена)
                    {
                        // Create Policy
                        var policy = new Policy
                        {
                            Type = claim.Service.InsuranceObjectType,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddYears(1),
                            PremiumAmount = claim.InsuranceObject.Price * claim.Service.PremiumCoef,
                            PaymentCoef = claim.Service.PaymentCoef,
                            Status = PolicyStatus.Активен,
                            InsuranceObjectId = claim.InsuranceObjectId,
                            InsuranceAgentId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                            ClientId = claim.ClientId
                        };
                        _context.Policies.Add(policy);
                    }
                    claim.ClaimStatus = ClaimStatus.Завершена;
                    break;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool PolicyClaimExists(int id)
        {
          return (_context.PolicyClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
