using System.ComponentModel.DataAnnotations;

namespace TeacherPortal.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
