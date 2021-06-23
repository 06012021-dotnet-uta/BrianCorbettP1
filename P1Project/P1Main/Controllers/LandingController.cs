using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1Main.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace P1Main.Controllers
{
  public class LandingController : Controller
  {
    private readonly ILogger<LandingController> _logger;

    public LandingController(ILogger<LandingController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
