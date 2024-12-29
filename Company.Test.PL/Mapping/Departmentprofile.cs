using AutoMapper;
using Company.Test.DAL.Models;
using Company.Test.PL.ViewModels;

namespace Company.Test.PL.Mapping
{
    public class Departmentprofile:Profile
    {

        public Departmentprofile()
        {
            CreateMap<Department,DepartmentVM>().ReverseMap();
            //CreateMap<DepartmentVM, Department>();
        }
    }
}
