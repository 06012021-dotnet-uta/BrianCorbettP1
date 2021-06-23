using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLibrary
{
  public class CustomerModel
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
    public string Username { get; set; }
    [MaxLength(20)] [Required(ErrorMessage= "This field is required.")] [Display(Name = "Password")]
    public string Password { get; set; }
    [Required] [Display(Name = "Signup Date")]
    public DateTime SignupDate { get; set; }
  }
}
