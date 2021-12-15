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
    public class AssetController : Controller
    {
        private readonly AssetTrackerContext _context;

        public AssetController(AssetTrackerContext context)
        {
            _context = context;
        }

        // GET: Asset
        public async Task<IActionResult> Index()
        {
            var assetTrackerContext = _context.Assets.Include(a => a.Asset_State).Include(a => a.Category).Include(a => a.Department).Include(a => a.Location).Include(a => a.Physical_check).Include(a => a.Present_location).Include(a => a.Vendor);
            return View(await assetTrackerContext.ToListAsync());
        }

        // GET: Asset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.Asset_State)
                .Include(a => a.Category)
                .Include(a => a.Department)
                .Include(a => a.Location)
                .Include(a => a.Physical_check)
                .Include(a => a.Present_location)
                .Include(a => a.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Asset/Create
        public IActionResult Create()
        {
            ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
            ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name");
            ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name");
            return View();
        }

        // POST: Asset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Asset_name,Asset_description,CategoryId,LocationId,DepartmentId,AssetStateId,Asset_tag,Service_tag,Assigned_to,Purchased_price,VendorId,Delivery_date,Requistion_pack,CheckId,Image,PresentLocationId,Present_user")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
            ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name");
            ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name");
            return View(asset);
        }

        // GET: Asset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state", asset.AssetStateId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
            ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
            ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
            return View(asset);
        }

        // POST: Asset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Asset_name,Asset_description,CategoryId,LocationId,DepartmentId,AssetStateId,Asset_tag,Service_tag,Assigned_to,Purchased_price,VendorId,Delivery_date,Requistion_pack,CheckId,Image,PresentLocationId,Present_user")] Asset asset)
        {
            if (id != asset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.Id))
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
            ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state", asset.AssetStateId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
            ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
            ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
            return View(asset);
        }

        // GET: Asset/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.Asset_State)
                .Include(a => a.Category)
                .Include(a => a.Department)
                .Include(a => a.Location)
                .Include(a => a.Physical_check)
                .Include(a => a.Present_location)
                .Include(a => a.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // POST: Asset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.Id == id);
        }
    }
}
