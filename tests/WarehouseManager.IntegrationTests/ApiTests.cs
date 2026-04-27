// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using WarehouseManager.Application.DTOs;

// namespace WarehouseManager.IntegrationTests;

// public class ApiTests : IClassFixture<CustomWebAppFactory>
// {
//     private readonly HttpClient _client;

//     public ApiTests(CustomWebAppFactory factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GetProducts_ReturnsOk()
//     {
//         var r = await _client.GetAsync("/api/products");
//         r.StatusCode.Should().Be(HttpStatusCode.OK);
//     }

//     [Fact]
//     public async Task Login_ValidCreds_ReturnsToken()
//     {
//         var r = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest("admin@warehouse.com", "admin123"));
//         r.StatusCode.Should().Be(HttpStatusCode.OK);
//         var auth = await r.Content.ReadFromJsonAsync<AuthResponse>();
//         auth!.Token.Should().NotBeNullOrEmpty();
//     }

//     [Fact]
//     public async Task GetCategories_ReturnsOk()
//     {
//         var r = await _client.GetAsync("/api/categories");
//         r.StatusCode.Should().Be(HttpStatusCode.OK);
//     }
// }