using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
  public class StoredItemsModel
  {
    public StoredItemsModel() { }

    [Display(Name = "Item ID")]
    public int ItemId { get; set; }
    [ForeignKey("ItemId")]
    public ItemsModel Item { get; set; }
    [Display(Name = "Store ID")]
    public int StoreId { get; set; }
    [ForeignKey("StoreId")]
    public StoreModel Store { get; set; }
    [Required]
    [Display(Name = "In Stock")]
    public int InStock { get; set; } = 0;
  }
}
