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
    public class InsuranceAgentsController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public InsuranceAgentsController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        // GET: InsuranceAgents
        public async Task<IActionResult> Index()
        {
              return _context.InsuranceAgents != null ? 
                          View(await _context.InsuranceAgents.ToListAsync()) :
                          Problem("Entity set 'InsuranceAgencyDbContext.InsuranceAgents'  is null.");
        }

        // GET: InsuranceAgents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InsuranceAgents == null)
            {
                return NotFound();
            }

            var insuranceAgent = await _context.InsuranceAgents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceAgent == null)
            {
                return NotFound();
            }

            return View(insuranceAgent);
        }

        // GET: InsuranceAgents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceAgents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] InsuranceAgent insuranceAgent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insuranceAgent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuranceAgent);
        }

        // GET: InsuranceAgents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsuranceAgents == null)
            {
                return NotFound();
            }

            var insuranceAgent = await _context.InsuranceAgents.FindAsync(id);
            if (insuranceAgent == null)
            {
                return NotFound();
            }
            return View(insuranceAgent);
        }

        // POST: InsuranceAgents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Salary,Id,Login,Password,Surname,Name,Patronymic,Email,PhoneNumber,Role")] InsuranceAgent insuranceAgent)
        {
            if (id != insuranceAgent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceAgent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceAgentExists(insuranceAgent.Id))
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
            return View(insuranceAgent);
        }

        // GET: InsuranceAgents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InsuranceAgents == null)
            {
                return NotFound();
            }

            var insuranceAgent = await _context.InsuranceAgents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceAgent == null)
            {
                return NotFound();
            }

            return View(insuranceAgent);
        }

        // POST: InsuranceAgents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InsuranceAgents == null)
            {
                return Problem("Entity set 'InsuranceAgencyDbContext.InsuranceAgents'  is null.");
            }
            var insuranceAgent = await _context.InsuranceAgents.FindAsync(id);
            if (insuranceAgent != null)
            {
                _context.InsuranceAgents.Remove(insuranceAgent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceAgentExists(int id)
        {
          return (_context.InsuranceAgents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
