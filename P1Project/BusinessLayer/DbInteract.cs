using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using RepositoryLayer;

namespace BusinessLayer
{
  public class DbInteract : IDbInteract
  {
    private readonly P1Db _context;

    // register the context in Startup.ConfigureServices(); dependency injection (instantiated when this class is instantiated)
    public DbInteract(P1Db context)
    {
      this._context = context;
    }

    public List<ItemOrderDetail> GetOrderDetails(int orderId)
    {
      var OrdersTable = _context.CustomerOrders;
      var OrderedItemsTable = _context.OrderedItems;
      var ItemsTable = _context.Items;
      var dbOrderDetails = (from o in OrdersTable
                            join oi in OrderedItemsTable on o.OrderId equals oi.OrderId
                            join i in ItemsTable on oi.ItemId equals i.ItemId
                            where o.OrderId == orderId
                            select new
                            {
                              i.ItemName,
                              i.ItemPrice,
                              oi.QuantityOrdered
                            }).ToList();

      List<ItemOrderDetail> orderDetails = new();
      foreach (var item in dbOrderDetails)
        orderDetails.Add(new ItemOrderDetail() 
        {
          ItemName = item.ItemName,
          QuantityOrdered = item.QuantityOrdered,
          ItemCost = (item.QuantityOrdered * item.ItemPrice)
        });
      return orderDetails;
    }

    public List<StoreOrderDisplay> GetStoreOrderHistory(int storeId)
    {
      var StoresTable = _context.Stores;
      var OrdersTable = _context.CustomerOrders;
      var CustomersTable = _context.Customers;
      var dbCustomerOrders = (from o in OrdersTable
                              join s in StoresTable on o.StoreId equals s.StoreId
                              join c in CustomersTable on o.CustomerId equals c.CustomerId
                              where o.StoreId == storeId
                              select new
                              {
                                o.OrderId,
                                o.OrderDate,
                                o.OrderCost,
                                c.Username,
                                s.StoreLocation
                              }).ToList();

      List<StoreOrderDisplay> ordersInformation = new();
      foreach (var order in dbCustomerOrders)
        ordersInformation.Add(new StoreOrderDisplay()
        {
          OrderId = order.OrderId,
          OrderDate = order.OrderDate,
          OrderCost = order.OrderCost,
          Username = order.Username,
          StoreLocation = order.StoreLocation
        });
      return ordersInformation;
    }

    public List<OrderDisplay> GetCustomerOrderHistory(int customerId)
    {
      var StoresTable = _context.Stores;
      var OrdersTable = _context.CustomerOrders;
      var CustomersTable = _context.Customers;
      var dbCustomerOrders = (from o in OrdersTable
                              join s in StoresTable on o.StoreId equals s.StoreId
                              join c in CustomersTable on o.CustomerId equals c.CustomerId
                              where o.CustomerId == customerId
                              select new 
                              {
                                o.OrderId,
                                o.OrderDate,
                                o.OrderCost,
                                s.StoreLocation
                              }).ToList();

      List<OrderDisplay> ordersInformation = new();
      foreach (var order in dbCustomerOrders)
        ordersInformation.Add(new OrderDisplay()
        {
          OrderId = order.OrderId,
          OrderDate = order.OrderDate,
          OrderCost = order.OrderCost,
          StoreLocation = order.StoreLocation
        });
      return ordersInformation;
    }

    public async Task<bool> InsertNewOrderedItemAsync(OrderedItemsModel orderedItem)
    {
      await _context.OrderedItems.AddAsync(orderedItem);

      try { await _context.SaveChangesAsync(); }
      catch (DbUpdateConcurrencyException exc)
      {
        // instead of WriteLine use Logging for exception
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }
      catch (DbUpdateException exc)
      {
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }

      return true;
    }

    public async Task<bool> CreateNewOrderAsync(CustomerOrderModel customerOrder)
    {
      await _context.CustomerOrders.AddAsync(customerOrder);

      try { await _context.SaveChangesAsync(); }
      catch (DbUpdateConcurrencyException exc)
      {
        // instead of WriteLine use Logging for exception
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }
      catch (DbUpdateException exc)
      {
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }

      return true;
    }

    /// <summary>
    /// Saves a new customer in the Db. If unsuccessful, returns FALSE, else return TRUE
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    public async Task<bool> RegisterNewCustomerAsync(CustomerModel customer)
    {
      var dbExistingCustomerResult = _context.Customers.Where(c => c.Username == customer.Username).ToList();
      if (dbExistingCustomerResult.Count != 0)
        return false;

      await _context.Customers.AddAsync(customer);

      try { await _context.SaveChangesAsync(); }
      catch (DbUpdateConcurrencyException exc)
      {
        // instead of WriteLine use Logging for exception
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }
      catch (DbUpdateException exc)
      {
        Console.WriteLine($"There was a problem updating the Db => {exc.InnerException}");
        return false;
      }

      return true;
    }

