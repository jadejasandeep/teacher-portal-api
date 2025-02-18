using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TeacherPortal.Application.Common.Extensions;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest :  IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("ValidationBehaviour: Validating request {Request}", request);
                if (_validators.Any())
                {
                    var context = new ValidationContext<TRequest>(request);

                    var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                    var failures = validationResults.SelectMany(r => r.Errors).Select(failure => new ValidationError
                    {
                        PropertyName = failure.PropertyName,
                        ErrorMessage = failure.ErrorMessage,
                        ErrorCode = failure.ErrorCode
                    }).Where(f => f != null).Distinct().ToList();

                    if (failures.Count != 0)
                        throw new CustomValidationException(failures);
                   
                }
                return await next();
            }
        }
        
    }
}
