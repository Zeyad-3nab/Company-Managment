using Company.Test.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Test.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenaricRepository<Employee>
    {
         Task<IEnumerable<Employee>> GetByNameAsync(string name);
    }
}
