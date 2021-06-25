namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class StoredItemDisplay
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public string ItemName;
    public string ItemDescription;
    public decimal ItemPrice;
    public int InStock;
    public int ItemId;

    public override bool Equals(object obj)
    {
      if (obj is StoredItemDisplay)
      {
        var that = obj as StoredItemDisplay;
        return (this.ItemName == that.ItemName &&
          this.ItemDescription == that.ItemDescription &&
          this.ItemPrice == that.ItemPrice &&
          this.InStock == that.InStock &&
          this.ItemId == that.ItemId);
      }
      return false;
    }
  }
}
