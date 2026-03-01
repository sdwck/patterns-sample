using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Export;
using WarehouseManager.Application.Features.Stock.Commands.RestockProduct;
using WarehouseManager.Application.Features.Stock.Queries.GetLowStock;
using WarehouseManager.Application.Services;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _uow;

    public StockController(IMediator mediator, IUnitOfWork uow)
    {
        _mediator = mediator;
        _uow = uow;
    }

    [HttpGet("low")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetLowStock()
    {
        return Ok(await _mediator.Send(new GetLowStockQuery()));
    }

    [HttpPost("restock")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Restock([FromBody] RestockProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.Error });
    }

    [HttpGet("report")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetStockReport()
    {
        var report = await BuildReport();
        return Ok(report);
    }

    [HttpGet("report/export")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ExportStockReport([FromQuery] string format = "json")
    {
        var report = await BuildReport();

        var exporter = ReportExporterFactory.Create(format);
        var exported = exporter.Export(report);

        return Content(exported.Content, exported.ContentType);
    }

    private async Task<StockReport> BuildReport()
    {
        var allStock = await _uow.Stock.GetAllAsync();

        var builder = new StockReportBuilder()
            .WithTitle("Warehouse Stock Report")
            .GeneratedAt(DateTime.UtcNow)
            .AddItems(allStock.Select(s => new StockReportItem
            {
                ProductName = s.Product.Name,
                Sku = s.Product.Sku,
                Quantity = s.QuantityOnHand,
                Location = s.WarehouseLocation,
                IsLowStock = s.QuantityOnHand <= s.ReorderLevel
            }));

        return builder.Build();
    }
}