using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RepositoryLayer;
using System;

namespace P1Main.Controllers
{
  public class HomeController : Controller
  {
    private readonly IDbInteract _DbInteract;
    public HomeController(IDbInteract dbInteract)
    {
      this._DbInteract = dbInteract;
    }

    /// <summary>
    /// A view to display the home page
    /// </summary>
    /// <param name="customer">The Customer object of the customer who has signed in</param>
    /// <returns>The view object to display to the user</returns>
    // GET: HomeController
    public ActionResult Index(CustomerModel customer)
    {
      Cart.EmptyCart();
      ViewBag.CustomerId = HttpContext.Session.GetInt32("CustomerId");
      ViewBag.DefaultStore = _DbInteract.GetStoreName(_DbInteract.GetCustomer(customerId: customer.CustomerId).DefaultStoreId);
      return View(customer);
    }
  }
}
