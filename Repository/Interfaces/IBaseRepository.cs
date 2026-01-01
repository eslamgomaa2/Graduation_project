using Domins.Model;
using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public  Task<IEnumerable<T>> GetById(Expression<Func<T, bool>> critria);


        public Task<IEnumerable<T>> GetAllAsync();
        
        public Task<T> Addasync(T entity);
       






    }
}
