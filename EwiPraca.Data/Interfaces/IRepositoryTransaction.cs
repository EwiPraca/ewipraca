using System;

namespace EwiPraca.Data.Interfaces
{
    public interface IRepositoryTransaction : IDisposable
    {
        void Rollback();

        void Save();
    }
}