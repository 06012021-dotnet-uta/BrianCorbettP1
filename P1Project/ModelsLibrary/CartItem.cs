using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
  public class CartItem
  {
    public int ItemId { get; set; }  
    public int StoreId { get; set; }
    public int Quantity { get; set; }
  }

  public class CartItemDisplay
  {
    public string DisplayName { get; set; }
    public decimal OrderPrice { get; set; }
    public int Quantity { get; set; }
  }
}
