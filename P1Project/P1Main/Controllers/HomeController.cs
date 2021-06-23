using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1Main.Controllers
{
  public class HomeController : Controller
  {
    // GET: HomeController
    public ActionResult Index(CustomerModel customer)
    {
      Cart.EmptyCart();
      return View(customer);
    }
  }
}
