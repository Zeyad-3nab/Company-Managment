using Company.Test.DAL.Models;
using Company.Test.PL.Helpers;
using Company.Test.PL.ViewModels.AuthViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Company.Test.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

        //-----------------------------------------------------------------SignUp---------------------------------------------------
        [HttpGet]
        public IActionResult SignUp() 
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> SignUp(SignUpVM model)
		{
            if (ModelState.IsValid) 
            {
				try
				{
					var user = await userManager.FindByNameAsync(model.Email);    //بشوف ال الاسم ده موجود عندي ولا لا 
					if (user == null)
					{
						user = await userManager.FindByEmailAsync(model.Email);   //بشوف ال الايميل ده موجود عندي ولا لا 
						if (user == null)                                         //لو مش موجودين
						{
							user = new ApplicationUser()                       
							{
								Email = model.Email,
								FirstName = model.FirstName,
								LastName = model.LastName,
								UserName = model.UserName
							};

							var result = await userManager.CreateAsync(user, model.Password);                //ضيفه فالداتا بيز
							if (result.Succeeded)                                                  //اتاكد ان العمليه تمت بنجاح
							{
								return RedirectToAction("SignIn");
							}
							else
							{
								foreach (var error in result.Errors)
								{
									ModelState.AddModelError("", error.Description);
								}
								return View(model);

							}

						}
						ModelState.AddModelError("", "Email is already exists..!!");
						return View(model);
					}
					ModelState.AddModelError("", "UserName is already exists..!!");
					return View(model);
				}
				catch (Exception ex) 
				{
					ModelState.AddModelError("", ex.Message);
				}
			}
			return View(model);
		}




		//-------------------------------------------------SignIn---------------------------------------------

		[HttpGet]
		public IActionResult SignIn() 
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInVM model)
		{
			if (ModelState.IsValid) 
			{
				try 
				{
					var user = await userManager.FindByEmailAsync(model.Email);
					if (user is not null)
					{
						var flag = await userManager.CheckPasswordAsync(user, model.Password);
						if (flag)
						{
							var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

							if (result.Succeeded) 
							{
							    return RedirectToAction("Index", "Home");
							}
						}
					}
					ModelState.AddModelError("", "Invalid In Login..!!");
					return View(model);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}

			}
			return View(model);
		}



		//--------------------------------------------------------------Logout-------------------------------------------------------------

		
		public async Task<IActionResult> SignOut() 
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}


		[HttpGet]
		public IActionResult ForgetPassword() 
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
		{
			if (ModelState.IsValid) 
			{
				var user =await userManager.FindByEmailAsync(model.Email);

				if (user is not null) 
				{
					var token= await userManager.GeneratePasswordResetTokenAsync(user);

					var url=Url.Action("ResetPassword", "Account", new { email = model.Email , token=token }, Request.Scheme);
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url

					};


					//Send	Email

					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError("", "Invalid!!");
			}
			return View(model);
		}


		[HttpGet]
		public IActionResult CheckYourInbox() 
		{
			return View();
		}


		[HttpGet]
		public IActionResult ResetPassword(string email, string token) 
		{
			TempData["email"]= email;
			TempData["token"]= token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordVM model) 
		{
			if (ModelState.IsValid) 
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var user = await userManager.FindByEmailAsync(email);
				if (user != null)
				{
				    var result=await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (result.Succeeded)
                    {
						return RedirectToAction(nameof(SignIn));
                        
                    }
                }

			}
			ModelState.AddModelError("", "Invalid Please try again");
			return View(model);
		}




		public IActionResult AccessDenied() 
		{
			return View();
		}
	}



}
