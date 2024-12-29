using AutoMapper;
using Company.Test.BLL.Interfaces;
using Company.Test.BLL.Repositories;
using Company.Test.DAL.Models;
using Company.Test.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Test.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }


        //----------------------------------------------------Get All------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments= await unitOfWork.departmentRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<DepartmentVM>>(departments);
            return View(result);
        }

        //----------------------------------------------Details----------------------------------

        [HttpGet]
        public async Task<IActionResult> Details(int id,string ViewName = "Details") 
        {
            var department =await unitOfWork.departmentRepository.GetAsync(id);
            if (department is not null) 
            {
                var result=_mapper.Map<DepartmentVM>(department);
                return View(ViewName, result);
            }
            return NotFound();
        }



        //--------------------------------------------------Craete------------------------------------------------

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Create(DepartmentVM departmentvm)
        {
            if (ModelState.IsValid)       //تعمل تاكيد علي الداتا الي رجعالي صح ولا لا
            {
                var result = _mapper.Map<Department>(departmentvm);
                var count=await unitOfWork.departmentRepository.AddAsync(result);    //لو الراجع صفر يبقي في ايرور ومتعملوش سيف لو واجد يبقي تمام

                if (count > 0) 
                {
                   return RedirectToAction("Index");
                }
                    return BadRequest();
            }
            return View(departmentvm);
        }


        //-------------------------------------------------------Edit-----------------------------------------------------


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Edit([FromRoute]int? id ,DepartmentVM departmentvm)
        {
            if(id!= departmentvm.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var result = _mapper.Map<Department>(departmentvm);
                var count = await unitOfWork.departmentRepository.UpdateAsync(result); //لو الراجع صفر يبقي في ايرور ومتعملوش سيف لو واجد يبقي تمام

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return View(departmentvm);
        }



        //-------------------------------------------------------------Delete-----------------------------------------


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Delete([FromRoute]int? id ,DepartmentVM departmentvm)
        {
            if (id != departmentvm.Id) return BadRequest();
            var result=_mapper.Map<Department>(departmentvm);
            var count =await unitOfWork.departmentRepository.DeleteAsync(result);

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
         
        }


    }
}
