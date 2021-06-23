using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
  public class StoreModel
  {
    public virtual ICollection<StoredItemsModel> StoredItems { get; set; }
    public StoreModel()
    {
      this.StoredItems = new HashSet<StoredItemsModel>();
    }

    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Display(Name = "Store ID")]
    public int StoreId { get; set; }
    [MaxLength(30)] [Required] [Display(Name = "Store Location")]
    public string StoreLocation { get; set; }
  }
}
