using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary;
using RepositoryLayer;

namespace BusinessLayer
{
  public static class Cart
  {
    public static List<CartItem> InCartItems { get; set; } = new List<CartItem>();
    public static int focusStoreId { get; set; }

    /// <summary>
    /// Calculates the total of all the prices of items in the cart
    /// </summary>
    /// <param name="_DbInteract">A DbInteract object connected to the database in use</param>
    /// <returns>A decimal representing the cart total</returns>
    public static decimal CartTotal(IDbInteract _DbInteract)
    {
      decimal total = 0;
      foreach (var item in DisplayCart(_DbInteract))
        total += item.OrderPrice;
      return total;
    }

    /// <summary>
    /// Adds an item to the cart
    /// </summary>
    /// <param name="cartItem">The CartItem object to add to the cart</param>
    public static void AddToCart(CartItem cartItem)
    {
      InCartItems.Add(cartItem);
    }

    /// <summary>
    /// Converts each item in the cart into a CartItemDisplay object to display to the user
    /// </summary>
    /// <param name="_DbInteract">A DbInteract object connected to the database in use</param>
    /// <returns>A list of CartItemDisplay object that contain displayable information of each item in the cart</returns>
    public static List<CartItemDisplay> DisplayCart(IDbInteract _DbInteract)
    {
      List<CartItemDisplay> stringCart = new();
      foreach (var item in InCartItems)
      {
        string displayName = _DbInteract.GetItemName(item.ItemId);
        decimal orderPrice = Decimal.Parse(_DbInteract.GetItemPrice(item.ItemId).ToString()) * item.Quantity;
        stringCart.Add(new CartItemDisplay() { DisplayName = displayName, Quantity = item.Quantity, OrderPrice = orderPrice });
      }
      return stringCart;
    }

    /// <summary>
    /// Empties out the cart by reassigning a new list to the InCartItems property
    /// </summary>
    public static void EmptyCart()
    {
      InCartItems = new();
      focusStoreId = -1;
    }
  }
}
