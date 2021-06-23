using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
  public interface IDbInteract // this is created for testing the Db
  {
    Task<bool> RegisterNewCustomer(CustomerModel customer);
    Task<bool> CreateNewOrder(CustomerOrderModel customerOrder);
    Task<bool> InsertNewOrderedItem(OrderedItemsModel orderedItem);
    bool VerifyCustomer(string username, string password);
    int GetCustomerId(string username);
    List<List<string>> GetCustomerSearchDetails(string userName = "", string firstName = "", string lastName = "");
    CustomerModel GetCustomer(string userName="", string passWord="", int customerId=-1);
    StoreModel GetStore(string storeLocation = "", int storeId = -1);
    List<string> GetStoreLocations();
    List<List<dynamic>> GetStoreInventory(int storeId);
    string GetStoreName(int storeId);
    string GetItemName(int itemId);
    decimal GetItemPrice(int itemId);
  }
}
