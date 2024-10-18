using ForumProjects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProjects.Infrastructure.Repository
{
    public class GenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public T GetById(string id)
        {
            var result = _context.Set<T>().Find(id);

            return result;
        }

        public T Create(T model)
        {
            _context.Add(model);
            _context.SaveChanges();

            return model;
        }

        public T Update(T model)
        {
            _context.Update(model);
            _context.SaveChanges();

            return model;
        }

        public bool Delete(string id)
        {
            var result = _context.Set<T>().Find(id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            _context.SaveChanges();

            return true;
        }
    }
}
