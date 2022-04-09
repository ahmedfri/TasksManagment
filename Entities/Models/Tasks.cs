using Domain.Entities;

namespace Entities.Models
{
    public class Tasks : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}