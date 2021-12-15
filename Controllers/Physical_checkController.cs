using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gmt_Asset_Tracker.Data;
using Gmt_Asset_Tracker.Models;

namespace Gmt_Asset_Tracker.Controllers
{
    public class Physical_checkController : Controller
    {
        private readonly AssetTrackerContext _context;

        public Physical_checkController(AssetTrackerContext context)
        {
            _context = context;
        }

        // GET: Physical_check
        public async Task<IActionResult> Index()
        {
            return View(await _context.Physical_Checks.ToListAsync());
        }

        // GET: Physical_check/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physical_check = await _context.Physical_Checks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physical_check == null)
            {
                return NotFound();
            }

            return View(physical_check);
        }

        // GET: Physical_check/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Physical_check/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Check_name")] Physical_check physical_check)
        {
            if (ModelState.IsValid)
            {
                _context.Add(physical_check);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(physical_check);
        }

        // GET: Physical_check/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physical_check = await _context.Physical_Checks.FindAsync(id);
            if (physical_check == null)
            {
                return NotFound();
            }
            return View(physical_check);
        }

        // POST: Physical_check/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Check_name")] Physical_check physical_check)
        {
            if (id != physical_check.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(physical_check);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Physical_checkExists(physical_check.Id))
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
            return View(physical_check);
        }

        // GET: Physical_check/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physical_check = await _context.Physical_Checks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physical_check == null)
            {
                return NotFound();
            }

            return View(physical_check);
        }

        // POST: Physical_check/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var physical_check = await _context.Physical_Checks.FindAsync(id);
            _context.Physical_Checks.Remove(physical_check);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Physical_checkExists(int id)
        {
            return _context.Physical_Checks.Any(e => e.Id == id);
        }
    }
}
