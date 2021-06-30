using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class CustomerOrderModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public virtual ICollection<OrderedItemsModel> OrderedItems { get; set; }
    public CustomerOrderModel()
    {
      this.OrderedItems = new HashSet<OrderedItemsModel>();
      this.OrderDate = DateTime.Now;
    }

    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Display(Name = "Order ID")]
    public int OrderId { get; set; }
    [Required] [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; }
    [Range(0, 9999.99)] [RegularExpression(@"^\d+\.\d{0,2}$")] [Required] [Display(Name = "Order Cost")]
    public decimal OrderCost { get; set; }
    [Display(Name = "CustomerId")]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public CustomerModel Customer { get; set; }
    [Display(Name = "StoreId")]
    public int StoreId { get; set; }
    [Required] [ForeignKey("StoreId")]
    public StoreModel Store { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is CustomerOrderModel)
      {
        var that = obj as CustomerOrderModel;
        return (this.OrderId == that.OrderId &&
          this.OrderDate == that.OrderDate &&
          this.OrderCost == that.OrderCost &&
          this.CustomerId == that.CustomerId &&
          this.StoreId == that.StoreId);
      }
      return false;
    }
  }
}
