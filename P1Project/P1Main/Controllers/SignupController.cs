using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System.Threading.Tasks;

namespace P1Main.Controllers
{
  public class SignupController : Controller // maybe change to AccountController and have signup and login here
  {
    private readonly IDbInteract _DbInteract; //! might need to change this DbInteract after testing
    public SignupController(IDbInteract dbInteract) // dependency injection
    {
      this._DbInteract = dbInteract;
    }

    // GET: SignupController/Create
    public ActionResult CreateCustomer() // has view [CreateCustomer.cshtml]
    {
      return View();
    }

    // GET: SignupController/Create/[id]
    [HttpPost] // because you're sending data
    public ActionResult VerifyCreateCustomer(CustomerModel customer) // has view [VerifyCreateCustomer.cshtml]
    {
      if (!ModelState.IsValid) // if the model passed in is not valid (it didn't get bound correctly somehow)
        RedirectToAction("CreateCustomer"); // redirects to the Create() action method
      return View(customer);
    }

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
