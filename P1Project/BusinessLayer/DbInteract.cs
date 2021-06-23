using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public async Task<bool> InsertNewOrderedItem(OrderedItemsModel orderedItem)
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

    public async Task<bool> CreateNewOrder(CustomerOrderModel customerOrder)
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
    public async Task<bool> RegisterNewCustomer(CustomerModel customer)
    {
      var dbExistingCustomerResult = _context.Customers.Where(c => c.Username == customer.Username).ToList();
      if (dbExistingCustomerResult.Count != 0)
        return false;

      await _context.Customers.AddAsync(customer);

      try
      {
        await _context.SaveChangesAsync();
      }
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

    public bool VerifyCustomer(string username, string password)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username && customer.Password == password).ToList();
      return (dbResult.Count == 1);
    }

    public int GetCustomerId(string username)
    {
      var dbResult = _context.Customers.Where(customer => customer.Username == username)
                                       .Select(customer => new { customer.CustomerId }).ToList();
      return dbResult[0].CustomerId;
    }

    public List<List<string>> GetCustomerSearchDetails(string userName="", string firstName="", string lastName="")
    {
      dynamic dbSearchResults;
      var CustomerTable = _context.Customers;
      if (userName != null && firstName != null && lastName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.FirstName == firstName &&
                                                          customer.LastName == lastName).ToList();
      }
      else if (userName != null && firstName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.FirstName == firstName).ToList();
      }
      else if (userName != null && lastName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName &&
                                                          customer.LastName == lastName).ToList();
      }
      else if (firstName != null && lastName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.FirstName == firstName &&
                                                          customer.LastName == lastName).ToList();
      }
      else if (firstName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.FirstName == firstName).ToList();
      }
      else if (lastName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.LastName == lastName).ToList();
      }
      else if (userName != null)
      {
        dbSearchResults = CustomerTable.Where(customer => customer.Username == userName).ToList();
      }
      else
      {
        dbSearchResults = new List<string>();
      }

      List<List<string>> DisplayStrings = new();
      foreach (var res in dbSearchResults)
      {
        DisplayStrings.Add(new List<string>() {
            res.FirstName, res.LastName, res.Username, res.SignupDate.ToString()
          });
      }

      return DisplayStrings;
    }

    public CustomerModel GetCustomer(string userName = "", string passWord = "", int customerId = -1)
    {
      dynamic dbResults;
      if (userName != "" && passWord != "")
      {
        dbResults = _context.Customers.Where(customer => customer.Username == userName && customer.Password == passWord).ToList();
      }
      else
      {
        dbResults = _context.Customers.Where(customer => customer.CustomerId == customerId).ToList();
      }
      
      CustomerModel Customer = new();
      foreach (var res in dbResults)
      {
        Customer = new()
        {
          CustomerId = res.CustomerId,
          FirstName = res.FirstName,
          LastName = res.LastName,
          Username = res.Username,
          Password = res.Password,
          SignupDate = res.SignupDate
        };
      }
      return Customer;
    }

    public StoreModel GetStore(string storeLocation = "", int storeId = -1)
    {
      dynamic dbResults;
      if (storeLocation != "")
      {
      dbResults = _context.Stores.Where(store => store.StoreLocation == storeLocation)
                                 .Select(store => new {
                                    store.StoreId, store.StoreLocation
                                 }).ToList();
      }
      else
      {
        dbResults = _context.Stores.Where(store => store.StoreId == storeId)
                                   .Select(store => new {
                                      store.StoreId,
                                      store.StoreLocation
                                   }).ToList();
      }
      StoreModel Store = new();
      foreach (var res in dbResults)
      {
        Store = new()
        {
          StoreId = res.StoreId,
          StoreLocation = res.StoreLocation
        };
      }
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
      {
        StoreLocations.Add(item.StoreLocation);
      }
      return StoreLocations;
    }

    public List<List<dynamic>> GetStoreInventory(int storeId)
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
      List<List<dynamic>> InventoryDisplayStrings = new();
      foreach (var row in dbStoreInventory)
        InventoryDisplayStrings.Add(new List<dynamic>()
        {
          row.ItemName, row.ItemDesc, row.ItemPrice, row.InStock, row.ItemId
        });
      return InventoryDisplayStrings;
    }
  }
}
