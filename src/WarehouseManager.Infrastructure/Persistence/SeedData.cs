using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // await context.Database.MigrateAsync();
        await context.Database.EnsureCreatedAsync();

        if (await context.Categories.AnyAsync())
            return;

        var electronics = new Category { Name = "Electronics", Description = "Electronic devices" };
        var computers = new Category
            { Name = "Computers", Description = "Laptops and desktops", ParentCategoryId = electronics.Id };
        var phones = new Category { Name = "Phones", Description = "Mobile phones", ParentCategoryId = electronics.Id };
        electronics.Add(computers);
        electronics.Add(phones);
        var clothing = new Category { Name = "Clothing", Description = "Apparel" };
        context.Categories.AddRange(electronics, computers, phones, clothing);

        var supplier1 = new Supplier { Name = "TechCorp", ContactEmail = "sales@techcorp.com", Phone = "+1234567890" };
        var supplier2 = new Supplier { Name = "FashionHub", ContactEmail = "info@fashionhub.com" };
        context.Suppliers.AddRange(supplier1, supplier2);

        var product1 = new Product
        {
            Name = "Laptop Pro 15", Sku = "LP-001", Price = 1299.99m, CategoryId = computers.Id,
            SupplierId = supplier1.Id
        };
        var product2 = new Product
        {
            Name = "Smartphone X", Sku = "SP-001", Price = 899.99m, CategoryId = phones.Id, SupplierId = supplier1.Id
        };
        var product3 = new Product
        {
            Name = "T-Shirt Basic", Sku = "TS-001", Price = 29.99m, CategoryId = clothing.Id, SupplierId = supplier2.Id
        };
        context.Products.AddRange(product1, product2, product3);

        context.Stocks.AddRange(
            new Stock { ProductId = product1.Id, QuantityOnHand = 50, ReorderLevel = 10, WarehouseLocation = "A1-01" },
            new Stock { ProductId = product2.Id, QuantityOnHand = 100, ReorderLevel = 20, WarehouseLocation = "A1-02" },
            new Stock
            {
                ProductId = product3.Id, QuantityOnHand = 200, ReorderLevel = 30, WarehouseLocation = "B2-01"
            });

        var adminCustomer = new Customer
        {
            FirstName = "Admin", LastName = "User", Email = "admin@warehouse.com", Phone = "+0000000000",
            Address = "HQ Office"
        };
        var managerCustomer = new Customer
        {
            FirstName = "John", LastName = "Doe", Email = "manager@warehouse.com", Phone = "+1111111111",
            Address = "123 Main St"
        };
        var customer1 = new Customer
            { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Address = "456 Oak Ave" };
        context.Customers.AddRange(adminCustomer, managerCustomer, customer1);

        var adminUser = new AppUser
        {
            Email = "admin@warehouse.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FirstName = "Admin",
            LastName = "User",
            Role = UserRole.Admin,
            CustomerId = adminCustomer.Id
        };

        var managerUser = new AppUser
        {
            Email = "manager@warehouse.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.Manager,
            CustomerId = managerCustomer.Id
        };

        var customerUser = new AppUser
        {
            Email = "jane@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
            FirstName = "Jane",
            LastName = "Smith",
            Role = UserRole.Customer,
            CustomerId = customer1.Id
        };

        context.Users.AddRange(adminUser, managerUser, customerUser);

        await context.SaveChangesAsync();
    }
}