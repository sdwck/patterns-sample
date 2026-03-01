using MediatR;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;

    public CreateCustomerCommandHandler(
        IUnitOfWork uow,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        if (await _userRepository.ExistsAsync(request.Email, ct))
            return Result.Failure<Guid>("Email already taken.");

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        };

        await _uow.Customers.AddAsync(customer, ct);
        await _uow.SaveChangesAsync(ct);

        var password = request.Password ?? "Welcome123!";
        var hash = _passwordHasher.Hash(password);

        await _userRepository.CreateAsync(
            new CreateUserRequest(
                request.Email,
                hash,
                request.FirstName,
                request.LastName,
                "Customer",
                customer.Id),
            ct);

        return Result.Success(customer.Id);
    }
}