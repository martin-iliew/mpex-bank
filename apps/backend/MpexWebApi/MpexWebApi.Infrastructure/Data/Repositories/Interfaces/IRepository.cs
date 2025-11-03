using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarApp.Infrastructure.Data.Repositories.Interfaces
{
    public interface IRepository<TType, TId>
    {
        TType? GetById(TId id);

        Task<TType?> GetByIdAsync(TId id);

        TType? FirstOrDefault(Func<TType, bool> predicate);

        Task<TType?> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate);

        IEnumerable<TType> GetAll();

        Task<IEnumerable<TType>> GetAllAsync();

        Task SaveChangesAsync();

        IQueryable<TType> GetAllAttached();

        IQueryable<TType> GetAllAsReadOnly();

        void Add(TType item);

        Task AddAsync(TType item);

        void AddRange(TType[] items);

        Task AddRangeAsync(TType[] items);

        bool Delete(TType entity);

        Task<bool> DeleteAsync(TType entity);

        bool Update(TType item);

        Task<bool> UpdateAsync(TType item);
    }
}
