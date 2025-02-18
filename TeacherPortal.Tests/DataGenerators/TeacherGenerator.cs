using Bogus;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Tests.DataGenerators
{
    public static class TeacherGenerator
    {
        public static List<Teacher> GenerateTeachers(int amount)
        {
            var teachers = new Faker<Teacher>()
                .RuleFor(x => x.Id, f => f.IndexVariable)
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.PasswordHash, f => f.Random.String())
                .RuleFor(x => x.Username, f => f.Person.UserName)
                .Generate(amount);

            return teachers;
        }
    }
}
