using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BookingBus.Controllers;
using BookingBus.Controllers;

using BookingBus.models;

namespace BookingBus.Repository
{
    public class RepositoryGeneric<T> : IRepository<T> where T : class
    {
        private readonly appDbcontext1 _db;
        internal DbSet<T> dbset;

        public RepositoryGeneric(appDbcontext1 db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbset.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    


    public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool track = true)
        {
            IQueryable<T> qurey = dbset;
            if (!track) { qurey = qurey.AsNoTracking(); }
            if (filter != null) { qurey = qurey.Where(filter); }
            return await qurey.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> qurey = dbset;
            if (filter != null) { qurey = qurey.Where(filter); }
            return await qurey.ToListAsync();
        }

       
        public async Task<List<TEntity>> GetAllTEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool tracked = true) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>(); // Use Set<TEntity> for DbSet<TEntity> access

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }




        public async Task<TEntity> GetSpecialEntity<TEntity>(Expression<Func<TEntity, bool>> filter = null, bool tracked = true) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>(); // Use Set<TEntity> for DbSet<TEntity> access

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }


        public async Task Remove<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = _db.Set<TEntity>();
            dbSet.Remove(entity);
            await save();
        }

        public async Task Remove(T entity)
        {
            dbset.Remove(entity);

            await save();
        }


       
        public async Task updatat<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = _db.Set<TEntity>();
            dbSet.Update(entity);
            await save();
        }



        public async Task<List<TEntity>> GetALLEntityByDynamicQuery<TEntity>(string searchField = null, string searchValue = null, bool tracked = true) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>(); // Use Set<TEntity> for DbSet<TEntity> access

            if (!string.IsNullOrEmpty(searchField) && !string.IsNullOrEmpty(searchValue))
            {
                var parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
                var propertyExpr = Expression.Property(parameterExpr, searchField);
                var constantExpr = Expression.Constant(searchValue);

                var equalityExpr = Expression.Equal(propertyExpr, constantExpr);
                var lambdaExpr = Expression.Lambda<Func<TEntity, bool>>(equalityExpr, parameterExpr);

                query = query.Where(lambdaExpr);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }
       
        
        
       
        public async Task<List<TEntity>> DateFilter<TEntity>(string searchField = null, int? year = null, int? month = null, int? day = null, bool tracked = true) where TEntity : class
        {
            IQueryable<TEntity> query = _db.Set<TEntity>(); // Use Set<TEntity> for DbSet<TEntity> access

            if (!string.IsNullOrEmpty(searchField))
            {
                var parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
                var propertyExpr = Expression.Property(parameterExpr, searchField);

                // Convert property type to nullable DateTime if necessary
                Expression propertyConvertedExpr = propertyExpr;
                if (propertyExpr.Type == typeof(DateTime))
                {
                    propertyConvertedExpr = Expression.Convert(propertyExpr, typeof(DateTime?));
                }

                Expression conditionExpr = null;

                // Check if year, month, and day are provided, and add conditions accordingly
                if (year != null)
                {
                    conditionExpr = Expression.Equal(Expression.Property(Expression.Property(propertyConvertedExpr, "Value"), "Year"), Expression.Constant(year));
                }

                if (month != null)
                {
                    var monthCondition = Expression.Equal(Expression.Property(Expression.Property(propertyConvertedExpr, "Value"), "Month"), Expression.Constant(month));
                    conditionExpr = conditionExpr == null ? monthCondition : Expression.AndAlso(conditionExpr, monthCondition);
                }

                if (day != null)
                {
                    var dayCondition = Expression.Equal(Expression.Property(Expression.Property(propertyConvertedExpr, "Value"), "Day"), Expression.Constant(day));
                    conditionExpr = conditionExpr == null ? dayCondition : Expression.AndAlso(conditionExpr, dayCondition);
                }

                // Create lambda expression
                var lambdaExpr = Expression.Lambda<Func<TEntity, bool>>(conditionExpr, parameterExpr);

                query = query.Where(lambdaExpr);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }
        public async Task AddEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _db.Set<TEntity>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task save()
        {
            await _db.SaveChangesAsync();
        }






    }
}