using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
      List<List<string>> CustomerDetails = _DbInteract.GetCustomerSearchDetails(customer.Username, customer.FirstName, customer.LastName);
      return View(CustomerDetails);
    }
  }
}
