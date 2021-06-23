using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1Main.Controllers
{
  public class HistoryController : Controller
  {
    private readonly IDbInteract _DbInteract;
    // create a constructor into which the business layer dependency will be injected (in Startup.cs)
    public HistoryController(IDbInteract dbInteract)
    {
      this._DbInteract = dbInteract;
    }

    public ActionResult SeeCustomerHistory(int customerId)
    {
      return View();
    }

    public ActionResult SeeStoreHistory(int storeId)
    {
      return View();
    }
  }
}
