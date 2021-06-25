using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
  public class CustomerOrderModel
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
  }
}
