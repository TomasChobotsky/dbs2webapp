using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(object id) =>
            await _dbSet.FindAsync(id);

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => FindAsync(predicate, null);

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity) =>
            await _dbSet.AddAsync(entity);

        public void Update(TEntity entity) =>
            _dbSet.Update(entity);

        public void Remove(TEntity entity) =>
            _dbSet.Remove(entity);

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
