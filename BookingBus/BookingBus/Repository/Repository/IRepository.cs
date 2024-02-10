using System.Linq.Expressions;
using BookingBus.models;
using Microsoft.AspNetCore.Mvc;

namespace BookingBus.Repository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        public Task save();
        Task Remove(T entity);

        Task<List<TEntity>> GetAllTEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool tracked = true) where TEntity : class;
        

        Task<TEntity> GetSpecialEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool tracked = true) where TEntity : class;

        Task Remove<TEntity>(TEntity entity) where TEntity : class;
        Task updatat<TEntity>(TEntity entity) where TEntity : class;
        Task<List<TEntity>> GetALLEntityByDynamicQuery<TEntity>(string searchField = null, string searchValue = null, bool tracked = true) where TEntity : class;

        Task<List<TEntity>> DateFilter<TEntity>(string searchField = null, int? year = null, int? month = null, int? day = null, bool tracked = true) where TEntity : class;
        Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : class;

    }
}