    public bool ValidateCustomer(string username, string password)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username && customer.Password == password).ToList();
      return (dbResult.Count == 1);
    }

    public int GetCustomerId(string username)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username)
                                       .Select(customer => new { customer.CustomerId }).ToList();
      if (dbResult.Count == 0)
        return -1;
      return dbResult[0].CustomerId;
    }

    public List<CustomerModel> GetCustomerSearchDetails(string userName = null, string firstName = null, string lastName = null)
    {
      dynamic dbSearchResults;
      var CustomerTable = _context.Customers;
      if (userName != null && firstName != null && lastName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.FirstName == firstName &&
                                                          customer.LastName == lastName).ToList();
      else if (userName != null && firstName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.FirstName == firstName).ToList();
      else if (userName != null && lastName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.LastName == lastName).ToList();
      else if (firstName != null && lastName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.FirstName == firstName &&
                                                          customer.LastName == lastName).ToList();
      else if (firstName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.FirstName == firstName).ToList();
      else if (lastName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.LastName == lastName).ToList();
      else if (userName != null)
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName).ToList();
      else
        dbSearchResults = new List<CustomerModel>();

      List<CustomerModel> searchedCustomers = new();
      foreach (var res in dbSearchResults)
        searchedCustomers.Add(new CustomerModel() {
            FirstName = res.FirstName,
            LastName = res.LastName,
            Username = res.Username,
            SignupDate = res.SignupDate,
            CustomerId = res.CustomerId
          });
      return searchedCustomers;
    }

    public CustomerModel GetCustomer(string userName = "", string passWord = "", int customerId = -1)
    {
      dynamic dbResults;
      if (userName != "" && passWord != "")
        dbResults = _context.Customers.Where(customer => customer.Username == userName && customer.Password == passWord).ToList();
      else
        dbResults = _context.Customers.Where(customer => customer.CustomerId == customerId).ToList();
      
      CustomerModel Customer = new();
      foreach (var res in dbResults)
        Customer = new()
        {
          CustomerId = res.CustomerId,
          FirstName = res.FirstName,
          LastName = res.LastName,
          Username = res.Username,
          Password = res.Password,
          SignupDate = res.SignupDate
        };
      return Customer;
    }

    public StoreModel GetStore(string storeLocation = "", int storeId = -1)
    {
      dynamic dbResults;
      if (storeLocation != "")
        dbResults = _context.Stores.Where(store => store.StoreLocation == storeLocation)
                                 .Select(store => new {
                                    store.StoreId, store.StoreLocation
                                 }).ToList();
      else
        dbResults = _context.Stores.Where(store => store.StoreId == storeId)
                                   .Select(store => new {
                                      store.StoreId,
                                      store.StoreLocation
                                   }).ToList();
      StoreModel Store = new();
      foreach (var res in dbResults)
        Store = new()
        {
          StoreId = res.StoreId,
          StoreLocation = res.StoreLocation
        };
      return Store;
    }

    public string GetStoreName(int storeId)
    {
      var dbResults = _context.Stores.Where(store => store.StoreId == storeId)
                                     .Select(store => new {
                                       store.StoreLocation
                                     }).ToList();
      foreach (var item in dbResults)
        return item.StoreLocation;
      return "";
    }

    public string GetItemName(int itemId)
    {
      var dbResults = _context.Items.Where(item => item.ItemId == itemId)
                                    .Select(item => new {
                                      item.ItemName
                                    }).ToList();
      foreach (var item in dbResults)
        return item.ItemName;
      return "";
    }

    public decimal GetItemPrice(int itemId)
    {
      var dbResults = _context.Items.Where(item => item.ItemId == itemId)
                                    .Select(item => new {
                                      item.ItemPrice
                                    }).ToList();
      foreach (var item in dbResults)
        return item.ItemPrice;
      return 0;
    }

    public List<string> GetStoreLocations()
    {
      var dbResults = _context.Stores;
      List<string> StoreLocations = new();
      foreach (var item in dbResults)
        StoreLocations.Add(item.StoreLocation);
      return StoreLocations;
    }

    public List<StoredItemDisplay> GetStoreInventory(int storeId)
    {
      var StoresTable = _context.Stores;
      var ItemsTable = _context.Items;
      var InventoryTable = _context.StoredItems;
      var dbStoreInventory = (from si in InventoryTable
                              join s in StoresTable on si.StoreId equals s.StoreId
                              join i in ItemsTable on si.ItemId equals i.ItemId
                              where s.StoreId == storeId
                              select new
                              {
                                i.ItemName,
                                ItemDesc = i.ItemDescription,
                                ItemPrice = Math.Round(i.ItemPrice, 4),
                                si.InStock,
                                i.ItemId
                              }).ToList();
      List<StoredItemDisplay> Inventory = new();
      foreach (var row in dbStoreInventory)
        Inventory.Add(new StoredItemDisplay()
        {
          ItemName = row.ItemName,
          ItemDescription = row.ItemDesc,
          ItemPrice = row.ItemPrice,
          InStock = row.InStock,
          ItemId = row.ItemId
        });
      return Inventory;
    }
  }
}
