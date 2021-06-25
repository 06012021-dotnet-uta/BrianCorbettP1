using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System;
using System.Collections.Generic;

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

    public ActionResult SeeCustomerHistory(int customerId = -1)
    {
      List<OrderDisplay> orders = new();
      if (customerId == -1)
      {
        ViewBag.CustomerUsername = _DbInteract.GetCustomer(customerId: Int32.Parse(HttpContext.Session.GetInt32("CustomerId").ToString())).Username;
        orders = _DbInteract.GetCustomerOrderHistory(Int32.Parse(HttpContext.Session.GetInt32("CustomerId").ToString()));
      }
      else
      {
        ViewBag.CustomerUsername = _DbInteract.GetCustomer(customerId: customerId).Username;
        orders = _DbInteract.GetCustomerOrderHistory(customerId);
      }
      return View(orders);
    }

    public ActionResult SeeStoreHistory(string location)
    {
      ViewBag.StoreLocation = location;
      List<StoreOrderDisplay> orders = _DbInteract.GetStoreOrderHistory(Int32.Parse(HttpContext.Session.GetInt32("FocusStoreId").ToString()));
      return View(orders);
    }

    public ActionResult SeeOrderDetails(int orderId)
    {
      List<ItemOrderDetail> orderDetails = _DbInteract.GetOrderDetails(orderId);
      return View(orderDetails);
    }
  }
}
