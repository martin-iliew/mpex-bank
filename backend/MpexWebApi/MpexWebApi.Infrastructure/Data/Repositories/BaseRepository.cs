using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MpexTestApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarApp.Infrastructure.Data.Repositories
{
    public class BaseRepository<TType, Tid> : IRepository<TType, Tid>
        where TType : class
    {
        private readonly AppDbContext context;

        private readonly DbSet<TType> dbset;

        public BaseRepository(AppDbContext _context)
        {
            context = _context;
            dbset = context.Set<TType>();
        }

        public void Add(TType item)
        {
            dbset.Add(item);
            context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await dbset.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public void AddRange(TType[] items)
        {
            dbset.AddRange(items);
            context.SaveChanges();
        }

        public async Task AddRangeAsync(TType[] items)
        {
            await dbset.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        public bool Delete(TType entity)
        {
            dbset.Remove(entity);
            context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TType entity)
        {
            dbset.Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        public TType? FirstOrDefault(Func<TType, bool> predicate)
        {
            TType? entity = dbset
                .FirstOrDefault(predicate);

            return entity;
        }

        public async Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate)
        {
            TType? entity = await dbset
                .FirstOrDefaultAsync(predicate);

            return entity;
        }

        public IEnumerable<TType> GetAll()
        {
            return dbset.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await dbset.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return dbset.AsQueryable();
        }

        public IQueryable<TType> GetAllAsReadOnly()
        {
            return dbset.AsNoTracking();
        }

        public TType? GetById(Tid id)
        {
            TType? entity = dbset
                .Find(id);

            return entity;
        }

        public async Task<TType?> GetByIdAsync(Tid id)
        {
            TType? entity = await dbset
                .FindAsync(id);

            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public bool Update(TType item)
        {
            try
            {
                dbset.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                dbset.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
