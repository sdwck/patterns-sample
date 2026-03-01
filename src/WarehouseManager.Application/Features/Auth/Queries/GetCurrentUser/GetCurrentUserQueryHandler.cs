using MediatR;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return Result.Failure<UserDto>("User not found.");

        return Result.Success(user);
    }
}