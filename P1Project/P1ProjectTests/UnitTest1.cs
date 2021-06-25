using Xunit;
using BusinessLayer;
using RepositoryLayer;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary;
using System.Threading.Tasks;
using System.Linq;

namespace P1ProjectTests
{
  public class UnitTest1
  {
    DbContextOptions<P1Db> options = new DbContextOptionsBuilder<P1Db>()
      .UseInMemoryDatabase(databaseName: "TestingDb").Options;

    [Fact]
    public async Task CreateNewOrderedItemInsertsItemCorrectly()
    {
      //Arrange
      OrderedItemsModel orderedItem = new()
      {
        OrderId = 1,
        ItemId = 1,
        QuantityOrdered = 1
      };

      //Act
      bool result = false;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        result = await dbInteract.InsertNewOrderedItemAsync(orderedItem);

        context.SaveChanges();
      }

      //Assert
      using (var context = new P1Db(options))
      {
        Assert.True(result);

        int ct = await context.OrderedItems.CountAsync();
        Assert.Equal(1, ct);

        var ordItem = context.OrderedItems.Where(co => co.OrderId == 1).FirstOrDefault();
        Assert.NotNull(ordItem);
        Assert.Contains(ordItem, context.OrderedItems);
      }
    }

    [Fact]
    public async Task CreateNewOrderInsertsOrderCorrectly()
    {
      //Arrange
      CustomerOrderModel customerOrder = new() 
      {
        OrderCost = 3.99M,
        CustomerId = 1,
        StoreId = 2
      };

      //Act
      bool result = false;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        result = await dbInteract.CreateNewOrderAsync(customerOrder);

        context.SaveChanges();
      }

