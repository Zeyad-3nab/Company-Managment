using System.ComponentModel.DataAnnotations;

namespace Company.Test.PL.ViewModels.AuthViewModel
{
	public class ResetPasswordVM
	{

		[Required(ErrorMessage = "Password is required..!!")]
		[DataType(DataType.Password, ErrorMessage = "Confirmed password doesn't match password..!!")]
		public string Password { get; set; }


		[Required(ErrorMessage = "Confirmed Password is required..!!")]
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmedPassword { get; set; }
	}
}
