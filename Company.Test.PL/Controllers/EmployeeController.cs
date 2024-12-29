using AutoMapper;
using Company.Test.BLL.Interfaces;
using Company.Test.DAL.Models;
using Company.Test.PL.Helpers;
using Company.Test.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Company.Test.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //-----------------------------------------------------------GetAll-----------------------------------------

        public async Task<IActionResult> Index(string InputSearch)
        {
            var employees=Enumerable.Empty<Employee>();
            if (InputSearch.IsNullOrEmpty())
            {
                 employees = await unitOfWork.employeeRepository.GetAllAsync();
            }
            else 
            {
                 employees =await unitOfWork.employeeRepository.GetByNameAsync(InputSearch);
            }

            var reesult = _mapper.Map<IEnumerable<EmployeeVM>>(employees);

            return View(reesult);
        }


        //-----------------------------------------------------------Details-----------------------------------------


        [HttpGet]
        public async Task<IActionResult> Details(int id, string ViewName = "Details")
        {
            var employee = await unitOfWork.employeeRepository.GetAsync(id);

            if (employee is not null)
            {
                var employeevm = _mapper.Map<EmployeeVM>(employee);
                return View(ViewName, employeevm);
            }
            return NotFound();
        }



        //-----------------------------------------------------------Create-----------------------------------------

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments= await unitOfWork.departmentRepository.GetAllAsync();
           var result= _mapper.Map<IEnumerable<DepartmentVM>>(departments);

            ViewData["Departments"] = result;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Create(EmployeeVM employeevm)
        {
            if (ModelState.IsValid)       //تعمل تاكيد علي الداتا الي رجعالي صح ولا لا
            {
                if (employeevm.Image is not null) 
                {
                    employeevm.ImageName = DocumentSettings.Upload(employeevm.Image, "Images");   // خزن الصوره وهات اسمها
                }

                var employee =_mapper.Map<Employee>(employeevm);

                var count = await unitOfWork.employeeRepository.AddAsync(employee);    //لو الراجع صفر يبقي في ايرور ومتعملوش سيف لو واجد يبقي تمام

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return View(employeevm);
        }



       //--------------------------------------------------------Edit------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var departments =await unitOfWork.departmentRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<DepartmentVM>>(departments);
            ViewData["Departments"] = result;
            return await Details(id, "Edit");
        }



        [HttpPost]   // Get data and save it in DB
        [ValidateAntiForgeryToken] //Refuse any out app to contact with this method 
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeVM employeevm)
        {
            if (id != employeevm.Id) return BadRequest();   //Check if any one send wrong data

            if (ModelState.IsValid)
            {
                if (employeevm.ImageName is not null) 
                    DocumentSettings.Delete(employeevm.ImageName, "Images");  //Upload Image to wwwroot


                if(employeevm.Image is not null)
                    employeevm.ImageName = DocumentSettings.Upload(employeevm.Image, "Images");   //Save image && return name


                var employee=_mapper.Map<Employee>(employeevm); //map to employee
                var count =await unitOfWork.employeeRepository.UpdateAsync(employee);

                if (count > 0) //Done
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return View(employeevm);
        }






        //---------------------------------------------------Delete------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]    //تمنع اي ابلكيشن خارجي يكلم الابلكيشن ده زي Postman
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeVM employeevm)
        {
            if (id != employeevm.Id) return BadRequest();


            var employee= _mapper.Map<Employee>(employeevm);
            var count =await unitOfWork.employeeRepository.DeleteAsync(employee);

            if (count > 0)
            {
                if (employeevm.ImageName is not null)
                {
                    DocumentSettings.Delete(employeevm.ImageName, "Images");  //اتاكد الاول انه اتمسح وبعدين امسح الصوره
                }
                return RedirectToAction("Index");
            }
            return BadRequest();

        }
    }
}
