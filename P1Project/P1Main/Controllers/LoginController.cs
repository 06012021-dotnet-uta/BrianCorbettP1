using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System.Web;

namespace P1Main.Controllers
{
  public class LoginController : Controller
  {
    private readonly IDbInteract _DbInteract;
    // create a constructor into which the business layer dependency will be injected (in Startup.cs)
    public LoginController(IDbInteract dbInteract)
    {
      this._DbInteract = dbInteract;
    }

    /// <summary>
    /// A view to display a form for a user to enter in their login information
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    // GET: LoginController/Create
    public ActionResult LoginCustomer()
    {
      return View();
    }

    /// <summary>
    /// A view to verify valid login credentials
    /// </summary>
    /// <param name="customerLogin"></param>
    /// <returns>The view object to display to the user</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult VerifyLoginCustomer(CustomerModel customerLogin)
    {
      string sanUsername = HttpUtility.HtmlEncode(customerLogin.Username);
      string sanPassword = HttpUtility.HtmlEncode(customerLogin.Password);
      bool SuccessfulVerification = _DbInteract.ValidateCustomer(sanUsername, sanPassword);
      if (SuccessfulVerification)
      {
        CustomerModel customer = _DbInteract.GetCustomer(sanUsername, sanPassword);
        HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
        HttpContext.Session.SetString("DefaultStore", _DbInteract.GetStoreName(customer.DefaultStoreId));
        return RedirectToAction("Index", "Home", customer);
      }
      else
        return RedirectToAction("LoginCustomer", "Login");
    }
  }
}
