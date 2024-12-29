using AutoMapper;
using Company.Test.DAL.Models;
using Company.Test.PL.ViewModels;

namespace Company.Test.PL.Mapping
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee,EmployeeVM>().ReverseMap();
            //CreateMap<EmployeeVM, Employee>();
        }


    }
}
