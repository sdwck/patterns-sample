namespace WarehouseManager.Application.DTOs;

public record UserDto(Guid Id, string Email, string FirstName, string LastName, string Role, Guid? CustomerId);

public record CreateUserRequest(
    string Email,
    string PasswordHash,
    string FirstName,
    string LastName,
    string Role,
    Guid? CustomerId);