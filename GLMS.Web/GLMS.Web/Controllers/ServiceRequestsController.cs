using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using GLMS.Web.Factories;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly GLMSDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(
            GLMSDbContext context,
            CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var gLMSDbContext = _context.ServiceRequests
                .Include(s => s.Contract)
                .OrderByDescending(s => s.ServiceRequestId);

            return View(await gLMSDbContext.ToListAsync());
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.ServiceRequestId == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            var contracts = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    DisplayText = $"Contract {c.ContractId} - {c.Status} - {c.ServiceLevel}"
                })
                .ToList();

            ViewData["ContractId"] = new SelectList(
                contracts,
                "ContractId",
                "DisplayText");

            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ServiceRequestId,Description,UsdAmount,Cost,Status,ContractId")] ServiceRequest serviceRequest)
        {
            // Validate USD Amount
            if (serviceRequest.UsdAmount <= 0)
            {
                ModelState.AddModelError(
                    "UsdAmount",
                    "USD Amount must be greater than zero.");
            }

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(
                    c => c.ContractId == serviceRequest.ContractId);

            if (contract != null)
            {
                if (contract.Status == "Expired" ||
                    contract.Status == "On Hold")
                {
                    ModelState.AddModelError(
                        "",
                        "Service Requests cannot be created for Expired or On Hold contracts.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    decimal exchangeRate =
                        await _currencyService.GetUsdToZarRateAsync();

                    serviceRequest.Cost =
                        serviceRequest.UsdAmount * exchangeRate;
                }
                catch
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to retrieve the current exchange rate.");
                }
            }

            if (ModelState.IsValid)
            {
                var request = ServiceRequestFactory.Create(
                    serviceRequest.Description,
                    serviceRequest.UsdAmount,
                    serviceRequest.Cost,
                    serviceRequest.Status,
                    serviceRequest.ContractId);

                _context.Add(request);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var contracts = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    DisplayText =
                        $"Contract {c.ContractId} - {c.Status} - {c.ServiceLevel}"
                })
                .ToList();

            ViewData["ContractId"] = new SelectList(
                contracts,
                "ContractId",
                "DisplayText",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            var contracts = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    DisplayText = $"Contract {c.ContractId} - {c.Status} - {c.ServiceLevel}"
                })
                .ToList();

            ViewData["ContractId"] = new SelectList(
                contracts,
                "ContractId",
                "DisplayText",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ServiceRequestId,Description,UsdAmount,Cost,Status,ContractId")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return NotFound();
            }

            // Validate USD Amount
            if (serviceRequest.UsdAmount <= 0)
            {
                ModelState.AddModelError(
                    "UsdAmount",
                    "USD Amount must be greater than zero.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    decimal exchangeRate =
                        await _currencyService.GetUsdToZarRateAsync();

                    serviceRequest.Cost =
                        serviceRequest.UsdAmount * exchangeRate;

                    _context.Update(serviceRequest);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.ServiceRequestId))
                    {
                        return NotFound();
                    }

                    throw;
                }
                catch
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to retrieve the current exchange rate.");
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var contracts = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    DisplayText = $"Contract {c.ContractId} - {c.Status} - {c.ServiceLevel}"
                })
                .ToList();

            ViewData["ContractId"] = new SelectList(
                contracts,
                "ContractId",
                "DisplayText",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.ServiceRequestId == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.ServiceRequestId == id);
        }
    }
}