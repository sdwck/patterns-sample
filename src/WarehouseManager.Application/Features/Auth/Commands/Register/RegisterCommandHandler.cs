using MediatR;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _uow = uow;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _userRepository.ExistsAsync(request.Email, ct))
            return Result.Failure<AuthResponse>("Email already taken.");

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };
        await _uow.Customers.AddAsync(customer, ct);
        await _uow.SaveChangesAsync(ct);

        var passwordHash = _passwordHasher.Hash(request.Password);
        var userId = await _userRepository.CreateAsync(
            new CreateUserRequest(request.Email, passwordHash, request.FirstName, request.LastName, "Customer",
                customer.Id), ct);

        var token = _jwtTokenGenerator.GenerateToken(userId, request.Email, "Customer");
        return Result.Success(new AuthResponse(token, request.Email, "Customer"));
    }
}