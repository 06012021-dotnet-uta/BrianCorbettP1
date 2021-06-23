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
  public class OrderingController : Controller
  {
    private readonly IDbInteract _DbInteract; //! might need to change this DbInteract after testing
    public OrderingController(IDbInteract dbInteract) // dependency injection
    {
      this._DbInteract = dbInteract;
    }

    // GET: OrderingController
    public ActionResult SelectStore()
    {
      List<string> StoreLocations = _DbInteract.GetStoreLocations();
      return View(StoreLocations);
    }

    public ActionResult StoreOptions(string storeName)
    {
      StoreModel Store = _DbInteract.GetStore(storeName);
      HttpContext.Session.SetInt32("FocusStoreId", Store.StoreId);
      Cart.focusStoreId = (int)Store.StoreId;
      return View(Store);
    }

    public ActionResult Inventory(/*int id*/)
    {
      //int storeId = id;
      List<List<dynamic>> StoreInventory = _DbInteract.GetStoreInventory(/*storeId*/(int)HttpContext.Session.GetInt32("CustomerId"));
      return View(StoreInventory);
    }
    
    public ActionResult SpecifyQuantity(int id, string name, int inStock)
    {
      int itemId = id;
      string itemName = name;
      int itemInStock = inStock;
      TempData["OrderItemId"] = itemId;
      TempData["OrderItemName"] = itemName;
      TempData["OrderItemInStock"] = itemInStock;
      return View();
    }

    public ActionResult SeeCart(int qty)
    {
      int quantity = qty;
      if (qty == 0)
      {
        return View(new List<List<dynamic>>());
      }
      else
      {
        Cart.AddToCart((int)TempData["OrderItemId"], (int)Cart.focusStoreId, quantity);
        ViewBag.CartTotal = Cart.CartTotal(_DbInteract);
        return View(Cart.DisplayCart(_DbInteract));
      }
    }

    public async Task<ActionResult> Checkout()
    {
      CustomerOrderModel NewCustomerOrder = new()
      {
        OrderDate = DateTime.Now,
        OrderCost = Cart.CartTotal(_DbInteract),
        CustomerId = Int32.Parse(HttpContext.Session.GetInt32("CustomerId").ToString()),
        StoreId = (int)Cart.focusStoreId
      };
      bool SuccessfulOrderCreation = await _DbInteract.CreateNewOrder(NewCustomerOrder);
      if (SuccessfulOrderCreation) 
      {
        foreach (var item in Cart.InCartItems)
        {
          OrderedItemsModel NewOrderedItem = new()
          {
            OrderId = NewCustomerOrder.OrderId,
            ItemId = item[0],
            QuantityOrdered = item[2]
          };
          bool SuccessfulOrderedItemInsertion = await _DbInteract.InsertNewOrderedItem(NewOrderedItem);
          if (!SuccessfulOrderedItemInsertion)
          {
            return View("Error", "Order creation failed.");
          }
        }
        Cart.EmptyCart();
        return View();
      }
      return View("Error", "Order creation failed.");
    }

    public ActionResult StoreOrderHistory(string storeName)
    {
      // get details of CustomerOrders & OrderedItems with storeId
      return View();
    }

  }
}
