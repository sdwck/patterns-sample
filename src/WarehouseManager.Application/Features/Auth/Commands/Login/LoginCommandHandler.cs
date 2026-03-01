using MediatR;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var passwordValid = await _userRepository.VerifyPasswordAsync(request.Email, request.Password, ct);
        if (!passwordValid)
            return Result.Failure<AuthResponse>("Invalid credentials.");

        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null)
            return Result.Failure<AuthResponse>("Invalid credentials.");

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);
        return Result.Success(new AuthResponse(token, user.Email, user.Role));
    }
}