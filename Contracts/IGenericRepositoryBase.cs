using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TennisWebApp8.Contracts
{
    public interface IGenericRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            List<string> incudes = null
            );
        Task<T> Find(
            Expression<Func<T, bool>> expression,            
            List<string> incudes = null
            );
        Task<bool> isExist(
            Expression<Func<T, bool>> expression = null
            );
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        //Task<bool> Save();
    }
}
