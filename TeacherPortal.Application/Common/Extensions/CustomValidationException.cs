using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Application.Common.Extensions
{
    public class CustomValidationException : Exception
    {
        public List<ValidationError> Errors { get; }

        public CustomValidationException(List<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}
