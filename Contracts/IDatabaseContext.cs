using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Contracts
{
    public interface IDatabaseContext
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Tasks> Tasks { get; set; }
        Task<int> SaveChangesAsync();
    }

}
