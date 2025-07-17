using FluentValidation;
using MediatR;
using mPass.Domain;

namespace mPass.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

        if (failures.Count == 0) return await next(cancellationToken);

        var errors = failures.Select(f => f.ErrorMessage).ToArray();

        if (!typeof(TResponse).IsGenericType || typeof(TResponse).GetGenericTypeDefinition() != typeof(Result<>))
            throw new ValidationException(failures);

        var resultType = typeof(TResponse).GetGenericArguments()[0];
        var failureMethod = typeof(Result<>).MakeGenericType(resultType)
            .GetMethod(nameof(Result<object>.Failure), [typeof(string[])]);

        return (TResponse)failureMethod!.Invoke(null, [errors])!;
    }
}