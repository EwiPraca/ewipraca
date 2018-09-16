using System.Collections.Generic;

namespace EwiPraca.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        int Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        T GetById(int id);

        IEnumerable<T> All();
    }
}