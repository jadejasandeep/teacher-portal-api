using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Domain.Common;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Infrastructure
{
    public class TeacherDBContext : DbContext, ITeacherDbContext
    {
        

        public TeacherDBContext(DbContextOptions<TeacherDBContext> options) : base(options)
        {
            
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsInMemory())
            {
                modelBuilder.Entity<Teacher>().Ignore(t => t.ConcurrencyToken);
                modelBuilder.Entity<Student>().Ignore(t => t.ConcurrencyToken);
            }
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeacherDBContext).Assembly);
            modelBuilder.Ignore<BaseEntity>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("Created").CurrentValue = DateTime.UtcNow;
            });

            return await base.SaveChangesAsync(cancellationToken);
        }



        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
    }

    public static class DatabaseFacadeExtensions
    {
        public static bool IsInMemory(this DatabaseFacade database)
        {
            return database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
        }
    }
}
