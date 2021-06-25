using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
  public class OrderedItemsModel
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
  }
}