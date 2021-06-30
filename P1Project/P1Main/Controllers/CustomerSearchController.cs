using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System.Collections.Generic;
using BusinessLayer;
using System.Web;

namespace P1Main.Controllers
{
  public class CustomerSearchController : Controller
  {
    private readonly IDbInteract _DbInteract; //! might need to change this DbInteract after testing
    public CustomerSearchController(IDbInteract dbInteract) // dependency injection
    {
      this._DbInteract = dbInteract;
    }

    /// <summary>
    /// A view that allows the user to search for another user
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    // GET: CustomerSearch/Search
    public ActionResult Search() // has views
    {
      return View();
    }

    /// <summary>
    /// A view that displays a searched-for customer's details
    /// </summary>
    /// <param name="customer">The Customer object whose details to display</param>
    /// <returns>The view object to display to the user</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DisplayCustomerDetails(CustomerModel customer)
    {
      List<CustomerModel> SearchedCustomers = _DbInteract.GetCustomerSearchDetails(
        userName: HttpUtility.HtmlEncode(customer.Username), firstName: HttpUtility.HtmlEncode(customer.FirstName), lastName: HttpUtility.HtmlEncode(customer.LastName));
      return View(SearchedCustomers);
    }
  }
}
