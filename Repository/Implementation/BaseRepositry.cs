using Domins.Model;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Linq.Expressions;

namespace Repository.Implementation
{
    public class BaseRepositry<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbcontext _dbcontext;

        public BaseRepositry(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<T>> GetById(Expression<Func<T, bool>> critria)
        {
            var users = await _dbcontext.Set<T>()
        .Where(critria).ToListAsync();
            return users;

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbcontext.Set<T>().ToListAsync();
        }

        

        public async Task<T> Addasync(T entity)
        {
             await _dbcontext.Set<T>().AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

    }
}
