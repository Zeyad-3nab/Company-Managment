using Company.Test.BLL.Interfaces;
using Company.Test.BLL.Repositories;
using Company.Test.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Test.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IDepartmentRepository _departmentRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _departmentRepository = new DepartmentRepository(context);
            _employeeRepository = new EmployeeRepository(context);
            _context = context;
        }

        public IEmployeeRepository employeeRepository => _employeeRepository;

        public IDepartmentRepository departmentRepository => _departmentRepository;
    }
}
