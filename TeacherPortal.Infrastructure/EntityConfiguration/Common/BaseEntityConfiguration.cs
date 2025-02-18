using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeacherPortal.Domain.Common;

namespace TeacherPortal.Infrastructure.EntityConfiguration.Common
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<BaseEntity>
    {  
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {   
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.HasKey(e => e.Id).IsClustered();
            builder.Property(t => t.Email).IsRequired().HasMaxLength(200);
            builder.Property(t => t.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(t => t.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.ConcurrencyToken).IsRowVersion().ValueGeneratedOnAddOrUpdate();
        }
    }
}
