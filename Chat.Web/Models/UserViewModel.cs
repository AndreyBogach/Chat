using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}