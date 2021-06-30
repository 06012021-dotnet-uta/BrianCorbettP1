using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  public class CustomerModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
  {
    public virtual ICollection<CustomerOrderModel> CustomerOrders { get; set; }
    public CustomerModel()
    {
      this.CustomerOrders = new HashSet<CustomerOrderModel>();
      this.SignupDate = DateTime.Now;
    }

    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Display(Name = "Customer ID")]
    public int CustomerId { get; set; }
    [MaxLength(30)] [Required(ErrorMessage="This field is required.")] // server-side validation
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [MaxLength(30)] [Required(ErrorMessage= "This field is required.")] [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [MaxLength(20)] [Required(ErrorMessage = "This field is required.")] [Display(Name = "Username")]
    [RegularExpression(@"^\S*$", ErrorMessage ="No spaces allowed")]
    public string Username { get; set; }
    [MaxLength(20)] [Required(ErrorMessage= "This field is required.")] [Display(Name = "Password")]
    public string Password { get; set; }
    [Required] [Display(Name = "Signup Date")]
    public DateTime SignupDate { get; set; }
    [Display(Name = "Default Store")]
    public int DefaultStoreId { get; set; }
    [ForeignKey("DefaultStoreId")]
    public StoreModel Store { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is CustomerModel)
      {
        var that = obj as CustomerModel;
        return (this.CustomerId == that.CustomerId && 
          this.FirstName == that.FirstName &&
          this.LastName == that.LastName &&
          this.Username == that.Username &&
          this.Password == that.Password &&
          this.SignupDate == that.SignupDate &&
          this.DefaultStoreId == that.DefaultStoreId);
      }
      return false;
    }
  }
}
