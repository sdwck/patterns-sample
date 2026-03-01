namespace WarehouseManager.Application.DTOs;

public record LoginRequest(string Email, string Password);

public record RegisterRequest(string Email, string Password, string FirstName, string LastName);

public record AuthResponse(string Token, string Email, string Role);

public record UpdateStatusRequest(string Action);