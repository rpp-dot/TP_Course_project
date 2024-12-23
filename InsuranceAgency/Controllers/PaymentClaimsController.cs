﻿using System;
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
    [Authorize(Roles = "Administrator, InsuranceAgent, Accountant, Client")]
    public class PaymentClaimsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public PaymentClaimsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrator, InsuranceAgent, Accountant")]
        // GET: PaymentClaims
        public async Task<IActionResult> Index()
        {
            var insuranceAgencyDbContext = _context.PaymentClaims.Include(p => p.Client).Include(p => p.Policy).OrderByDescending(p => p.ClaimDate);
            return View(await insuranceAgencyDbContext.ToListAsync());
        }
        [Authorize(Roles = "Administrator, InsuranceAgent, Accountant")]
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
        [Authorize(Roles = "Administrator")]
        // GET: PaymentClaims/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["PolicyId"] = new SelectList(_context.Policies, "Id", "Type");
            return View();
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator,  InsuranceAgent, Accountant")]
        private bool PaymentClaimExists(int id)
        {
          return (_context.PaymentClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Administrator,  InsuranceAgent, Accountant")]
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
        [Authorize(Roles = "Administrator,  InsuranceAgent, Accountant")]
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

        //GET: ClientPaymentClaims
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientPaymentClaims()
        {
            // Получаем ID текущего пользователя (предполагается, что он соответствует ClientId)
            var clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clientId) || !int.TryParse(clientId, out int parsedClientId))
            {
                return Unauthorized(); // Если ID клиента не найден, запрещаем доступ
            }

            // Фильтруем платежи по текущему клиенту
            var clientPaymentClaims = await _context.PaymentClaims
                .Include(p => p.Client)
                .Include(p => p.Policy)
                .Where(p => p.ClientId == parsedClientId).OrderByDescending(p => p.ClaimDate)
                .ToListAsync();

            return View(clientPaymentClaims);
        }

        //GET: ClientPaymentClaims/Details
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientPaymentClaimDetails(int? id)
        {
            if (id == null || _context.PaymentClaims == null)
            {
                return NotFound();
            }

            // Получаем ID текущего клиента
            var clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clientId) || !int.TryParse(clientId, out int parsedClientId))
            {
                return Unauthorized();
            }

            // Ищем платеж, принадлежащий текущему клиенту
            var paymentClaim = await _context.PaymentClaims
                .Include(p => p.Client)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(m => m.Id == id && m.ClientId == parsedClientId);

            if (paymentClaim == null)
            {
                return NotFound();
            }

            return View(paymentClaim);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public IActionResult ClientCreate(int policyId)
        {
            // Ensure the Policy exists
            var policy = _context.Policies.FirstOrDefault(p => p.Id == policyId);
            if (policy == null || policy.Status != PolicyStatus.Активен)
            {
                return NotFound();
            }

            // Prepare the model for the view
            var model = new PaymentClaim
            {
                PolicyId = policyId
            };
            return View(model);
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClientCreate(PaymentClaim model)
        {
            // Get the logged-in user's ID
            var clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (clientId == null)
            {
                return Unauthorized();
            }
            var policy = _context.Policies.Include(p => p.InsuranceObject).FirstOrDefault(p => p.Id == model.PolicyId);
            // Set the default values
            model.ClientId = int.Parse(clientId);
            model.ClaimStatus = ClaimStatus.Создана;
            model.ClaimDate = DateOnly.FromDateTime(DateTime.Now);
            model.ClaimAmount = Decimal.Round(Decimal.Multiply(policy.PaymentCoef, policy.InsuranceObject.Price), 2);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.PaymentClaims.Add(model);
            _context.SaveChanges();

            return RedirectToAction("ClientPaymentClaims", "PaymentClaims"); // Redirect to a relevant page
        }

    }
}
