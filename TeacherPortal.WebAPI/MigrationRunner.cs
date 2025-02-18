using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TeacherPortal.Infrastructure;

namespace TeacherPortal.WebAPI
{
    public static class MigrationRunner
    {
        public static void RunMigrations(IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
                .Options;

            var dbContext = new TeacherDBContext(options);

            dbContext.Database.Migrate();

        }
    }
}
