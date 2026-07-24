# 🏭 WarehouseManager — Design Patterns & Clean Architecture PoC

> Full-stack warehouse & order management system built to demonstrate **Clean Architecture**, **Gang of Four (GoF) design patterns**, and **CQRS with MediatR** in .NET 10, backed by a modern React dashboard.

## Architecture

```
┌─────────────────────────────────────────────────┐
│                  Host / Api                     │
│ ASP.NET Core · Controllers · JWT Auth · OpenAPI │
├─────────────────────────────────────────────────┤
│                 Application                     │
│  MediatR CQRS · Pipeline Behaviors · DTOs       │
│  Export Formatters · Report Builders & Director │
├─────────────────────────────────────────────────┤
│                   Domain                        │
│   Entities · Aggregates · State Machine         │
│   Discount Strategies · Custom Iterators        │
├─────────────────────────────────────────────────┤
│               Infrastructure                    │
│    EF Core · Npgsql · Unit of Work · Auth       │
│    Notifications · Domain Event Handlers        │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│                    Client                       │
│   React 19 · TypeScript · TanStack Query        │
│   Zustand · Tailwind CSS · Vite                 │
│                                                 │
│   Admin:    Dashboard, Products, Orders, Reports│
│   Customer: Catalog, Cart, Checkout, My Orders  │
└─────────────────────────────────────────────────┘
```

## Design & Architectural Patterns Implemented

### Gang of Four (GoF) Patterns

| Pattern | Type | Location | Usage |
|---------|------|----------|-------|
| **Strategy** | Behavioral | `Domain/Strategies` | Dynamic discount strategies (`DiscountStrategyRegistry`) applied during checkout |
| **State** | Behavioral | `Domain/States` | Order lifecycle transitions (`Pending` → `Confirmed` → `Processing` → `Shipped` → `Delivered` / `Cancelled`) |
| **Observer** | Behavioral | `Domain/Observers` | Event-driven notifications on stock level changes & order updates |
| **Iterator** | Behavioral | `Domain/Iterators` | Custom traversal over hierarchical category trees |
| **Template Method** | Behavioral | `Application/Common` | `BaseCommandHandler<TCommand, TResult>` defining strict `ValidateAsync` → `ExecuteAsync` → `PostExecuteAsync` workflow |
| **Builder & Director** | Creational | `Application/Services` | `StockReportBuilder` & `StockReportDirector` constructing custom stock reports |
| **Factory Method** | Creational | `Application/Export` | `ReportExporterFactory` & `ReportExporter` creating CSV, JSON, and Plain Text report exporters |
| **Prototype** | Creational | `Domain/Entities` | `CloneAsNewOrder()` for duplicating past orders in 1-click reorder flow |
| **Singleton** | Creational | `Domain/Strategies` | `DiscountStrategyRegistry.Instance` for thread-safe strategy lookup |
| **Facade** | Structural | `Application/Services` | `Facade` simplifying complex order placement (resolves stock, charges payment, saves DB, sends email) |

### Architectural & Enterprise Patterns

| Pattern | Location | Usage |
|---------|----------|-------|
| **CQRS** | `Application/Features` | Clear separation of Read/Write operations via **MediatR** Commands & Queries |
| **Pipeline Behaviors** | `Application/Common/Behaviours` | Cross-cutting concerns via `LoggingBehaviour` and `ValidationBehaviour` (**FluentValidation**) |
| **Unit of Work & Repository** | `Domain/Interfaces` & `Infrastructure` | `IUnitOfWork` coordinating multi-repository transactions |
| **Domain Events** | `Domain/Events` & `Infrastructure/Events` | Decoupled side-effects on entity mutations |

## Tech Stack

- **Backend:** .NET 10, ASP.NET Core Controllers, EF Core (Npgsql / PostgreSQL), MediatR, FluentValidation, JWT Bearer Auth, Swagger / OpenAPI
- **Frontend:** React 19, TypeScript, TanStack Query (React Query), Zustand, Tailwind CSS, Lucide Icons, Vite

## Key Features

- **Admin Dashboard** — Revenue & order metrics, stock alerts, category hierarchy management, inventory restocking.
- **Flexible Report Exporters** — Export stock telemetry to **CSV**, **JSON**, or **Plain Text**.
- **Customer Storefront** — Product search, shopping cart with persistent state, checkout with discount strategy selection, 1-click reorder from order history.

## Project Structure

```
src/
├── WarehouseManager.Domain/          # Entities, States, Strategies, Iterators, Observers, Interfaces
├── WarehouseManager.Application/     # CQRS Commands & Queries, Behaviors, Export Formatters, Facade, Directors
├── WarehouseManager.Infrastructure/  # EF Core DbContext, Repositories, UnitOfWork, Auth, Email Notifications
├── WarehouseManager.Api/             # API Controllers & Route Definitions
├── WarehouseManager.Host/            # App Startup & Dependency Injection Setup
└── WarehouseManager.Client/          # React SPA (Admin Console & Customer Storefront)
```

## License

[MIT](LICENSE)
