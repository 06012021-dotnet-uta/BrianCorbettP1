using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
  public class ItemsModel
  {
    public virtual ICollection<OrderedItemsModel> OrderedItems { get; set; }
    public virtual ICollection<StoredItemsModel> StoredItems { get; set; }
    public ItemsModel()
    {
      this.OrderedItems = new HashSet<OrderedItemsModel>();
      this.StoredItems = new HashSet<StoredItemsModel>();
    }

    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Display(Name = "Item ID")]
    public int ItemId { get; set; }
    [MaxLength(30)] [Required] [Display(Name = "Item Name")]
    public string ItemName { get; set; }
    [RegularExpression(@"^\d+\.\d{0,2}$")] [Range(0, 9999.99)] [Required] [Display(Name = "Item Price")]
    public decimal ItemPrice { get; set; }
    [MaxLength(150)] [Required] [Display(Name = "Item Description")]
    public string ItemDescription { get; set; }
  }
}
