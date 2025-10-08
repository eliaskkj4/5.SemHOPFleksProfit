# FleksProfit API Test Examples

## Test the API endpoints using curl or your favorite HTTP client

### 1. Get System Performance Data
```bash
curl -X GET "http://localhost:5249/api/SystemData/performance?startDate=2024-01-01T00:00:00Z&endDate=2024-01-02T00:00:00Z&area=DK1" \
  -H "accept: application/json"
```

### 2. Get Electricity Prices
```bash
curl -X GET "http://localhost:5249/api/SystemData/prices?startDate=2024-01-01T00:00:00Z&endDate=2024-01-02T00:00:00Z&area=DK1" \
  -H "accept: application/json"
```

### 3. Calculate Profit
```bash
curl -X POST "http://localhost:5249/api/Profit/calculate" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "capacity": 100,
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-01-07T23:59:59Z",
    "area": "DK1",
    "batteryLossPercentage": 15.0,
    "startupCost": 1000.0
  }'
```

## PowerShell Examples

### Get System Performance Data
```powershell
Invoke-RestMethod -Uri "http://localhost:5249/api/SystemData/performance?startDate=2024-01-01T00:00:00Z&endDate=2024-01-02T00:00:00Z&area=DK1" `
  -Method Get `
  -Headers @{"accept"="application/json"}
```

### Calculate Profit
```powershell
$body = @{
    capacity = 100
    startDate = "2024-01-01T00:00:00Z"
    endDate = "2024-01-07T23:59:59Z"
    area = "DK1"
    batteryLossPercentage = 15.0
    startupCost = 1000.0
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5249/api/Profit/calculate" `
  -Method Post `
  -Body $body `
  -ContentType "application/json" `
  -Headers @{"accept"="application/json"}
```

## Notes
- Replace `localhost:5249` with your actual API URL and port
- Dates should be in ISO 8601 format
- Area can be "DK1" or "DK2" for Danish price areas
- The API requires valid date ranges (startDate < endDate)
