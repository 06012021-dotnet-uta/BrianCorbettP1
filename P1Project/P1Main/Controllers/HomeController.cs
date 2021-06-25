using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;

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
