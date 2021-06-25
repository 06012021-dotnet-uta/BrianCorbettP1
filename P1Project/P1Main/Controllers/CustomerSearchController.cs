using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System.Collections.Generic;
using BusinessLayer;

namespace P1Main.Controllers
{
  public class CustomerSearchController : Controller
  {
    private readonly IDbInteract _DbInteract; //! might need to change this DbInteract after testing
    public CustomerSearchController(IDbInteract dbInteract) // dependency injection
    {
      this._DbInteract = dbInteract;
    }

    // GET: CustomerSearch/Search
    public ActionResult Search() // has views
    {
      return View();
    }

    public ActionResult DisplayCustomerDetails(CustomerModel customer)
    {
      List<CustomerModel> SearchedCustomers = _DbInteract.GetCustomerSearchDetails(
        userName: customer.Username, firstName: customer.FirstName, lastName: customer.LastName);
      return View(SearchedCustomers);
    }
  }
}
