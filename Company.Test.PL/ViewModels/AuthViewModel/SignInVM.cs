using System.ComponentModel.DataAnnotations;

namespace Company.Test.PL.ViewModels.AuthViewModel
{
	public class SignInVM
	{
        [Required(ErrorMessage ="Email Is Required..!!")]
        [DataType(DataType.EmailAddress , ErrorMessage ="Ivalid Email..!!")]
        public string Email { get; set; }


        [Required(ErrorMessage ="Password Is Required..!!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
