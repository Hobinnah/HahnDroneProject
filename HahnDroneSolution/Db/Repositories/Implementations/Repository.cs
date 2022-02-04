using HahnDroneAPI.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected HahnDroneDBContext _context;
        public Repository(HahnDroneDBContext context)
        {
            this._context = context;
        }

        public Task Save() => _context.SaveChangesAsync();

        public async Task<bool> Any(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(Predicate).AnyAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(Predicate).CountAsync();
        }

        public int Count()
        {
            return _context.Set<T>().AsNoTracking().Count();
        }


        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().AsNoTracking().CountAsync();
        }

        public async Task<T> Create_Save(T entity)
        {
            _context.Set<T>().Add(entity);
            await Save();

            return entity;
        }

        public async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }

        public async Task<T> Update_Save(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Save();

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task Delete_Save(T entity)
        {
            _context.Set<T>().Remove(entity);
            await Save();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> FirstOrDefault()
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(Predicate);
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(Predicate).ToListAsync();
        }

        public IEnumerable<T> FindWhere(Expression<Func<T, bool>> Predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(Predicate).ToList();
        }

        public async Task<IEnumerable<T>> FindWhereAsync(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(Predicate).ToListAsync();
        }

        public async Task<T> Single(Expression<Func<T, bool>> Predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(Predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByID(int ID)
        {
            return await _context.Set<T>().FindAsync(ID);
        }

    }
}
