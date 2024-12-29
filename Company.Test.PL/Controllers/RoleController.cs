using Company.Test.BLL;
using Company.Test.DAL.Models;
using Company.Test.PL.Helpers;
using Company.Test.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace Company.Test.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        //Get , GetAll , Create , Update  ,Delete


        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }



        public async Task<IActionResult> Index(string InputSearch)
        {
            var roles = Enumerable.Empty<RoleVM>();
            if (InputSearch.IsNullOrEmpty())
            {
                roles = await roleManager.Roles.Select(u => new RoleVM()
                {   //Mapping from user to uservm

                    Id = u.Id,
                    Name=u.Name

                }).ToListAsync();
            }
            else
            {
                roles = await roleManager.Roles.Where(u => u.Name.
                                                ToLower().
                                                Contains(InputSearch.ToLower()))
                                                .Select(u => new RoleVM()
                                                {   //Mapping from user to uservm

                                                    Id = u.Id,
                                                   Name= u.Name

                                                }).ToListAsync();
            }

            return View(roles);
        }



        //-----------------------------------------------------------Details-----------------------------------------


        [HttpGet]
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest();

            var roleFromDb = await roleManager.FindByIdAsync(id);

            if (roleFromDb is not null)
            {

                var role = new RoleVM()
                {
                    Id = roleFromDb.Id,
                    Name=roleFromDb.Name
                  
                };

                return View(ViewName, role);
            }
            return NotFound();
        }



        //----------------------------------------------------------Create----------------------------------------------------


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)       //تعمل تاكيد علي الداتا الي رجعالي صح ولا لا
            {
                var role = new IdentityRole()
                {
                    Name = roleVM.Name,
                    NormalizedName=roleVM.Name.ToUpper()
                };
                var result=await roleManager.CreateAsync(role);
                if(result.Succeeded)
                return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Invalid Role");
            }
            return View(roleVM);
        }



        //--------------------------------------------------------Edit------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Edit([FromRoute] string id, RoleVM roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var roleFromDb = await roleManager.FindByIdAsync(id);

                if (roleFromDb is not null)
                {
                    roleFromDb.Name = roleVM.Name;

                    await roleManager.UpdateAsync(roleFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(roleVM);
        }






        //---------------------------------------------------Delete------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Delete([FromRoute] string id, RoleVM roleVM)
        {

            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var roleFromDb = await roleManager.FindByIdAsync(id);

                if (roleFromDb is not null)
                {
                   await roleManager.DeleteAsync(roleFromDb);
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(roleVM);

        }




        //----------------------------------------------------Add Or Remove User From Role----------------------------------------
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            ViewData["RoleId"] = role.Id;
            var UsersInRole = new List<UserInRoleVM>();
            var users = userManager.Users.ToList();
            foreach (var user in users) 
            {
                var userInRole=new UserInRoleVM() 
                {
                    UserId = user.Id,
                    UserName=user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else 
                {
                    userInRole.IsSelected= false;
                }
                UsersInRole.Add(userInRole);
            }
            return View(UsersInRole);
            


        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId , List<UserInRoleVM> userInRoleVMs)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) 
                return NotFound();

            if (ModelState.IsValid) 
            {
                foreach (var user in userInRoleVMs) 
                {
                    var appUser = await userManager.FindByIdAsync(user.UserId);
                    if (appUser != null) 
                    {
                        if (user.IsSelected && !await userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            //Create
                           await userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await userManager.IsInRoleAsync(appUser, role.Name)) 
                        {
                            //Remove
                            await userManager.RemoveFromRoleAsync(appUser, role.Name);
                            
                        }

                    }

                }
                    return RedirectToAction(nameof(Edit) , new {id=roleId});

            }
            return View(userInRoleVMs);
          

        }
    }
}
