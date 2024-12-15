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
    public class PoliciesController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public PoliciesController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: Policies
        public async Task<IActionResult> Index()
        {
            var insuranceAgencyDbContext = _context.Policies.Include(p => p.Client).Include(p => p.InsuranceAgent).Include(p => p.InsuranceObject);
            return View(await insuranceAgencyDbContext.ToListAsync());
        }

        // GET: Policies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.Client)
                .Include(p => p.InsuranceAgent)
                .Include(p => p.InsuranceObject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // GET: Policies/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["InsuranceAgentId"] = new SelectList(_context.InsuranceAgents, "Id", "Email");
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type");
            return View();
        }

        // POST: Policies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,StartDate,EndDate,PremiumAmount,PaymentCoef,Status,InsuranceObjectId,InsuranceAgentId,ClientId")] Policy policy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policy.ClientId);
            ViewData["InsuranceAgentId"] = new SelectList(_context.InsuranceAgents, "Id", "Email", policy.InsuranceAgentId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policy.InsuranceObjectId);
            return View(policy);
        }

        // GET: Policies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policy.ClientId);
            ViewData["InsuranceAgentId"] = new SelectList(_context.InsuranceAgents, "Id", "Email", policy.InsuranceAgentId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policy.InsuranceObjectId);
            return View(policy);
        }

        // POST: Policies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,StartDate,EndDate,PremiumAmount,PaymentCoef,Status,InsuranceObjectId,InsuranceAgentId,ClientId")] Policy policy)
        {
            if (id != policy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", policy.ClientId);
            ViewData["InsuranceAgentId"] = new SelectList(_context.InsuranceAgents, "Id", "Email", policy.InsuranceAgentId);
            ViewData["InsuranceObjectId"] = new SelectList(_context.InsuranceObjects, "Id", "Type", policy.InsuranceObjectId);
            return View(policy);
        }

        // GET: Policies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .Include(p => p.Client)
                .Include(p => p.InsuranceAgent)
                .Include(p => p.InsuranceObject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Policies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Policies == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.Policies'  is null.");
            }
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyExists(int id)
        {
          return (_context.Policies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // AJAX: Фильтрация по статусу
        public async Task<IActionResult> FilterByStatus(int status)
        {
            var policies = await _context.Policies
                .Include(p => p.Client)
                .Where(p => (int)p.Status == status)
                .ToListAsync();

            return PartialView("_PolicyRows", policies);
        }

        // AJAX: Поиск по Email и ФИО клиента
        public async Task<IActionResult> SearchByClient(string query)
        {
            var policies = await _context.Policies
                .Include(p => p.Client)
                .Where(p => p.Client.Email.Contains(query) ||
                            (p.Client.Name + " " + p.Client.Surname + " " + p.Client.Patronymic).Contains(query))
                .ToListAsync();

            return PartialView("_PolicyRows", policies);
        }

        // AJAX: Изменение статуса полиса
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, PolicyStatus status)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            policy.Status = status;
            _context.Update(policy);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
