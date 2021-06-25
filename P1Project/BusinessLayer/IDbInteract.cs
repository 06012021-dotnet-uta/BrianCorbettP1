using ModelsLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer
{
  public interface IDbInteract // this is created for testing the Db
  {
    Task<bool> RegisterNewCustomerAsync(CustomerModel customer);
    Task<bool> CreateNewOrderAsync(CustomerOrderModel customerOrder);
    Task<bool> InsertNewOrderedItemAsync(OrderedItemsModel orderedItem);
    bool ValidateCustomer(string username, string password);
    int GetCustomerId(string username);
    CustomerModel GetCustomer(string userName="", string passWord="", int customerId=-1);
    StoreModel GetStore(string storeLocation = "", int storeId = -1);
    List<string> GetStoreLocations();
    List<StoredItemDisplay> GetStoreInventory(int storeId);
    string GetStoreName(int storeId);
    string GetItemName(int itemId);
    decimal GetItemPrice(int itemId);
    List<OrderDisplay> GetCustomerOrderHistory(int customerId);
    List<StoreOrderDisplay> GetStoreOrderHistory(int storeId);
    List<CustomerModel> GetCustomerSearchDetails(string userName = "", string firstName = "", string lastName = "");
    List<ItemOrderDetail> GetOrderDetails(int orderId);
    // ^^^ Passed ^^^
    // ^^^ Failed ^^^
    // ^^^ Untested ^^^
  }
}
