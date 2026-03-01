using MediatR;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Orders.Queries.ResolveCustomerId;

public class ResolveCustomerIdQueryHandler : IRequestHandler<ResolveCustomerIdQuery, Result<Guid>>
{
    private readonly IUserRepository _userRepository;

    public ResolveCustomerIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(ResolveCustomerIdQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return Result.Failure<Guid>("User not found.");

        if (user.CustomerId is null)
            return Result.Failure<Guid>("No customer profile linked to this account.");

        return Result.Success(user.CustomerId.Value);
    }
}