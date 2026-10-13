using System.Diagnostics;
using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GLMSDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            GLMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel
            {
                TotalClients = _context.Clients.Count(),

                TotalContracts = _context.Contracts.Count(),

                ActiveContracts = _context.Contracts
                    .Count(c => c.Status == "Active"),

                ExpiredContracts = _context.Contracts
                    .Count(c => c.Status == "Expired"),

                TotalServiceRequests = _context.ServiceRequests.Count()
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                });
        }
    }
}