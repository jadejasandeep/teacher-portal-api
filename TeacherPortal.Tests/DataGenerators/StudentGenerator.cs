using Bogus;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Tests.DataGenerators
{
    public static class StudentGenerator
    {
        public static List<Student> GenerateStudents(int amount,int teacherId)
        {
            var teachers = new Faker<Student>()
                .RuleFor(x => x.Id, f => f.IndexVariable)
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x=>x.TeacherId,teacherId)
                .Generate(amount);

            return teachers;
        }
    }
}
