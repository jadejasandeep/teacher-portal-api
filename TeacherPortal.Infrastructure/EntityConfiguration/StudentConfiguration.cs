using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure.EntityConfiguration.Common;

namespace TeacherPortal.Infrastructure.EntityConfiguration
{
    public class StudentConfiguration : BaseEntityConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {  
            builder.HasOne(s => s.Teacher)
                   .WithMany(t => t.Students)
                   .HasForeignKey(s => s.TeacherId);
        }
    }
}
