using Contracts;
using Entities;
using Entities.DataTransferObjects.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;
using X.PagedList;


namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DatabaseContext DatabaseContext;
        private readonly DbSet<T> Dbset;
        public BaseRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            Dbset = DatabaseContext.Set<T>();
        }

        public async Task Delete(int id)
        {
            var entity = await Dbset.FindAsync(id);
            Dbset.Remove(entity);
        }

             public async Task<T> Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = Dbset;


            if (include != null)
            {
                query = include(query);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = Dbset;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable> FindBy(Expression<Func<T, bool>> filter, Func<IQueryable, IOrderedQueryable> orderBy = null, params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = Dbset.Where(filter);
            includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();

        }
        public async Task<IPagedList<T>> GetPagedList(PaginationParameters paginationParameters, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = Dbset;


            if (include != null)
            {
                query = include(query);
            }

            return await query.AsNoTracking()
                .ToPagedListAsync(paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task AddAsync(T entity)
        {
            await Dbset.AddAsync(entity);
        }

        public void Update(T entity)
        {
            Dbset.Attach(entity);
            DatabaseContext.Entry(entity).State = EntityState.Modified;
        }


    }
}