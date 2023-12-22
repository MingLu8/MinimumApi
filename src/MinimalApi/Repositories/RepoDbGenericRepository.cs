using RepoDb;
using System.Data.Common;

namespace MinimumApi.Repositories
{
    public class RepoDbGenericRepository<T, TDbConnection>(TDbConnection connection) 
        : BaseRepository<T, TDbConnection>(connection.ConnectionString), IGenericRepository<T> where T : class where TDbConnection : DbConnection, new()
    {
        public int Delete(long id) => base.Delete(id);  
       
        public Task<int> DeleteAsync(long id) => base.DeleteAsync(id);
       
        public IEnumerable<T> GetAll() => QueryAll();
      
        public Task<IEnumerable<T>> GetAllAsync() => QueryAllAsync();

        public bool Exists(long id) => base.Exists(id);
        public T? GetById(long id) => Query(id).SingleOrDefault();

        public Task<bool> ExistsAsync(long id) => base.ExistsAsync(id);
        public async Task<T?> GetByIdAsync(long id)
        {
            var result = await QueryAsync(id);
            return result.SingleOrDefault();
        }

        public long Insert(T entity) => base.Insert<long>(entity);

        public Task<long> InsertAsync(T entity) => base.InsertAsync<long>(entity);
      
        public int Update(T entity) => base.Update(entity);

        public Task<int> UpdateAsync(T entity) => base.UpdateAsync(entity);      
    }
}
