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
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private protected readonly ApplicationDbContext context;

        public GenaricRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T)== typeof(Employee)) 
            {
                return (IEnumerable<T>) await context.employees.Include(e=>e.Workfor).ToListAsync();
            }
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            //if (typeof(T) == typeof(Employee))
            //{
            //    return context.employees.Include(e=>e.WorkforId).Find(id);
            //}
            return await context.Set<T>().FindAsync(id);
        }


        public async Task<int> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync();
        }

    }
}
