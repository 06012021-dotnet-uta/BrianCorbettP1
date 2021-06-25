using System;

namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class OrderDisplay
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public int OrderId;
    public DateTime OrderDate;
    public decimal OrderCost;
    public string StoreLocation;

    public override bool Equals(object obj)
    {
      if (obj is OrderDisplay)
      {
        var that = obj as OrderDisplay;
        return (this.OrderId == that.OrderId &&
          this.OrderDate == that.OrderDate &&
          this.OrderCost == that.OrderCost &&
          this.StoreLocation == that.StoreLocation);
      }
      return false;
    }
  }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class StoreOrderDisplay : OrderDisplay
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public string Username;

    public override bool Equals(object obj)
    {
      if (obj is StoreOrderDisplay)
      {
        var that = obj as StoreOrderDisplay;
        return base.Equals(obj) && this.Username == that.Username;
      }
      return false;
    }
  }
}