      //Assert
      using(var context = new P1Db(options))
      {
        Assert.True(result);

        int ct = await context.CustomerOrders.CountAsync();
        Assert.Equal(1, ct);
        
        var custOrd = context.CustomerOrders.Where(co => co.CustomerId == 1).FirstOrDefault();
        Assert.NotNull(custOrd);
        Assert.Contains(custOrd, context.CustomerOrders);
      }
    }

    [Fact]
    public async Task RegisterCustomerInsertsCustomerCorrectly()
    {
      //Arrange
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };

      //Act
      bool result = false;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        result = await dbInteract.RegisterNewCustomerAsync(customer);

        context.SaveChanges();
      }

      //Assert
      using (var context = new P1Db(options))
      {
        Assert.True(result);

        int ct = await context.Customers.CountAsync();
        Assert.Equal(1, ct);

        var cust = context.Customers.Where(c => c.FirstName == "Ben").FirstOrDefault();
        Assert.NotNull(cust);
        Assert.Contains(cust, context.Customers);
      }
    }

    [Fact]
    public async void ValidateCustomerCorrectlyIdentifiesExistingCustomer()
    {
      //Arrange
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };
      
      //Act
      bool result1, result2;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        await dbInteract.RegisterNewCustomerAsync(customer);

        context.SaveChanges();

        result1 = dbInteract.ValidateCustomer(customer.Username, customer.Password);
        result2 = dbInteract.ValidateCustomer("username", "password");
      }

      //Assert
      Assert.True(result1);
      Assert.False(result2);
    }

    [Fact]
    public async void GetCustomerIdReturnsCorrectId()
    {
      //Arrange
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };

      //Act
      int result1;
      int result2;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        await dbInteract.RegisterNewCustomerAsync(customer);

        context.SaveChanges();

        result1 = dbInteract.GetCustomerId(customer.Username);
        result2 = dbInteract.GetCustomerId("username");
      }

      //Assert
      Assert.Equal(1, result1);
      Assert.Equal(-1, result2);
    }

    [Fact]
    public void CustomerSearchDetailsAreReturnedCorrectly()
    {
      //Arrange
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };

      //Act
      List<CustomerModel> resultDetails1;
      List<CustomerModel> resultDetails2;
      List<CustomerModel> resultDetails3;
      List<CustomerModel> resultDetails4;
      List<CustomerModel> resultDetails5;
      List<CustomerModel> resultDetails6;
      List<CustomerModel> resultDetails7;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Customers.Add(customer);

        context.SaveChanges();

        resultDetails1 = dbInteract.GetCustomerSearchDetails(userName: "baffleck");
        resultDetails2 = dbInteract.GetCustomerSearchDetails(firstName: "Ben");
        resultDetails3 = dbInteract.GetCustomerSearchDetails(lastName: "Affleck");
        resultDetails4 = dbInteract.GetCustomerSearchDetails(firstName: "Ben", lastName: "Affleck");
        resultDetails5 = dbInteract.GetCustomerSearchDetails(userName: "baffleck", lastName: "Affleck");
        resultDetails6 = dbInteract.GetCustomerSearchDetails(firstName: "Ben", userName: "baffleck");
        resultDetails7 = dbInteract.GetCustomerSearchDetails(firstName: "Ben", lastName: "Affleck", userName: "baffleck");
      }

      //Assert
      CustomerModel ExpectedResult = new() { FirstName = customer.FirstName, LastName = customer.LastName, Username = customer.Username, 
        SignupDate = customer.SignupDate, CustomerId = customer.CustomerId };
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails1);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails2);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails3);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails4);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails5);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails6);
      Assert.Equal(new List<CustomerModel>() { ExpectedResult }, resultDetails7);
    }

    [Fact]
    public async void GetCustomerReturnsCorrectAndValidObject()
    {
      //Arrange
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };

      //Act
      CustomerModel result1;
      CustomerModel result2;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        await dbInteract.RegisterNewCustomerAsync(customer);

        context.SaveChanges();

        result1 = dbInteract.GetCustomer(userName: customer.Username, passWord: customer.Password);
        result2 = dbInteract.GetCustomer(customerId: customer.CustomerId);
      }

      //Assert
      Assert.Equal(customer, result1);
      Assert.Equal(customer, result2);
    }

    [Fact]
    public void GetStoreReturnsCorrectAndValidObject()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };

      //Act
      StoreModel result1;
      StoreModel result2;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Stores.Add(store);

        context.SaveChanges();

        result1 = dbInteract.GetStore(storeLocation: store.StoreLocation);
        result2 = dbInteract.GetStore(storeId: store.StoreId);
      }

      //Assert
      Assert.Equal(store, result1);
      Assert.Equal(store, result2);
    }

    [Fact]
    public void GetStoreLocationsReturnsAllAndCorrectLocations()
    {
      //Arrange
      StoreModel store1 = new() { StoreLocation = "Tokyo" };
      StoreModel store2 = new() { StoreLocation = "Kansas City" };
      StoreModel store3 = new() { StoreLocation = "Dubai" };

      //Act
      List<string> result;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Stores.Add(store1);
        context.Stores.Add(store2);
        context.Stores.Add(store3);

        context.SaveChanges();

        result = dbInteract.GetStoreLocations();
      }

      //Assert
      Assert.Equal(new List<string>() { "Tokyo", "Kansas City", "Dubai" }, result);
    }

    [Fact]
    public void GetStoreInventoryReturnsCorrectListOfItems()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };
      ItemsModel item1 = new() { ItemName = "Backpack", ItemDescription = "Made out of snake skin", ItemPrice = 109.99M };
      ItemsModel item2 = new() { ItemName = "Hat", ItemDescription = "Blocks the sun from your face", ItemPrice = 23.99M };
      StoredItemsModel storedItem1;
      StoredItemsModel storedItem2;

      //Act
      List<StoredItemDisplay> result;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Stores.Add(store);
        context.Items.Add(item1);
        context.Items.Add(item2);
        storedItem1 = new() { ItemId = item1.ItemId, StoreId = store.StoreId, InStock = 40 };
        storedItem2 = new() { ItemId = item2.ItemId, StoreId = store.StoreId, InStock = 30 };
        context.StoredItems.Add(storedItem1);
        context.StoredItems.Add(storedItem2);

        context.SaveChanges();

        result = dbInteract.GetStoreInventory(store.StoreId);
      }

      //Assert
      List<StoredItemDisplay> ExpectedResult = new()
      {
        new StoredItemDisplay() { ItemName = item1.ItemName, ItemDescription = item1.ItemDescription, ItemPrice = item1.ItemPrice, InStock = storedItem1.InStock, ItemId = item1.ItemId },
        new StoredItemDisplay() { ItemName = item2.ItemName, ItemDescription = item2.ItemDescription, ItemPrice = item2.ItemPrice, InStock = storedItem2.InStock, ItemId = item2.ItemId }
      };
      Assert.Equal(ExpectedResult, result);
    }

    [Fact]
    public void GetStoreNameReturnsCorrectName()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };

      //Act
      string result;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Stores.Add(store);

        context.SaveChanges();

        result = dbInteract.GetStoreName(store.StoreId);
      }

      //Assert
      Assert.Equal("Tokyo", result);
    }

    [Fact]
    public void GeItemNameReturnsCorrectName()
    {
      //Arrange
      ItemsModel item = new() { ItemName = "Backpack", ItemDescription = "Made out of snake skin", ItemPrice = 109.99M };

      //Act
      string result;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Items.Add(item);

        context.SaveChanges();

        result = dbInteract.GetItemName(item.ItemId);
      }

      //Assert
      Assert.Equal("Backpack", result);
    }

    [Fact]
    public void GeItemPriceReturnsCorrectPrice()
    {
      //Arrange
      ItemsModel item = new() { ItemName = "Backpack", ItemDescription = "Made out of snake skin", ItemPrice = 109.99M };

      //Act
      decimal result;
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Items.Add(item);

        context.SaveChanges();

        result = dbInteract.GetItemPrice(item.ItemId);
      }

      //Assert
      Assert.Equal(109.99M, result);
    }

    [Fact]
    public void GetCustomerOrderHistoryReturnsCorrectly()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };
      CustomerOrderModel customerOrder1 = new() { OrderCost = 3.99M, CustomerId = 1, StoreId = 1 };
      CustomerOrderModel customerOrder2 = new() { OrderCost = 47.49M, CustomerId = 1, StoreId = 1 };

      //Act
      List<OrderDisplay> result = new();
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Customers.Add(customer);
        context.Stores.Add(store);
        context.CustomerOrders.Add(customerOrder1);
        context.CustomerOrders.Add(customerOrder2);

        context.SaveChanges();

        result = dbInteract.GetCustomerOrderHistory(customer.CustomerId);
      }

      //Assert
      List<OrderDisplay> ExpectedResult = new()
      {
        new OrderDisplay() { OrderId = customerOrder1.OrderId, OrderDate = customerOrder1.OrderDate, OrderCost = customerOrder1.OrderCost, StoreLocation = store.StoreLocation },
        new OrderDisplay() { OrderId = customerOrder2.OrderId, OrderDate = customerOrder2.OrderDate, OrderCost = customerOrder2.OrderCost, StoreLocation = store.StoreLocation }
      };
      Assert.Equal(ExpectedResult, result);
    }

    [Fact]
    public void GetStoreOrderHistoryReturnsCorrectly()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };
      CustomerOrderModel customerOrder1 = new() { OrderCost = 3.99M, CustomerId = 1, StoreId = 1 };
      CustomerOrderModel customerOrder2 = new() { OrderCost = 47.49M, CustomerId = 1, StoreId = 1 };

      //Act
      List<StoreOrderDisplay> result = new();
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Customers.Add(customer);
        context.Stores.Add(store);
        context.CustomerOrders.Add(customerOrder1);
        context.CustomerOrders.Add(customerOrder2);

        context.SaveChanges();

        result = dbInteract.GetStoreOrderHistory(store.StoreId);
      }

      //Assert
      List<StoreOrderDisplay> ExpectedResult = new()
      {
        new StoreOrderDisplay() { OrderId = customerOrder1.OrderId, OrderDate = customerOrder1.OrderDate, OrderCost = customerOrder1.OrderCost, 
          StoreLocation = store.StoreLocation, Username = customer.Username },
        new StoreOrderDisplay() { OrderId = customerOrder2.OrderId, OrderDate = customerOrder2.OrderDate, OrderCost = customerOrder2.OrderCost, 
          StoreLocation = store.StoreLocation, Username = customer.Username }
      };
      Assert.Equal(ExpectedResult, result);
    }

    [Fact]
    public void GetOrderDetailsReturnsCorrectly()
    {
      //Arrange
      StoreModel store = new() { StoreLocation = "Tokyo" };
      CustomerModel customer = new()
      {
        FirstName = "Ben",
        LastName = "Affleck",
        Username = "baffleck",
        Password = "123"
      };
      ItemsModel item1 = new() { ItemName = "Backpack", ItemDescription = "Made out of leather", ItemPrice = 99.99M };
      ItemsModel item2 = new() { ItemName = "Baseball Bat", ItemDescription = "Made in the 1940's", ItemPrice = 43.99M };
      CustomerOrderModel customerOrder = new() { OrderCost = (item1.ItemPrice + item2.ItemPrice), CustomerId = customer.CustomerId, StoreId = store.StoreId };
      OrderedItemsModel orderedItem1 = new() { OrderId = 1, ItemId = 1, QuantityOrdered = 1 };
      OrderedItemsModel orderedItem2 = new() { OrderId = 1, ItemId = 2, QuantityOrdered = 1 };


      //Act
      List<ItemOrderDetail> result = new();
      using (var context = new P1Db(options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInteract dbInteract = new(context);
        context.Customers.Add(customer);
        context.Stores.Add(store);
        context.Items.Add(item1);
        context.Items.Add(item2);
        context.CustomerOrders.Add(customerOrder);
        context.OrderedItems.Add(orderedItem1);
        context.OrderedItems.Add(orderedItem2);

        context.SaveChanges();

        result = dbInteract.GetOrderDetails(customerOrder.OrderId);
      }

      //Assert
      List<ItemOrderDetail> ExpectedResult = new()
      {
        new ItemOrderDetail()
        {
          ItemName = item1.ItemName,
          ItemCost = (item1.ItemPrice * orderedItem1.QuantityOrdered),
          QuantityOrdered = orderedItem1.QuantityOrdered
        },
        new ItemOrderDetail()
        {
          ItemName = item2.ItemName,
          ItemCost = (item2.ItemPrice * orderedItem2.QuantityOrdered),
          QuantityOrdered = orderedItem2.QuantityOrdered
        }
      };
      Assert.Equal(ExpectedResult, result);
    }

    [Fact]
    public void AddToCartAppendsNewList()
    {
      //Arrange

      //Act
      Cart.AddToCart(1, 1, 1);

      //Assert
      Assert.Equal(new List<List<int>>() { new List<int>() { 1, 1, 1 } }, Cart.InCartItems);
    }

    [Fact]
    public void EmptyCartCreatesEmptyList()
    {
      //Arrange
      Cart.AddToCart(1, 2, 1);
      Cart.AddToCart(2, 3, 1);

      //Act
      Cart.EmptyCart();

      //Assert
      Assert.Equal(new List<List<int>>() { }, Cart.InCartItems);
    }
  }
}
