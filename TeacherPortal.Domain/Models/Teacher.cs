using TeacherPortal.Domain.Common;

namespace TeacherPortal.Domain.Models
{
    public class Teacher: BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public virtual List<Student> Students { get; set; } = new();
    }
}
