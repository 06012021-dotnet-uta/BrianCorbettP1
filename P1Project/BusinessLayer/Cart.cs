using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer;

namespace BusinessLayer
{
  public static class Cart
  {
    public static List<List<int>> InCartItems { get; set; } = new List<List<int>>();
    //private static readonly DbInteract _DbInteract = IDbInteract dbIntreat;
    public static int focusStoreId { get; set; }

    public static decimal CartTotal(IDbInteract _DbInteract)
    {
      decimal total = 0;
      foreach (var item in DisplayCart(_DbInteract))
      {
        total += Decimal.Parse(item[3].ToString());
      }
      return total;
    }

    public static void AddToCart(int itemId, int storeId, int quantity)
    {
      InCartItems.Add(new List<int>() { itemId, storeId, quantity });
    }

    public static List<List<dynamic>> DisplayCart(IDbInteract _DbInteract)
    {
      List<List<dynamic>> stringCart = new();
      foreach (var item in InCartItems)
      {
        string displayName = _DbInteract.GetItemName(item[0]);
        decimal orderPrice = Decimal.Parse(_DbInteract.GetItemPrice(item[0]).ToString()) * item[2];
        stringCart.Add(new List<dynamic>() { displayName, item[1], item[2], orderPrice });
      }
      return stringCart;
    }

    public static void EmptyCart()
    {
      InCartItems = new();
      focusStoreId = -1;
    }
  }
}
