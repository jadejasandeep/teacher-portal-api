using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Application.Common.Interfaces
{
    public interface ITeacherDbContext
    {
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Student> Students { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
