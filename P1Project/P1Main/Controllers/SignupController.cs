using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace P1Main.Controllers
{
  public class SignupController : Controller // maybe change to AccountController and have signup and login here
  {
    private readonly IDbInteract _DbInteract; //! might need to change this DbInteract after testing
    public SignupController(IDbInteract dbInteract) // dependency injection
    {
      this._DbInteract = dbInteract;
    }

    /// <summary>
    /// A view that allows a user to make a new account
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    // GET: SignupController/Create
    public ActionResult CreateCustomer() // has view [CreateCustomer.cshtml]
    {
      List<StoreModel> storeLocations = _DbInteract.GetStores();
      ViewBag.StoreLocations = storeLocations;
      return View();
    }

    /// <summary>
    /// A view that displays the user their information for review
    /// </summary>
    /// <param name="customer">The new Customer object for which an account is being made</param>
    /// <returns>The view object to display to the user</returns>
    // GET: SignupController/Create/[id]
    [HttpPost] // because you're sending data
    [ValidateAntiForgeryToken]
    public ActionResult VerifyCreateCustomer(CustomerModel customer) // has view [VerifyCreateCustomer.cshtml]
    {
      if (!ModelState.IsValid) // if the model passed in is not valid (it didn't get bound correctly somehow)
        RedirectToAction("CreateCustomer"); // redirects to the Create() action method
      ViewBag.StoreLocation = _DbInteract.GetStoreName(customer.DefaultStoreId);
      customer.Username = HttpUtility.HtmlEncode(customer.Username);
      customer.Password = HttpUtility.HtmlEncode(customer.Password);
      customer.FirstName = HttpUtility.HtmlEncode(customer.FirstName);
      customer.LastName = HttpUtility.HtmlEncode(customer.LastName);
      return View(customer);
    }

    /// <summary>
    /// A view that confirms whether the account information was successfully made and inserted into the database
    /// </summary>
    /// <param name="customer">A Customer object of the customer who is signing up</param>
    /// <returns>The view object to display to the user</returns>
    public async Task<ActionResult> SubmitNewCustomer(CustomerModel customer) // has view [SubmitNewCustomer.cshtml]
    {
      if (!ModelState.IsValid) // if the model passed in is not valid (it didn't get bound correctly somehow)
        RedirectToAction("CreateCustomer");

      bool SuccessfulRegistration = await _DbInteract.RegisterNewCustomerAsync(customer);
      if (SuccessfulRegistration)
      {
        ViewBag.RegSuccess = "You are registered!";
        return View();
      }
      else
        return View("Error", "Customer Db insert failed."); // Shared
    }
  }
}
