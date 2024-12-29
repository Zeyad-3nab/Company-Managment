using Company.Test.BLL;
using Company.Test.BLL.Interfaces;
using Company.Test.DAL.Models;
using Company.Test.PL.Helpers;
using Company.Test.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Company.Test.PL.Controllers
{
	public class userController : Controller
	{
		//Get , GetAll , Update  ,Delete
		private readonly UserManager<ApplicationUser> userManager;


		public userController(UserManager<ApplicationUser> userManager)
        {
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index(string InputSearch)
		{
			var users = Enumerable.Empty<UserVM>();
			if (InputSearch.IsNullOrEmpty())
			{
				users = await userManager.Users.Select(u=> new UserVM() {   //Mapping from user to uservm

					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Email=u.Email,
					Roles=userManager.GetRolesAsync(u).Result

				}).ToListAsync();
			}
			else
			{
				users = await userManager.Users.Where(u => u.Email.
				                                ToLower().
				                                Contains(InputSearch.ToLower()))
					                            .Select(u => new UserVM()
								                {   //Mapping from user to uservm
								              
								              	  Id = u.Id,
								              	  FirstName = u.FirstName,
								              	  LastName = u.LastName,
								              	  Email = u.Email,
								              	  Roles = userManager.GetRolesAsync(u).Result
								              
								                }).ToListAsync();

			}

			return View(users);
		}

		 

        //-----------------------------------------------------------Details-----------------------------------------


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

			if (id is null)
				return BadRequest();

			var userFromDb = await userManager.FindByIdAsync(id);

            if (userFromDb is not null)
            {

				var user = new UserVM()
				{
					Id = userFromDb.Id,
					FirstName = userFromDb.FirstName,
					LastName = userFromDb.LastName,
					Email=userFromDb.Email,
					Roles = userManager.GetRolesAsync(userFromDb).Result
                };

                return View(ViewName, user);
            }
            return NotFound();
        }






        //--------------------------------------------------------Edit------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Edit([FromRoute] string? id, UserVM userVM)
        {
            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var userFromDb = await userManager.FindByIdAsync(id);

                if (userFromDb is not null)
                {
                    userFromDb.FirstName = userVM.FirstName;
                    userFromDb.LastName = userVM.LastName;
                    userFromDb.Email = userVM.Email;

                    await userManager.UpdateAsync(userFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(userVM);
        }






        //---------------------------------------------------Delete------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Delete([FromRoute] string? id, UserVM userVM)
        {

            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var userFromDb = await userManager.FindByIdAsync(id);

                if (userFromDb is not null)
                {
                    userManager.DeleteAsync(userFromDb);
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(userVM);

        }



    }
}
