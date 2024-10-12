using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class ResetPasswordVM
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "password is required")]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfrimPassword { get; set; }

        [Required]
        public string Code {  get; set; }

        [Required]
        [EmailAddress]
        public string Email {  get; set; }
    }
}
