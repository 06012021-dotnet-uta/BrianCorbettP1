using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class OrderedItemsModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public OrderedItemsModel() { }

    [Display(Name = "Order ID")]
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public CustomerOrderModel CustomerOrder { get; set; }
    [Display(Name = "Item ID")]
    public int ItemId { get; set; }
    [ForeignKey("ItemId")]
    public ItemsModel Item { get; set; }
    [Required] [Display(Name = "Quantity ordered")]
    public int QuantityOrdered { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is OrderedItemsModel)
      {
        var that = obj as OrderedItemsModel;
        return (this.OrderId == that.OrderId &&
          this.ItemId == that.ItemId &&
          this.QuantityOrdered == that.QuantityOrdered);
      }
      return false;
    }
  }
}