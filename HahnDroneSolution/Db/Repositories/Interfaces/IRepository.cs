using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> Predicate);
        Task<T> GetByID(int ID);
        Task<bool> Any(Expression<Func<T, bool>> Predicate);
        Task<T> FirstOrDefault();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> Predicate);
        Task Save();
        Task<T> Create(T Entity);
        Task<T> Create_Save(T Entity);
        Task<T> Update(T Entity);
        Task<T> Update_Save(T Entity);
        Task Delete(T Entity);
        Task Delete_Save(T Entity);
        Task<int> Count(Expression<Func<T, bool>> Predicate);
        Task<int> CountAsync();
        int Count();
        Task<T> Single(Expression<Func<T, bool>> Predicate);
        IEnumerable<T> FindWhere(Expression<Func<T, bool>> Predicate);
        Task<IEnumerable<T>> FindWhereAsync(Expression<Func<T, bool>> Predicate);
    }
}
