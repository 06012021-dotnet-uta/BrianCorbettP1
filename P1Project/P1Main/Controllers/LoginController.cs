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
  public class LoginController : Controller
  {
    private readonly IDbInteract _DbInteract;
    // create a constructor into which the business layer dependency will be injected (in Startup.cs)
    public LoginController(IDbInteract dbInteract)
    {
      this._DbInteract = dbInteract;
    }

    // GET: LoginController/Create
    public ActionResult LoginCustomer()
    {
      return View();
    }

    [HttpPost]
    public ActionResult VerifyLoginCustomer(CustomerModel customerLogin)
    {
      bool SuccessfulVerification = _DbInteract.VerifyCustomer(customerLogin.Username, customerLogin.Password);
      if (SuccessfulVerification)
      {
        CustomerModel customer = _DbInteract.GetCustomer(customerLogin.Username, customerLogin.Password);
        HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
        return RedirectToAction("Index", "Home", customer);
      }
      else
        return View("LoginCustomer");
    }
  }
}
