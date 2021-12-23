using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gmt_Asset_Tracker.Data;
using Gmt_Asset_Tracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gmt_Asset_Tracker.Controllers
{
	[Authorize]
	public class Asset_StateController : Controller
	{
		private readonly AssetTrackerContext _context;

		public Asset_StateController(AssetTrackerContext context)
		{
			_context = context;
		}

		// GET: Asset_State
		public async Task<IActionResult> Index()
		{
			return View(await _context.Asset_States.ToListAsync());
		}

		// GET: Asset_State/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var asset_State = await _context.Asset_States
				.FirstOrDefaultAsync(m => m.Id == id);
			if (asset_State == null)
			{
				return NotFound();
			}

			return View(asset_State);
		}

		// GET: Asset_State/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Asset_State/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Asset_state")] Asset_State asset_States)
		{
			if (ModelState.IsValid)
			{
				_context.Add(asset_States);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(asset_States);
		}

		// GET: Asset_State/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var asset_State = await _context.Asset_States.FindAsync(id);
			if (asset_State == null)
			{
				return NotFound();
			}
			return View(asset_State);
		}

		// POST: Asset_State/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Asset_state")] Asset_State asset_States)
		{
			if (id != asset_States.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(asset_States);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!Asset_StateExists(asset_States.Id))
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
			return View(asset_States);
		}

		// GET: Asset_State/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var asset_State = await _context.Asset_States
				.FirstOrDefaultAsync(m => m.Id == id);
			if (asset_State == null)
			{
				return NotFound();
			}

			return View(asset_State);
		}

		// POST: Asset_State/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var asset_State = await _context.Asset_States.FindAsync(id);
			_context.Asset_States.Remove(asset_State);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool Asset_StateExists(int id)
		{
			return _context.Asset_States.Any(e => e.Id == id);
		}
	}
}