using System;
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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Gmt_Asset_Tracker.ViewModels;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

//using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using DocumentFormat.OpenXml.Bibliography;
using System.Security.Policy;

namespace Gmt_Asset_Tracker.Controllers
{
	[Authorize]
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
			//int pagesize = 2;
			var assets = _context.Assets.Include(a => a.Asset_State)
				.Include(a => a.Category)
				.Include(a => a.Department)
				.Include(a => a.Location)
				.Include(a => a.Present_location)
				.Include(a => a.Vendor)
				.Include(a => a.Physical_check)
				.OrderByDescending(p => p.Delivery_date);
			//.Skip((p - 1) * pagesize)
			//.Take(pagesize);

			ViewData["Assets"] = _context.Assets.Count();
			ViewData["Vendors"] = _context.Vendors.Count();
			ViewData["Departments"] = _context.Departments.Count();
			ViewData["Locations"] = _context.Locations.Count();

			//ViewBag.PageNumber = p;
			//ViewBag.PageRange = pagesize;
			//ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Assets.Count() / pagesize);

			//var assetTrackerContext = _context.Assets.Include(a => a.Asset_State).Include(a => a.Category).Include(a => a.Department).Include(a => a.Location).Include(a => a.Present_location).Include(a => a.Vendor);
			return View(await assets.ToListAsync());
		}

		public async Task<IActionResult> nullassettag()
		{
			var assets = _context.Assets.Include(a => a.Asset_State)
				.Include(a => a.Category)
				.Include(a => a.Department)
				.Include(a => a.Location)
				.Include(a => a.Present_location)
				.Include(a => a.Vendor)
				.Include(a => a.Physical_check)
				.OrderByDescending(p => p.Delivery_date).Where(x => x.Asset_tag == null);

			return View(await assets.ToListAsync());
		}

		public IActionResult Email()
		{
			var assets = _context.Assets.Include(a => a.Asset_State)
				.Include(a => a.Category)
				.Include(a => a.Department)
				.Include(a => a.Location)
				.Include(a => a.Present_location)
				.Include(a => a.Vendor)
				.Include(a => a.Physical_check)
				.OrderByDescending(p => p.Delivery_date).Where(x => x.Asset_tag == null);

			//ViewData["url"] = url;

			//var message = new MimeMessage();
			//message.From.Add(new MailboxAddress("GMT Vendor Evaluation", "Auto.Mail@gmt-limited.com"));
			//message.To.Add(new MailboxAddress(departmentinfo.email));
			//message.Subject = "GMT Vendor Evaluation Evaluation";
			//message.Body = new BodyBuilder { HtmlBody = string.Format("<h3 style='color:black;'>Click on the link below to Evaluate this Product/Service({0}) delivered to your Department <hr /> {1}</h3>", productname, url) }.ToMessageBody();

			//using (var client = new SmtpClient())
			//{
			//    client.Connect("smtp.office365.com", 587, false);
			//    client.Authenticate("Auto.Mail@gmt-limited.com", "Hav!34iT");

			//    client.Send(message);
			//    client.Disconnect(true);
			//}

			//var message = new MimeMessage();
			//message.From.Add(new MailboxAddress("GMT Vendor Evaluation", "fisayo.adegun@gmt-limited.com"));
			//message.To.Add(new MailboxAddress("i_zzyfizzy@live.com"));
			//message.Subject = "GMT Vendor Evaluation Evaluation";
			//message.Body = new BodyBuilder { HtmlBody = string.Format("<h3 style='color:black;'>Click on the link below to Evaluate this Product/Service({0}) delivered to your Department <hr /> {1}</h3>") }.ToMessageBody();

			//using (var client = new SmtpClient())
			//{
			//	client.Connect("smtp.office365.com", 587, false);
			//	client.Authenticate("fisayo.adegun@gmt-limited.com", "Surulere007");

			//	client.Send(message);
			//	client.Disconnect(true);
			//}
			return View();
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
				if (assettag != null && asset.Asset_tag != null)
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

				TempData["Success"] = "The Asset has been added";
				return RedirectToAction(nameof(Index));
			}
			ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state");
			ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name");
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name");
			ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
			ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name");
			ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name");
			ViewBag.VendorId = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			return View(asset);
		}

		// GET: Asset/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			Asset asset = await _context.Assets.FindAsync(id);
			if (id == null)
			{
				return NotFound();
			}

			ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state", asset.AssetStateId);
			ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
			ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
			ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
			//ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			ViewBag.VendorId = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			return View(asset);
		}

		// POST: Asset/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [FromServices] IWebHostEnvironment hostingEnvironment, Asset asset)
		{
			//if (id != asset.Id)
			//{
			//	return NotFound();
			//}
			ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
			if (ModelState.IsValid)
			{
				var assettag = await _context.Assets.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Asset_tag == asset.Asset_tag);
				if (assettag != null && asset.Asset_tag != null)
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
					string imageName = Guid.NewGuid().ToString() + "_" + asset.ImageUpload.FileName;
					string filePath = Path.Combine(uploadsDir, imageName);
					FileStream fs = new FileStream(filePath, FileMode.Create);
					await asset.ImageUpload.CopyToAsync(fs);
					fs.Close();
					asset.Image = imageName;
				}

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
					string pdffile = asset.RequistionpackUpload.FileName;
					using (FileStream fileStream = System.IO.File.Create(fileName))
					{
						asset.RequistionpackUpload.CopyTo(fileStream);
						fileStream.Flush();
					}
					asset.Requistion_pack = pdffile;
				}

				_context.Update(asset);
				_context.Entry(asset).Property(u => u.PresentLocationId).IsModified = false;
				_context.Entry(asset).Property(u => u.CheckId).IsModified = false;
				_context.Entry(asset).Property(u => u.Present_user).IsModified = false;
				_context.Entry(asset).Property(u => u.Image).IsModified = false;
				await _context.SaveChangesAsync();
				TempData["Success"] = "The Asset has been edited";

				return RedirectToAction(nameof(Index));
			}
			ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state", asset.AssetStateId);
			//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
			ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
			ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
			ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			return View(asset);
		}

		public async Task<IActionResult> AssetVerification(int? id)
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
			ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
			ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
			ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
			//ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			ViewBag.VendorId = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			return View(asset);
		}

		// POST: Asset/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssetVerification(int id, [FromServices] IWebHostEnvironment hostingEnvironment, Asset asset)
		{
			ViewData["AssetStateId"] = new SelectList(_context.Asset_States, "Id", "Asset_state", asset.AssetStateId);
			//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department_name", asset.DepartmentId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.LocationId);
			ViewData["CheckId"] = new SelectList(_context.Physical_Checks, "Id", "Check_name", asset.CheckId);
			ViewData["PresentLocationId"] = new SelectList(_context.Locations, "Id", "Location_name", asset.PresentLocationId);
			ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Vendor_name", asset.VendorId);
			//if (id != asset.Id)
			//{
			//	return NotFound();
			//}
			ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Category_name", asset.CategoryId);

			var assettag = await _context.Assets.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Asset_tag == asset.Asset_tag);
			if (assettag != null && asset.Asset_tag != null)
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
				string imageName = Guid.NewGuid().ToString() + "_" + asset.ImageUpload.FileName;
				string filePath = Path.Combine(uploadsDir, imageName);
				FileStream fs = new FileStream(filePath, FileMode.Create);
				await asset.ImageUpload.CopyToAsync(fs);
				fs.Close();
				asset.Image = imageName;
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
			_context.Assets.Attach(asset);
			var entry = _context.Entry(asset);
			entry.State = EntityState.Unchanged;
			entry.Property(p => p.PresentLocationId).IsModified = true;
			entry.Property(p => p.CheckId).IsModified = true;
			entry.Property(p => p.Image).IsModified = true;
			entry.Property(p => p.Present_user).IsModified = true;
			entry.Property(p => p.Check_date).IsModified = true;

			//_context.Entry(asset).Property(u => u.PresentLocationId).IsModified = false;
			//_context.Entry(asset).Property(u => u.CheckId).IsModified = false;
			await _context.SaveChangesAsync();
			TempData["Success"] = "The Asset has been verified";

			return RedirectToAction(nameof(Index));
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
				if (!string.Equals(asset.Requistion_pack, "norequisitionpack.pdf"))
				{
					string oldPdfPath = Path.Combine(fileName, asset.Requistion_pack);
					if (System.IO.File.Exists(oldPdfPath))
					{
						System.IO.File.Delete(oldPdfPath);
					}
				}
				_context.Assets.Remove(asset);
				await _context.SaveChangesAsync();
				TempData["Success"] = "The Asset has been deleted";
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

		public IActionResult ExportToExcel()
		{
			using (var workbook = new XLWorkbook())
			{
				var assets = _context.Assets.Include(c => c.Category).Include(c => c.Location).Include(c => c.Department)
					.Include(c => c.Asset_State).Include(c => c.Vendor).Include(c => c.Physical_check)
					.ToList();
				var worksheet = workbook.Worksheets.Add("Assets");
				var currentRow = 1;
				worksheet.Cell(currentRow, 1).Value = "Asset Name";
				worksheet.Cell(currentRow, 2).Value = "Asset Description";
				worksheet.Cell(currentRow, 3).Value = "Category";
				worksheet.Cell(currentRow, 4).Value = "Location";
				worksheet.Cell(currentRow, 5).Value = "Department";
				worksheet.Cell(currentRow, 6).Value = "Asset State";
				worksheet.Cell(currentRow, 7).Value = "Asset Tag";
				worksheet.Cell(currentRow, 8).Value = "Service Tag";
				worksheet.Cell(currentRow, 9).Value = "Assigned User";
				worksheet.Cell(currentRow, 10).Value = "Purchased Price";
				worksheet.Cell(currentRow, 11).Value = "Supplier";
				worksheet.Cell(currentRow, 12).Value = "Delivery Date";
				worksheet.Cell(currentRow, 13).Value = "Physical Check";
				worksheet.Cell(currentRow, 14).Value = "Check Date";
				worksheet.Cell(currentRow, 15).Value = "Present Location";
				worksheet.Cell(currentRow, 16).Value = "Present User";
				foreach (var user in assets)
				{
					currentRow++;
					worksheet.Cell(currentRow, 1).Value = user.Asset_name;
					worksheet.Cell(currentRow, 2).Value = user.Asset_description;
					worksheet.Cell(currentRow, 3).Value = user.Category.Category_name;
					worksheet.Cell(currentRow, 4).Value = user.Location.Location_name;
					worksheet.Cell(currentRow, 5).Value = user.Department.Department_name;
					worksheet.Cell(currentRow, 6).Value = user.Asset_State.Asset_state;
					worksheet.Cell(currentRow, 7).Value = user.Asset_tag;
					worksheet.Cell(currentRow, 8).Value = user.Service_tag;
					worksheet.Cell(currentRow, 9).Value = user.Assigned_to;
					worksheet.Cell(currentRow, 10).Value = user.Purchased_price;
					worksheet.Cell(currentRow, 11).Value = user.Vendor.Vendor_name;
					worksheet.Cell(currentRow, 12).Value = user.Delivery_date;
					worksheet.Cell(currentRow, 13).Value = user.Physical_check.Check_name;
					worksheet.Cell(currentRow, 14).Value = user.Check_date;
					worksheet.Cell(currentRow, 15).Value = user.Present_location.Location_name;
					worksheet.Cell(currentRow, 16).Value = user.Present_user;
				}

				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();

					return File(
						content,
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Assets.xlsx");
				}
			}
		}

		public ActionResult Filter(DateTime start, DateTime end)
		{
			var st = start;
			var en = end;

			var filterAsset = _context.Assets.Include(a => a.Asset_State)
					.Include(a => a.Category)
					.Include(a => a.Department)
					.Include(a => a.Location)
					.Include(a => a.Present_location)
					.Include(a => a.Vendor)
					.Include(a => a.Physical_check)
					.Where(x => x.Delivery_date >= start && x.Delivery_date <= end).ToList();
			ViewBag.START = st;
			ViewBag.END = en;

			//var filterTicket = _context.Assets
			//.Where(x => x.Delivery_date >= start && x.Delivery_date <= end).ToList();
			return View(filterAsset);
		}

		public IActionResult EmailExport()
		{
			byte[] bytes = null;
			using (var workbook = new XLWorkbook())
			{
				var assets = _context.Assets.Include(a => a.Asset_State)
				.Include(a => a.Category)
				.Include(a => a.Department)
				.Include(a => a.Location)
				.Include(a => a.Present_location)
				.Include(a => a.Vendor)
				.Include(a => a.Physical_check)
				.OrderByDescending(p => p.Delivery_date).Where(x => x.Asset_tag == null).ToList();

				var worksheet = workbook.Worksheets.Add("Assets");
				var currentRow = 1;
				worksheet.Cell(currentRow, 1).Value = "Asset Name";
				worksheet.Cell(currentRow, 2).Value = "Category";
				worksheet.Cell(currentRow, 3).Value = "Location";
				worksheet.Cell(currentRow, 4).Value = "Department";
				worksheet.Cell(currentRow, 5).Value = "Asset State";
				worksheet.Cell(currentRow, 6).Value = "Asset Tag";
				worksheet.Cell(currentRow, 7).Value = "Service Tag";
				worksheet.Cell(currentRow, 8).Value = "Assigned User";
				worksheet.Cell(currentRow, 9).Value = "Purchased Price";
				worksheet.Cell(currentRow, 10).Value = "Supplier";
				worksheet.Cell(currentRow, 11).Value = "Delivery Date";
				foreach (var user in assets)
				{
					currentRow++;
					worksheet.Cell(currentRow, 1).Value = user.Asset_name;
					worksheet.Cell(currentRow, 2).Value = user.Category.Category_name;
					worksheet.Cell(currentRow, 3).Value = user.Location.Location_name;
					worksheet.Cell(currentRow, 4).Value = user.Department.Department_name;
					worksheet.Cell(currentRow, 5).Value = user.Asset_State.Asset_state;
					worksheet.Cell(currentRow, 6).Value = user.Asset_tag;
					worksheet.Cell(currentRow, 7).Value = user.Service_tag;
					worksheet.Cell(currentRow, 8).Value = user.Assigned_to;
					worksheet.Cell(currentRow, 9).Value = user.Purchased_price;
					worksheet.Cell(currentRow, 10).Value = user.Vendor.Vendor_name;
					worksheet.Cell(currentRow, 11).Value = user.Delivery_date;
				}

				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					bytes = stream.ToArray();
					var message = new MimeMessage();
					message.From.Add(new MailboxAddress("GMT Asset Tracker", "i_zzyfizzy@live.com"));
					message.To.Add(new MailboxAddress("fisayoadegun@gmail.com"));
					message.Subject = "Assets with no asset tag";
					

					var builder = new BodyBuilder();
					builder.HtmlBody = string.Format("<h3 style='color:black;'>Kindly find attached an excel sheet of assets without asset tags. <hr /></h3>");
					//message.Body = new BodyBuilder { HtmlBody = string.Format("<h3 style='color:black;'>Kindly find attached an excel sheet of assets without asset tags. <hr /></h3>") }.ToMessageBody();
					//Attach Attachment to Email
					builder.Attachments.Add("Assets_With_No_Assettag.xlsx", bytes);
					message.Body = builder.ToMessageBody();

					using (var client = new SmtpClient())
					{
						client.Connect("smtp.office365.com", 587, false);
						client.Authenticate("i_zzyfizzy@live.com", "Surulere007");

						client.Send(message);
						client.Disconnect(true);
					}
					return View();
				}
			}
		}

		public IActionResult ExportToExcelfilter(DateTime start, DateTime end)
		{
			using (var workbook = new XLWorkbook())
			{
				var assets = _context.Assets.Include(c => c.Category).Include(c => c.Location).Include(c => c.Department)
					.Include(c => c.Asset_State).Include(c => c.Vendor).Include(c => c.Physical_check).Where(x => x.Delivery_date >= start && x.Delivery_date <= end)
					.ToList();

				var st = start;
				var en = end;
				ViewBag.START = st;
				ViewBag.END = en;
				var worksheet = workbook.Worksheets.Add("Assets");
				var currentRow = 1;
				worksheet.Cell(currentRow, 1).Value = "Asset Name";
				worksheet.Cell(currentRow, 2).Value = "Asset Description";
				worksheet.Cell(currentRow, 3).Value = "Category";
				worksheet.Cell(currentRow, 4).Value = "Location";
				worksheet.Cell(currentRow, 5).Value = "Department";
				worksheet.Cell(currentRow, 6).Value = "Asset State";
				worksheet.Cell(currentRow, 7).Value = "Asset Tag";
				worksheet.Cell(currentRow, 8).Value = "Service Tag";
				worksheet.Cell(currentRow, 9).Value = "Assigned User";
				worksheet.Cell(currentRow, 10).Value = "Purchased Price";
				worksheet.Cell(currentRow, 11).Value = "Supplier";
				worksheet.Cell(currentRow, 12).Value = "Delivery Date";
				worksheet.Cell(currentRow, 13).Value = "Physical Check";
				worksheet.Cell(currentRow, 14).Value = "Check Date";
				worksheet.Cell(currentRow, 15).Value = "Present Location";
				worksheet.Cell(currentRow, 16).Value = "Present User";
				foreach (var user in assets)
				{
					currentRow++;
					worksheet.Cell(currentRow, 1).Value = user.Asset_name;
					worksheet.Cell(currentRow, 2).Value = user.Asset_description;
					worksheet.Cell(currentRow, 3).Value = user.Category.Category_name;
					worksheet.Cell(currentRow, 4).Value = user.Location.Location_name;
					worksheet.Cell(currentRow, 5).Value = user.Department.Department_name;
					worksheet.Cell(currentRow, 6).Value = user.Asset_State.Asset_state;
					worksheet.Cell(currentRow, 7).Value = user.Asset_tag;
					worksheet.Cell(currentRow, 8).Value = user.Service_tag;
					worksheet.Cell(currentRow, 9).Value = user.Assigned_to;
					worksheet.Cell(currentRow, 10).Value = user.Purchased_price;
					worksheet.Cell(currentRow, 11).Value = user.Vendor.Vendor_name;
					worksheet.Cell(currentRow, 12).Value = user.Delivery_date;
					//worksheet.Cell(currentRow, 13).Value = user.Physical_check.Check_name;
					//worksheet.Cell(currentRow, 15).Value = user.Present_location.Location_name;
					if (user.Physical_check == null)
					{
						worksheet.Cell(currentRow, 13).Value = "";
					}
					else
					{
						worksheet.Cell(currentRow, 13).Value = user.Physical_check.Check_name;
					}

					worksheet.Cell(currentRow, 14).Value = user.Check_date;
					if (user.Present_location == null)
					{
						worksheet.Cell(currentRow, 15).Value = "";
					}
					else
					{
						worksheet.Cell(currentRow, 15).Value = user.Present_location.Location_name;
					}

					worksheet.Cell(currentRow, 16).Value = user.Present_user;
				}

				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();

					return File(
						content,
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Assets.xlsx");
				}
			}
		}
	}
}