using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure.EntityConfiguration.Common;

namespace TeacherPortal.Infrastructure.EntityConfiguration
{
    public class TeacherConfiguration: BaseEntityConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
           
            builder.Property(t => t.Username).IsRequired().HasMaxLength(100);            
            builder.Property(t => t.PasswordHash).IsRequired();
        }
    }
}
