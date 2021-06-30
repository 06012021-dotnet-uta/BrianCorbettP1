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

    /// <summary>
    /// Gets information about the specified order
    /// </summary>
    /// <param name="orderId">ID of the order to get information about</param>
    /// <returns>A list of ItemOrderDetail object that contain the information</returns>
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

    /// <summary>
    /// Gets the order history of the specified store
    /// </summary>
    /// <param name="storeId">The ID of the store to get the order history of</param>
    /// <returns>A list of StoreOrderDisplay objects that contain order information</returns>
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

    /// <summary>
    /// Gets the quantity in stock of a specific item at a specific store
    /// </summary>
    /// <param name="itemId">The ID of the item to get the quantity in stock of</param>
    /// <param name="storeId">The ID of the store that the item is stored in</param>
    /// <returns>An integer representing the quantity in stock of the item at the store</returns>
    public int GetStoredItemQuantity(int itemId, int storeId)
    {
      var InventoryTable = _context.StoredItems;
      var dbResults = (from i in InventoryTable
                       where i.StoreId == storeId &&
                             i.ItemId == itemId
                       select new
                       {
                         i.InStock
                       }).ToList();
      foreach (var res in dbResults)
        return res.InStock;
      return 0;
    }

    /// <summary>
    /// Decreases the InStock column in the StoredItems table, by the specified amount, of an item at a store
    /// </summary>
    /// <param name="itemId">The ID of the item whose quantity in stock will be decremented</param>
    /// <param name="storeId">The ID of the store where the item is stored</param>
    /// <param name="qty">The amount by which to decrement the quantity in stock</param>
    public void DecreaseInStock(int itemId, int storeId, int qty)
    {
      var InventoryTable = _context.StoredItems;
      var dbResults = InventoryTable.Where(i => i.ItemId == itemId && i.StoreId == storeId);
      foreach (var res in dbResults)
        res.InStock -= qty;
      _context.SaveChanges();
    }

    /// <summary>
    /// Gets the order history of a customer
    /// </summary>
    /// <param name="customerId">The ID of the customer to get the order history of</param>
    /// <returns>A list of OrderDisplay objects that contain the order details</returns>
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

    /// <summary>
    /// Inserts a new OrderedItem into the OrderedItems table
    /// </summary>
    /// <param name="orderedItem">The OrderedItem object to insert into the table</param>
    /// <returns>A bool representing whether or not the insert was successfully completeds</returns>
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

    /// <summary>
    /// Inserts a new Order to the CustomerOrders table
    /// </summary>
    /// <param name="customerOrder">The CustomerOrder object to insert into the table</param>
    /// <returns>A bool representing whether the insert was successfully completed</returns>
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
    /// Inserts a Customer into the Customers table
    /// </summary>
    /// <param name="customer">The Customer object to insert into the table</param>
    /// <returns>A bool representing whether the insert was successfully completed</returns>
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

    /// <summary>
    /// Checks the database for an existing entry with the same username and password
    /// </summary>
    /// <param name="username">The user inputted username</param>
    /// <param name="password">The user inputted password</param>
    /// <returns>A bool representing whether or not a match was found</returns>
    public bool ValidateCustomer(string username, string password)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username && customer.Password == password).ToList();
      return (dbResult.Count == 1);
    }

    /// <summary>
    /// Gets the CustomerId of the customer with a specified username
    /// </summary>
    /// <param name="username">The username to use in the database search</param>
    /// <returns>An integer representing the CustomerId of customer with the specified username</returns>
    public int GetCustomerId(string username)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username)
                                       .Select(customer => new { customer.CustomerId }).ToList();
      if (dbResult.Count == 0)
        return -1;
      return dbResult[0].CustomerId;
    }

    /// <summary>
    /// Gets all the customers in the database that match the user inputted fields
    /// </summary>
    /// <param name="userName">The username of a user being searched for</param>
    /// <param name="firstName">The first name of a user being searched for</param>
    /// <param name="lastName">The last name of a user being searched for</param>
    /// <returns>A list of CustomerModel objects that matched search criteria</returns>
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

    /// <summary>
    /// Gets a customer object from the database
    /// </summary>
    /// <param name="userName">The username of a user to get</param>
    /// <param name="passWord">The password of a user to get</param>
    /// <param name="customerId">The CustomerId of a user to get</param>
    /// <returns>A CustomerModel object that matches the search criteria</returns>
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
          SignupDate = res.SignupDate,
          DefaultStoreId = res.DefaultStoreId
        };
      return Customer;
    }

    /// <summary>
    /// Gets a Store object from the database 
    /// </summary>
    /// <param name="storeLocation">The location of the store to get</param>
    /// <param name="storeId">The StoreId of the store to get</param>
    /// <returns>A StoreModel object that matches</returns>
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

    /// <summary>
    /// Gets all the Store objects in the Stores table
    /// </summary>
    /// <returns>A list of all the StoreModel objects in the database</returns>
    public List<StoreModel> GetStores()
    {
      dynamic dbResults;
      dbResults = _context.Stores.Select(store => new {
                                   store.StoreId,
                                   store.StoreLocation
                                 }).ToList();
      List<StoreModel> dbStores = new();
      foreach (var store in dbResults)
        dbStores.Add(new StoreModel() { StoreId = store.StoreId, StoreLocation = store.StoreLocation });
      return dbStores;
    }

    /// <summary>
    /// Gets the location name of a specified store
    /// </summary>
    /// <param name="storeId">The StoreId of the store whose location name to get</param>
    /// <returns>A string representing the store location of the specified store</returns>
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

    /// <summary>
    /// Gets the name of an item in the database
    /// </summary>
    /// <param name="itemId">The ItemId of the item whose name to get</param>
    /// <returns>A string representing the name of the item</returns>
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

    /// <summary>
    /// Gets the price of an item in the database
    /// </summary>
    /// <param name="itemId">The ItemId whose price to get</param>
    /// <returns>A decimal representing the price of the item</returns>
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

    /// <summary>
    /// Gets all the store locations in the database
    /// </summary>
    /// <returns>A list of strings that represent all the store locations in the database</returns>
    public List<string> GetStoreLocations()
    {
      var dbResults = _context.Stores;
      List<string> StoreLocations = new();
      foreach (var item in dbResults)
        StoreLocations.Add(item.StoreLocation);
      return StoreLocations;
    }

    /// <summary>
    /// Gets the inventory of a specified store from the StoredItems table
    /// </summary>
    /// <param name="storeId">The StoreId of the store whose inventory to get</param>
    /// <returns>A list of StoredItemDisplay objects that contain the inventory details</returns>
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
