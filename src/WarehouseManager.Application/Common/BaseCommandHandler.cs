using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Common;

public abstract class BaseCommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>
    where TCommand : IRequest<Result<TResult>>
{
    protected readonly IUnitOfWork Uow;

    protected BaseCommandHandler(IUnitOfWork uow)
    {
        Uow = uow;
    }

    public async Task<Result<TResult>> Handle(TCommand request, CancellationToken ct)
    {
        var validation = await ValidateAsync(request, ct);
        if (validation.IsFailure)
            return Result.Failure<TResult>(validation.Error!);

        var result = await ExecuteAsync(request, ct);
        if (result.IsFailure)
            return result;

        await Uow.SaveChangesAsync(ct);
        await PostExecuteAsync(request, result.Value, ct);

        return result;
    }

    protected virtual Task<Result> ValidateAsync(TCommand request, CancellationToken ct)
    {
        return Task.FromResult(Result.Success());
    }

    protected abstract Task<Result<TResult>> ExecuteAsync(TCommand request, CancellationToken ct);

    protected virtual Task PostExecuteAsync(TCommand request, TResult result, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}