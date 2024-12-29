using System.ComponentModel.DataAnnotations;

namespace Company.Test.PL.ViewModels.AuthViewModel
{
	public class ForgetPasswordVM
	{
		[Required(ErrorMessage = "Email Is Required..!!")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Ivalid Email..!!")]
		public string Email { get; set; }

	}
}
