# FleksProfit API

This project develops an ASP.NET Core Web API backend functioning as a calculator to estimate profit from offering capacity as a system service, specifically frequency control. Earnings come from up/down-regulation, but costs such as buying electricity, battery losses, lost production, startup expenses, and price volatility reduce profit.

## Technology Stack

- **Framework:** ASP.NET Core Web API (.NET 8)
- **Database:** Entity Framework Core with SQL Server
- **API Documentation:** Swagger/OpenAPI
- **Architecture:** Model-Service-Controller pattern
- **External APIs:**
  - Energinet API (Energidataservice) - System performance data
  - Electricity spot prices data

## Features

- ✅ RESTful API with Swagger documentation
- ✅ Integration with Energinet's Energidataservice API
- ✅ Electricity price data retrieval
- ✅ Profit calculation engine with battery loss and startup costs
- ✅ Historical data storage with EF Core
- ✅ CORS support for Blazor frontend integration
- ✅ Comprehensive data models for system performance and pricing

## API Endpoints

### Profit Calculation
- **POST** `/api/Profit/calculate` - Calculate profit from capacity offering

### System Data
- **GET** `/api/SystemData/performance` - Get system performance data from Energinet
- **GET** `/api/SystemData/prices` - Get electricity spot prices

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 Community Edition (recommended) or any code editor

### Installation

1. Clone the repository:
```bash
git clone https://github.com/eliaskkj4/5.SemHOPFleksProfit.git
cd 5.SemHOPFleksProfit
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update database connection string in `FleksProfit.API/appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FleksProfitDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

4. Apply database migrations:
```bash
cd FleksProfit.API
dotnet ef database update
```

5. Run the application:
```bash
dotnet run
```

6. Access Swagger UI at: `http://localhost:5249` (or the port specified in your launch settings)

## Project Structure

```
FleksProfit.API/
├── Controllers/          # API Controllers
│   ├── ProfitController.cs
│   └── SystemDataController.cs
├── Services/            # Business logic services
│   ├── EnerginetService.cs
│   ├── ElectricityPriceService.cs
│   └── ProfitCalculationService.cs
├── Models/              # Data models
│   ├── SystemPerformance.cs
│   ├── ElectricityPrice.cs
│   └── ProfitCalculation.cs
├── DTOs/                # Data Transfer Objects
│   ├── ProfitCalculationRequest.cs
│   └── ProfitCalculationResponse.cs
├── Data/                # EF Core DbContext
│   └── FleksProfitDbContext.cs
└── Migrations/          # EF Core migrations
```

## Usage Example

### Calculate Profit

```http
POST /api/Profit/calculate
Content-Type: application/json

{
  "capacity": 100,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "area": "DK1",
  "batteryLossPercentage": 15.0,
  "startupCost": 1000.0
}
```

Response:
```json
{
  "capacity": 100,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "area": "DK1",
  "totalRevenue": 50000.0,
  "electricityCost": 30000.0,
  "batteryLoss": 4500.0,
  "startupCost": 1000.0,
  "netProfit": 14500.0,
  "dailyDetails": [
    {
      "date": "2024-01-01T00:00:00Z",
      "revenue": 1612.90,
      "cost": 1096.77,
      "profit": 516.13
    }
  ]
}
```

## Configuration

### Database
The application uses SQL Server LocalDB by default. For production, update the connection string in `appsettings.json`.

### External APIs
- **Energinet API:** Uses the public Energidataservice API at `https://api.energidataservice.dk/dataset/`
- No API keys required for basic usage

## Development

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Creating New Migrations
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Integration with Blazor Frontend

The API includes CORS support configured to allow requests from Blazor WebAssembly applications. The frontend can consume these endpoints to provide a user interface for profit calculations.

## Screenshots

### Swagger UI
![Swagger UI](https://github.com/user-attachments/assets/37da7f32-8bf2-4ca6-9b33-fa55ca9c7f6d)

### API Endpoint Details
![API Endpoint Details](https://github.com/user-attachments/assets/05f77454-ef8c-4263-b0c4-9b09c80e226d)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is part of a semester project for educational purposes.

## Authors

- Project Team - 5th Semester HOP FleksProfit

## Acknowledgments

- Energinet for providing the Energidataservice API
- Danish electricity market data providers
