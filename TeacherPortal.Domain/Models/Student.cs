using TeacherPortal.Domain.Common;

namespace TeacherPortal.Domain.Models
{
    public class Student: BaseEntity
    {
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; private set; }
    }
}
