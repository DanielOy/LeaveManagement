using LeaveManagement.Application.Contracts.Persitence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LeaveManagementDbContext _dbContext;
        public GenericRepository(LeaveManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            return entity;
        }

        public async Task Delete(T entity)
        {
            await Task.Run(() =>
            {
                _dbContext.Set<T>().Remove(entity);
            });
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await Get(id);
            return entity != null;
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            await Task.Run(() =>
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            });
        }
    }
}
