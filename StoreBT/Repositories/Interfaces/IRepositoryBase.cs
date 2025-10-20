using System.Linq.Expressions;


namespace StoreBT.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity?> FindByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includes
        );
        Task<IList<TEntity>> FindAll(
           Expression<Func<TEntity, bool>>? expression = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           CancellationToken cancellationToken = default,
           params Expression<Func<TEntity, object>>[] includes
       );
        Task<TEntity?> Find(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includes
        );
        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default
        );
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> filterExpression,
            CancellationToken cancellationToken = default
        );
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task<ICollection<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? expression = null,
            CancellationToken cancellationToken = default
        );

        Task<int> SaveChangeAsync();
    }
}