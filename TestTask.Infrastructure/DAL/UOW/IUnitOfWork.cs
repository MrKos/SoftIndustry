using System;
using TestTask.Common.DAL.DatabaseModels;

namespace TestTask.Common.DAL.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Counter> CountersRepository { get; }
        IGenericRepository<Marker> MarkersRepository { get; }

        void Save();
    }
}
