using Microsoft.AspNetCore.Mvc;
using NextIssueWeb.Models;
using System.Diagnostics;

namespace NextIssueWeb.Controllers
{
    public class LoadingController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public LoadingController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Loading()
        {
            return View();
        }

    }
}
