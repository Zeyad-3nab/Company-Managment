using Company.Test.BLL.Interfaces;
using Company.Test.DAL.Data.Contexts;
using Company.Test.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Test.BLL.Repositories
{
    public class DepartmentRepository :GenaricRepository<Department> ,IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) 
        {
        }

    }
}
