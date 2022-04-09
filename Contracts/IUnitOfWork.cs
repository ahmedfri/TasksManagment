using Entities.Models;

namespace Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        
        IBaseRepository<Tasks> Tasks { get; }

        Task Save();
    }
}
