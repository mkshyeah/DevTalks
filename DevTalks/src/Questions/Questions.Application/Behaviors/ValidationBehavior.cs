using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Pipelines;

// public class ValidationBehavior<TRequest, TResponse> 
//     : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : ICommand<TResponse>
// {
//     private readonly IEnumerable<IValidator<TRequest>> _validators;
//     
//     public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
//     {
//         _validators = validators;
//     }
//     
//     public async Task<TResponse> Handle(
//         TRequest request,
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         var context = new ValidationContext<TRequest>(request);
//         
//         var validationFailures = await Task.WhenAll(
//             _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
//
//         var failures = validationFailures
//             .Where(r => !r.IsValid)
//             .ToList();
//
//         if (failures.Count != 0)
//         {
//             var failure = failures.ToErrors();
//
//             var successType = typeof(TResponse).GetGenericArguments()[0];
//             var failureType = typeof(TRequest).GetGenericArguments()[1];
//             
//             var failureMethod = typeof(Result)
//                 .GetMethods()
//                 .First(m => m.Name == "Failure" &&
//                             m.GetParameters().Length == 1 &&
//                             m.GetGenericArguments().Length == 2)
//                 .MakeGenericMethod(successType, failureType);
//                     
//             return (TResponse)failureMethod.Invoke(null, [failure])!;
//         }
//         
//         var response = await next(cancellationToken);
//         
//         return response;
//     }
// }