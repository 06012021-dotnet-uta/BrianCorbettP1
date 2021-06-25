using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class StoreModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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

    public override bool Equals(object obj)
    {
      if (obj is StoreModel)
      {
        var that = obj as StoreModel;
        return (this.StoreId == that.StoreId &&
          this.StoreLocation == that.StoreLocation);
      }
      return false;
    }
  }
}
