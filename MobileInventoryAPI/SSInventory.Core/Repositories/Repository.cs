using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly SSInventoryDbContext _dbContext;
        public Repository(SSInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _dbContext.Set<TEntity>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<TEntity>> AddAsync(List<TEntity> entities)
        {
            if (entities?.Any() == false)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entities must not be null");
            }
            try
            {
                await _dbContext.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();

                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(DeleteAsync)} entity must not be null");
            }

            try
            {
                _dbContext.Remove(entity);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(List<TEntity> entities)
        {
            if (entities?.Any() == false)
            {
                throw new ArgumentNullException($"{nameof(DeleteAsync)} entity must not be null");
            }

            try
            {
                _dbContext.RemoveRange(entities);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }
            try
            {
                _dbContext.Update(entity);
                if (saveImmediately)
                {
                    await _dbContext.SaveChangesAsync();
                }

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TEntity>> UpdateAsync(List<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }

            try
            {
                _dbContext.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();

                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
