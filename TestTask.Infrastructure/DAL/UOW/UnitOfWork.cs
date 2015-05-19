using System;
using System.Threading.Tasks;
using TestTask.Common.DAL.Context;
using TestTask.Common.DAL.DatabaseModels;

namespace TestTask.Common.DAL.UOW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly MessureContext _context;

        private IGenericRepository<Counter> _counters;
        private IGenericRepository<Marker> _markers;

        public UnitOfWork()
        {
            _context = new MessureContext();
        }

        public IGenericRepository<Counter> CountersRepository
        {
            get { return _counters ?? (_counters = new GenericRepository<Counter>(_context)); }
        }

        public IGenericRepository<Marker> MarkersRepository
        {
            get { return _markers ?? (_markers = new GenericRepository<Marker>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
