﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gmt_Asset_Tracker.Data;
using Gmt_Asset_Tracker.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Gmt_Asset_Tracker.Controllers
{
    public class AssetController : Controller
    {
        private readonly AssetTrackerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AssetController(AssetTrackerContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Asset
        public async Task<IActionResult> Index()
        {
            var assetTrackerContext = _context.Assets.Include(a => a.Asset_State).Include(a => a.Category).Include(a => a.Department).Include(a => a.Location).Include(a => a.Present_location).Include(a => a.Vendor);
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
        public async Task<IActionResult> Create(int? id, [FromServices] IWebHostEnvironment hostingEnvironment, Asset asset)
        {
            if (ModelState.IsValid)
            {
                var assettag = await _context.Assets.FirstOrDefaultAsync(x => x.Asset_tag == asset.Asset_tag);
                if (assettag != null)
                {
                    ModelState.AddModelError("", "An asset with this asset tag already exists.");
                    return View(asset);
                }
                var servicetag = await _context.Assets.FirstOrDefaultAsync(x => x.Service_tag == asset.Service_tag);
                if (servicetag != null)
                {
                    ModelState.AddModelError("", "An asset with this service tag already exists");
                    return View(asset);
                }
                string imageName = "noimage.png";
                if (asset.ImageUpload != null)
                {
                    string folder = "media/images/";
                    imageName = Guid.NewGuid().ToString() + "_" + asset.ImageUpload.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder, imageName);

                    await asset.ImageUpload.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }

                string pdffile = "norequisitionpack.pdf";
                if (asset.RequistionpackUpload != null)
                {                    
                  string fileName = $"{hostingEnvironment.WebRootPath}\\media\\pdfs\\{asset.RequistionpackUpload.FileName}";
                   
                  pdffile = asset.RequistionpackUpload.FileName;
                    using (FileStream fileStream = System.IO.File.Create(fileName))
                    {
                       
                        asset.RequistionpackUpload.CopyTo(fileStream);
                        fileStream.Flush();
                    }                    
                }
                asset.Requistion_pack = pdffile;
                asset.Image = imageName;
               
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
        public async Task<IActionResult> Edit(int id, [FromServices] IWebHostEnvironment hostingEnvironment, Asset asset)
        {
            if (id != asset.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var assettag = await _context.Assets.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Asset_tag == asset.Asset_tag);
                if (assettag != null)
                {
                    ModelState.AddModelError("", "An asset with this asset tag already exists.");
                    return View(asset);
                }
                var servicetag = await _context.Assets.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Service_tag == asset.Service_tag);
                if (servicetag != null)
                {
                    ModelState.AddModelError("", "An asset with this service tag already exists");
                    return View(asset);
                }
                string imageName = "noimage.png";
                if (asset.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/images/");
                    if (!string.Equals(asset.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, asset.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    imageName = Guid.NewGuid().ToString() + "_" + asset.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await asset.ImageUpload.CopyToAsync(fs);                                       
                }
                string pdffile = "norequisitionpack.pdf";
                if (asset.RequistionpackUpload != null)
                {
                    string fileName = $"{hostingEnvironment.WebRootPath}\\media\\pdfs\\{asset.RequistionpackUpload.FileName}";
                    if (!string.Equals(asset.Requistion_pack, "norequisition.pdf"))
                    {
                        string oldPdfPath = Path.Combine(fileName);
                        if (System.IO.File.Exists(oldPdfPath))
                        {
                            System.IO.File.Delete(oldPdfPath);
                        }
                    }
                    pdffile = asset.RequistionpackUpload.FileName;
                    using (FileStream fileStream = System.IO.File.Create(fileName))
                    {

                        asset.RequistionpackUpload.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                   
                }
                asset.Requistion_pack = pdffile;
                asset.Image = imageName;
                _context.Update(asset);
                await _context.SaveChangesAsync();
            
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
        public async Task<IActionResult> DeleteConfirmed(int id, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            Asset asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                TempData["Error"] = "The asset does not exist";
            }
            else
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/images/");
                if (!string.Equals(asset.Image, "noimage.png"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, asset.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string fileName = Path.Combine(_webHostEnvironment.WebRootPath, "media/pdfs/");
                if (!string.Equals(asset.Requistion_pack, "norequisition.pdf"))
                {
                    string oldPdfPath = Path.Combine(fileName, asset.Requistion_pack);
                    if (System.IO.File.Exists(oldPdfPath))
                    {
                        System.IO.File.Delete(oldPdfPath);
                    }
                }
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));           
        }
        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.Id == id);
        }

        public IActionResult PDFViewerNewTab(string fileName)
        {
            string path = _webHostEnvironment.WebRootPath + "\\media\\pdfs\\" + fileName;
            return File(System.IO.File.ReadAllBytes(path), "application/pdf");
        }

    }

}
