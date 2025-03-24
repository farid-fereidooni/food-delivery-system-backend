using MediatR;
using RestaurantManagement.Write.Domain.Contracts;

namespace RestaurantManagement.Write.Application;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var transactionId = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var response = await next();
        await _unitOfWork.CommitTransaction(transactionId, cancellationToken);

        return response;
    }
}
