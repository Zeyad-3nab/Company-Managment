using System.ComponentModel.DataAnnotations;

namespace Company.Test.PL.ViewModels.AuthViewModel
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "First name is required..!!")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required..!!")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "User name is required..!!")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required..!!")]
        [DataType(DataType.EmailAddress , ErrorMessage = "Invalid Email..!!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required..!!")]
        [DataType(DataType.Password , ErrorMessage ="Confirmed password doesn't match password..!!")]
        public string Password { get; set; }


        [Required(ErrorMessage ="Confirmed Password is required..!!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }


        [Required(ErrorMessage ="Confirmed Password is required..!!")]
        public bool IsAgree { get; set; }
    }
}
