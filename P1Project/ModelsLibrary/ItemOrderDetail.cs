namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class ItemOrderDetail
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public string ItemName;
    public int QuantityOrdered;
    public decimal ItemCost;
    public int CustomerId;

    public override bool Equals(object obj)
    {
      if (obj is ItemOrderDetail)
      {
        var that = obj as ItemOrderDetail;
        return (this.CustomerId == that.CustomerId &&
          this.ItemName == that.ItemName &&
          this.ItemCost == that.ItemCost &&
          this.QuantityOrdered == that.QuantityOrdered);
      }
      return false;
    }
  }
}
