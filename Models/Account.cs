using System.ComponentModel.DataAnnotations;

namespace myBlog.Models
{
    public class Account
    {
        [Key]
        [MaxLength(100)]
        [RegularExpression(@"^\w+([\.-]?\w+)*@*(fpt\.edu\.vn)$", ErrorMessage = "Email format invalid")]
        [Required(ErrorMessage = "Email can not blank")]
        [Display(Name ="Email:")]
        public string Email { get; set; }
        
        [MaxLength(30)]
        [Required(ErrorMessage = "Password can not blank")]
        [Display(Name ="Password:")]
        public string Password { get; set; }

    }
}