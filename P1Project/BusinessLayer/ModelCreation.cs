using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary;

namespace BusinessLayer
{
  public static class ModelCreation
  {
    /// <summary>
    /// Creates a CustomerOrder object
    /// </summary>
    /// <param name="orderCost">The cost of an order</param>
    /// <param name="customerId">The CustomerId of the customer who placed the order</param>
    /// <param name="storeId">The StoreId of the store the order was made from</param>
    /// <returns>A CustomerOrder object representing the newly made order</returns>
    public static CustomerOrderModel CreateCustomerOrder(decimal orderCost, int customerId, int storeId)
    {
      return new CustomerOrderModel() 
      {
        OrderCost = orderCost,
        CustomerId = customerId,
        StoreId = storeId
      }; 
    }

    /// <summary>
    /// Creates an OrderedItem object
    /// </summary>
    /// <param name="orderId">The OrderId of the order the item belongs to</param>
    /// <param name="itemId">The ItemId of the item being ordered</param>
    /// <param name="quantityOrdered">The quantity of the item being ordered</param>
    /// <returns>A OrderItem object that represents the item being ordered</returns>
    public static OrderedItemsModel CreateOrderedItem(int orderId, int itemId, int quantityOrdered)
    {
      return new OrderedItemsModel()
      {
        OrderId = orderId,
        ItemId = itemId,
        QuantityOrdered = quantityOrdered
      };
    }

    /// <summary>
    /// Creates a CheckoutItem object
    /// </summary>
    /// <param name="itemId">The ItemId of the item being checked-out</param>
    /// <param name="itemName">The ItemName of the item being checked-out</param>
    /// <param name="inStock">The amount in stock of the item</param>
    /// <returns></returns>
    public static CheckoutItemDetails CreateCheckoutItem(int itemId, string itemName, int inStock)
    {
      return new CheckoutItemDetails()
      {
        ItemId = itemId,
        ItemName = itemName,
        InStock = inStock
      };
    }
  }
}
