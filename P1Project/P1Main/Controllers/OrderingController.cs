using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using System;
using System.Collections.Generic;
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

    /// <summary>
    /// A view to display the stores that can be accessed
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    // GET: OrderingController
    public ActionResult SelectStore()
    {
      List<string> StoreLocations = _DbInteract.GetStoreLocations();
      return View(StoreLocations);
    }

    /// <summary>
    /// A view to display the options that can be made for a store
    /// </summary>
    /// <param name="storeName">The location name of the store to display options for</param>
    /// <returns>The view object to display to the user</returns>
    public ActionResult StoreOptions(string storeName)
    {
      StoreModel Store = _DbInteract.GetStore(storeName);
      HttpContext.Session.SetInt32("FocusStoreId", Store.StoreId);
      Cart.focusStoreId = (int)Store.StoreId;
      ViewBag.StoreLocation = storeName;
      return View(Store);
    }

    /// <summary>
    /// A view to display the inventory of the focused store
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    public ActionResult Inventory()
    {
      List<StoredItemDisplay> StoreInventory = _DbInteract.GetStoreInventory(Int32.Parse(HttpContext.Session.GetInt32("FocusStoreId").ToString()));
      return View(StoreInventory);
    }

    /// <summary>
    /// A view to allow a customer to specify the quantity of an item to order
    /// </summary>
    /// <param name="id">The ItemId of the item being ordered</param>
    /// <param name="name">The ItemName of the item being ordered</param>
    /// <param name="inStock">The quantity in stock of the item being ordered</param>
    /// <returns>The view object to display to the user</returns>
    public ActionResult SpecifyQuantity(int id, string name, int inStock)
    {
      CheckoutItemDetails checkoutItemDetails = ModelCreation.CreateCheckoutItem(id, name, inStock);
      ViewBag.InStock = _DbInteract.GetStoredItemQuantity(id, Cart.focusStoreId);
      TempData["OrderItemId"] = id;
      return View(checkoutItemDetails);
    }

    /// <summary>
    /// A view to display to the user their shopping cart
    /// </summary>
    /// <param name="qty">The quantity of an item the user is ordering</param>
    /// <returns>The view object to display to the user</returns>
    public ActionResult SeeCart(int qty = -1)
    {
      if (qty == -1)
      {
        ViewBag.CartTotal = Cart.CartTotal(_DbInteract);
        return View(Cart.DisplayCart(_DbInteract));
      }
      else
      {
        _DbInteract.DecreaseInStock((int)TempData["OrderItemId"], Int32.Parse(HttpContext.Session.GetInt32("FocusStoreId").ToString()), qty);
        CartItem CartItem = new() { ItemId = (int)TempData["OrderItemId"], StoreId = (int)Cart.focusStoreId, Quantity = qty };
        Cart.AddToCart(CartItem);
        ViewBag.CartTotal = Cart.CartTotal(_DbInteract);
        return View(Cart.DisplayCart(_DbInteract));
      }
    }

    /// <summary>
    /// A view that confirms whether the order was checkedout successfully
    /// </summary>
    /// <returns>The view object to display to the user</returns>
    public async Task<ActionResult> Checkout()
    {
      CustomerOrderModel NewCustomerOrder = ModelCreation.CreateCustomerOrder(
        Cart.CartTotal(_DbInteract), Int32.Parse(HttpContext.Session.GetInt32("CustomerId").ToString()), (int)Cart.focusStoreId);
      
      bool SuccessfulOrderCreation = await _DbInteract.CreateNewOrderAsync(NewCustomerOrder);
      if (SuccessfulOrderCreation) 
      {
        foreach (var item in Cart.InCartItems)
        {
          OrderedItemsModel NewOrderedItem = ModelCreation.CreateOrderedItem(NewCustomerOrder.OrderId, item.ItemId, item.Quantity);
          bool SuccessfulOrderedItemInsertion = await _DbInteract.InsertNewOrderedItemAsync(NewOrderedItem);
          if (!SuccessfulOrderedItemInsertion)
            return View("Error", "Order creation failed.");
        }
        Cart.EmptyCart();
        return View();
      }
      return View("Error", "Order creation failed.");
    }
  }
}
