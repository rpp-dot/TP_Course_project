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
    [Authorize(Roles = "Administrator, InsuranceAgent, Client")]
    public class PolicyClaimsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public PolicyClaimsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrator, InsuranceAgent")]
        // GET: PolicyClaims
        public async Task<IActionResult> Index()
        {
            var insuranceAgencyDbContext = _context.PolicyClaims.Include(p => p.Client).Include(p => p.InsuranceObject).Include(p => p.Service);
            return View(await insuranceAgencyDbContext.ToListAsync());
        }
        [Authorize(Roles = "Administrator, InsuranceAgent")]
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
        [Authorize(Roles = "Administratort")]
        // GET: PolicyClaims/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "InsuranceObjectType");
            return View();
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator, InsuranceAgent")]
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
        [Authorize(Roles = "Administrator, InsuranceAgent")]
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
        [Authorize(Roles = "Administrator, InsuranceAgent")]
        private bool PolicyClaimExists(int id)
        {
          return (_context.PolicyClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //GET: ClientPolicyClaims
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientPolicyClaims()
        {
            // Получаем ID текущего пользователя (предполагается, что он соответствует ClientId)
            var clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clientId) || !int.TryParse(clientId, out int parsedClientId))
            {
                return Unauthorized(); // Если ID клиента не найден, запрещаем доступ
            }

            // Фильтруем претензии по текущему клиенту
            var clientPolicyClaims = await _context.PolicyClaims
                .Include(p => p.Client)
                .Include(p => p.InsuranceObject)
                .Include(p => p.Service)
                .Where(p => p.ClientId == parsedClientId)
                .ToListAsync();

            return View(clientPolicyClaims);
        }
        //GET: ClientPolicyClaims/Details
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientPolicyClaimDetails(int? id)
        {
            if (id == null || _context.PolicyClaims == null)
            {
                return NotFound();
            }

            // Получаем ID текущего клиента
            var clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clientId) || !int.TryParse(clientId, out int parsedClientId))
            {
                return Unauthorized();
            }

            // Ищем претензию, принадлежащую текущему клиенту
            var policyClaim = await _context.PolicyClaims
                .Include(p => p.Client)
                .Include(p => p.InsuranceObject)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.Id == id && m.ClientId == parsedClientId);

            if (policyClaim == null)
            {
                return NotFound();
            }

            return View(policyClaim);
        }
    }
}
