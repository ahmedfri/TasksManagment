using System.ComponentModel.DataAnnotations;

namespace  Domain.Entities

{
    public class BaseEntity
    {
        public BaseEntity()
        {
            IsDeleted = false;
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Date created is required")]
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
    }

}
