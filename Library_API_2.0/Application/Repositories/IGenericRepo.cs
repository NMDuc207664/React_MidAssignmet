using System.Linq.Expressions;

namespace Library_API_2._0.Application.Repositories
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T> GetIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid Id);
        // void FindbyId(int id);
        Task SaveChangesAsync();
    }
}