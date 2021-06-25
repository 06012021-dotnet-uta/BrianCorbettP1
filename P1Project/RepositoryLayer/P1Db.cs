using Microsoft.EntityFrameworkCore;
using ModelsLibrary;

namespace RepositoryLayer
{
  public class P1Db : DbContext
  {
    public DbSet<CustomerModel> Customers { get; set; }
    public DbSet<StoreModel> Stores { get; set; }
    public DbSet<ItemsModel> Items { get; set; }
    public DbSet<CustomerOrderModel> CustomerOrders { get; set; }
    public DbSet<OrderedItemsModel> OrderedItems { get; set; }
    public DbSet<StoredItemsModel> StoredItems { get; set; }

    //create constructor
    public P1Db() { }
    public P1Db(DbContextOptions options) : base(options) { }

    //override OnConfiguring()
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      // --- this is now done in Startup.ConfigureServices() when DbContext dependency is injected
      //if (!options.IsConfigured)
      //  options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CodeFirstP1Db;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<StoredItemsModel>().HasKey(sim => new { sim.ItemId, sim.StoreId });
      modelBuilder.Entity<OrderedItemsModel>().HasKey(oim => new { oim.ItemId, oim.OrderId });
    }
  }
}