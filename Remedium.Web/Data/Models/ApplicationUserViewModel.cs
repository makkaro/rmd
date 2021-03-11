using System;
using System.ComponentModel.DataAnnotations;

namespace Remedium.Web.Data.Models
{
    public sealed record ApplicationUserViewModel
    {
        [Required, Display(Name = "Login"), MaxLength(32)]
        public String UserName { get; set; }
        
        [Required, EmailAddress]
        public String Email { get; set; }
        
        [Required, DataType(DataType.Password)]
        public String Password { get; set; }
        
        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Failed to confirm password.")]
        public String ConfirmationPassword { get; init; }
    }
}