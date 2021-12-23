﻿using System;
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
	public class DepartmentController : Controller
	{
		private readonly AssetTrackerContext _context;

		public DepartmentController(AssetTrackerContext context)
		{
			_context = context;
		}

		// GET: Department
		public async Task<IActionResult> Index()
		{
			return View(await _context.Departments.ToListAsync());
		}

		// GET: Department/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var department = await _context.Departments
				.FirstOrDefaultAsync(m => m.Id == id);
			if (department == null)
			{
				return NotFound();
			}

			return View(department);
		}

		// GET: Department/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Department/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Department_name")] Department department)
		{
			if (ModelState.IsValid)
			{
				_context.Add(department);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(department);
		}

		// GET: Department/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var department = await _context.Departments.FindAsync(id);
			if (department == null)
			{
				return NotFound();
			}
			return View(department);
		}

		// POST: Department/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Department_name")] Department department)
		{
			if (id != department.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(department);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DepartmentExists(department.Id))
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
			return View(department);
		}

		// GET: Department/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var department = await _context.Departments
				.FirstOrDefaultAsync(m => m.Id == id);
			if (department == null)
			{
				return NotFound();
			}

			return View(department);
		}

		// POST: Department/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			_context.Departments.Remove(department);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool DepartmentExists(int id)
		{
			return _context.Departments.Any(e => e.Id == id);
		}
	}
}