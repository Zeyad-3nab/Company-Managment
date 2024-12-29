using Company.Test.BLL.Interfaces;
using Company.Test.DAL.Data.Contexts;
using Company.Test.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Test.BLL.Repositories
{
    public class EmployeeRepository : GenaricRepository<Employee>, IEmployeeRepository
    { 
        public EmployeeRepository(ApplicationDbContext context):base(context)
        {
            
        }

        public async Task<IEnumerable<Employee>> GetByNameAsync(string name)
        {
            return await context.employees.Where(e => e.Name.ToLower().Contains(name)).Include(e=>e.Workfor).ToListAsync();
        }
    }
}
