using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IBaseRepository<Tasks> _Tasks;
 
        public UnitOfWork(DatabaseContext context) => _context = context;
        public IBaseRepository<Tasks> Tasks => _Tasks ??= new BaseRepository<Tasks>(_context);
     
        public async Task Save() => await _context.SaveChangesAsync();
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
