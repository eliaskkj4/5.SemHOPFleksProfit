# Development Guide - FleksProfit API

## Overview
This guide provides information for developers working on the FleksProfit API project.

## Architecture

The project follows a clean architecture pattern with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│           Controllers Layer                 │
│  (HTTP Request Handling & Routing)          │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│           Services Layer                    │
│  (Business Logic & External API Calls)      │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│           Data Layer                        │
│  (EF Core DbContext & Database Access)      │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│           SQL Server Database               │
└─────────────────────────────────────────────┘
```

## Project Components

### Controllers
- **ProfitController**: Handles profit calculation requests
- **SystemDataController**: Provides access to system performance and price data

### Services
- **IEnerginetService / EnerginetService**: Fetches system performance data from Energinet API
- **IElectricityPriceService / ElectricityPriceService**: Fetches electricity spot prices
- **IProfitCalculationService / ProfitCalculationService**: Calculates profit based on capacity and market data

### Models
- **SystemPerformance**: Historical system performance data
- **ElectricityPrice**: Electricity spot price data
- **ProfitCalculation**: Calculated profit results

### DTOs (Data Transfer Objects)
- **ProfitCalculationRequest**: Input for profit calculation
- **ProfitCalculationResponse**: Output with profit details and daily breakdown

## External API Integration

### Energinet API (Energidataservice)
- **Base URL**: `https://api.energidataservice.dk/dataset/`
- **Datasets Used**:
  - `Regulating_Prices`: Up/down regulation prices
  - `Elspotprices`: Spot electricity prices

### API Query Parameters
- `start`: Start date/time in ISO 8601 format
- `end`: End date/time in ISO 8601 format
- `filter`: JSON filter for price area (DK1 or DK2)

Example:
```
https://api.energidataservice.dk/dataset/Elspotprices?start=2024-01-01T00:00&end=2024-01-02T00:00&filter={"PriceArea":"DK1"}
```

## Database Schema

### SystemPerformances Table
- `Id` (int, PK)
- `Timestamp` (datetime)
- `Area` (nvarchar(50))
- `UpRegulation` (float)
- `DownRegulation` (float)
- `Frequency` (float)
- `CapacityUtilization` (float)

### ElectricityPrices Table
- `Id` (int, PK)
- `Timestamp` (datetime)
- `Area` (nvarchar(50))
- `SpotPrice` (float)
- `Currency` (nvarchar(10))

### ProfitCalculations Table
- `Id` (int, PK)
- `CalculationDate` (datetime)
- `Capacity` (float)
- `Revenue` (float)
- `ElectricityCost` (float)
- `BatteryLoss` (float)
- `StartupCost` (float)
- `TotalProfit` (float)
- `Area` (nvarchar(50))

## Profit Calculation Logic

The profit calculation considers multiple factors:

1. **Revenue Calculation**:
   ```
   Daily Revenue = Capacity × ((UpRegulation + DownRegulation) / 2) × 24 hours
   ```

2. **Cost Calculation**:
   ```
   Daily Cost = Capacity × Average Spot Price × 24 hours
   ```

3. **Battery Loss**:
   ```
   Battery Loss Cost = Daily Cost × (Battery Loss % / 100)
   ```

4. **Net Profit**:
   ```
   Net Profit = Total Revenue - Total Electricity Cost - Total Battery Loss - Startup Cost
   ```

## Development Workflow

### 1. Adding New Features

1. Create/update models in `Models/`
2. Update DbContext if database changes are needed
3. Create migration: `dotnet ef migrations add MigrationName`
4. Implement service interface in `Services/`
5. Implement service logic
6. Register service in `Program.cs`
7. Create/update controller endpoint
8. Test with Swagger UI

### 2. Database Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

Rollback migration:
```bash
dotnet ef database update PreviousMigrationName
```

Remove last migration (if not applied):
```bash
dotnet ef migrations remove
```

### 3. Testing

#### Manual Testing with Swagger
1. Run the application: `dotnet run`
2. Navigate to `http://localhost:5249`
3. Use Swagger UI to test endpoints

#### Testing with curl
See `API-EXAMPLES.md` for curl examples

## Configuration

### appsettings.json
- `ConnectionStrings:DefaultConnection`: Database connection string
- `Logging`: Logging configuration
- `AllowedHosts`: Allowed HTTP host headers

### appsettings.Development.json
Development-specific settings (not committed to source control for sensitive data)

## Best Practices

1. **Dependency Injection**: All services are registered and injected via DI
2. **Async/Await**: All I/O operations use async methods
3. **Error Handling**: Controllers catch exceptions and return appropriate HTTP status codes
4. **Logging**: Use ILogger for all logging operations
5. **DTOs**: Use DTOs to control API contract and separate from internal models
6. **Validation**: Validate input in controllers before processing

## Common Issues

### Issue: Database connection fails
**Solution**: Check connection string in `appsettings.json` and ensure SQL Server is running

### Issue: Migration fails
**Solution**: 
1. Check DbContext configuration
2. Ensure no pending code changes
3. Delete `Migrations/` folder and recreate if needed

### Issue: External API returns no data
**Solution**:
1. Check date ranges are valid
2. Verify area code (DK1 or DK2)
3. Check internet connectivity
4. Review API documentation for changes

## Future Enhancements

Potential areas for improvement:
- Add unit tests for services
- Add integration tests for controllers
- Implement caching for external API calls
- Add authentication/authorization
- Implement rate limiting for API calls
- Add health check endpoints
- Implement background jobs for data refresh
- Add more detailed error responses
- Implement pagination for large data sets

## Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [Energinet API Documentation](https://www.energidataservice.dk/guides/api-guides)
- [Swagger/OpenAPI Documentation](https://swagger.io/docs/)
