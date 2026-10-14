using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GLMS.Web.Data;
using GLMS.Web.Models;

namespace GLMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly GLMSDbContext _context;

        public ContractsController(GLMSDbContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(
            string statusFilter,
            DateTime? startDate,
            DateTime? endDate)
        {
            var contracts = _context.Contracts
    .Include(c => c.Client)
    .OrderByDescending(c => c.ContractId)
    .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
            {
                contracts = contracts.Where(
                    c => c.Status == statusFilter);
            }

            if (startDate.HasValue)
            {
                contracts = contracts.Where(
                    c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                contracts = contracts.Where(
                    c => c.EndDate <= endDate.Value);
            }

            return View(await contracts.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(
                    m => m.ContractId == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "ClientId",
                "Name");

            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ContractId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath,ClientId")]
            Contract contract,
            IFormFile? agreementFile)
        {
            // Validate the uploaded file first.
            if (agreementFile != null && agreementFile.Length > 0)
            {
                var extension = Path.GetExtension(
                    agreementFile.FileName);

                if (!string.Equals(
                    extension,
                    ".pdf",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        "agreementFile",
                        "Only PDF files are allowed.");
                }
            }

            // Return to the form if validation fails.
            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = new SelectList(
                    _context.Clients,
                    "ClientId",
                    "Name",
                    contract.ClientId);

                return View(contract);
            }

            // Save the uploaded PDF, if one was provided.
            if (agreementFile != null && agreementFile.Length > 0)
            {
                // CHANGED: "wwwroot" -> "App_Data" so saving a file
                // does not trigger Hot Reload and restart the app.
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "App_Data",
                    "Uploads");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + ".pdf";

                var filePath = Path.Combine(
                    uploadsFolder,
                    fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await agreementFile.CopyToAsync(stream);
                }

                contract.AgreementFilePath = fileName;
            }

            // Save the contract to the database.
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "ClientId",
                "Name",
                contract.ClientId);

            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ContractId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath,ClientId")]
            Contract contract)
        {
            if (id != contract.ContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "ClientId",
                "Name",
                contract.ClientId);

            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(
                    m => m.ContractId == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Contracts/DownloadFile
        public IActionResult DownloadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            // Prevent directory traversal through the filename.
            if (Path.GetFileName(fileName) != fileName)
            {
                return BadRequest();
            }

            // CHANGED "wwwroot" -> "App_Data" to match the upload folder above.
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "App_Data",
                "Uploads",
                fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            return PhysicalFile(
                filePath,
                "application/pdf",
                fileName);
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(
                e => e.ContractId == id);
        }
    }
}