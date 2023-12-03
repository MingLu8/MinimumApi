using MinimimApi.Entities;
using MinimumApi.Entities;
using MinimumApi.Services;
using System.Data.Common;
using System.Linq.Expressions;

namespace MinimumApi.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        bool Exists(long id);
        T? GetById(long id);
        IEnumerable<T> GetAll();
        long Insert(T entity);
        int Update(T entity);
        int Delete(long id);

        Task<bool> ExistsAsync(long id);
        Task<T?> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<long> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(long id);
    }

    public interface IPersonRepository : IGenericRepository<Person>
    {
        //Task<IEnumerable<Person>> GetAllPeopleAsync();
        //Task<Person> GetPersonByIdAsync(long id);
        //Task<IEnumerable<Person>> FindPersonAsync(Expression<Func<Person, bool>> where);
        //Task<bool> ExistsAsync(long id);
        //Task AddPersonAsync(Person person);
        //Task UpdatePersonAsync(Person person);
        //Task DeletePersonAsync(long id);              
    }
}
