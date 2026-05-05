# Currency Exchange Office — System Documentation

## Architecture Overview

The system consists of four components:

- **CurrencyExchange.Service** — WCF service contracts and business logic
- **CurrencyExchange.Host** — ASP.NET Core host exposing the CoreWCF service
- **CurrencyExchange.Data** — EF Core DbContext and PostgreSQL entities
- **CurrencyExchange.UI** — Avalonia desktop client

## Service Operations

| Method | Description |
|--------|-------------|
| GetExchangeRate(currency) | Current rate from NBP API |
| GetHistoricalRate(currency, date) | Rate on a specific date |
| GetRatesForDateRange(currency, start, end) | Rates over a date range |
| Register(username, password) | Create a new user account |
| Login(username, password) | Authenticate a user |
| TopUp(username, amount) | Add PLN to user balance |
| GetBalances(username) | List all currency balances |
| BuyCurrency(username, currency, amount) | Purchase foreign currency |
| SellCurrency(username, currency, amount) | Sell foreign currency |
| GetTransactionHistory(username) | List all past transactions |

## Database Schema

Three tables:
- **Users** — id, username, password_hash, created_at
- **Balances** — id, user_id, currency, amount
- **Transactions** — id, user_id, type, currency, amount, rate, date

## External API

Exchange rates are fetched from the National Bank of Poland (NBP) API:
- Documentation: http://api.nbp.pl/en.html
- No authentication required
- Supports current and historical rates

## How to Run

### Prerequisites
- .NET 8 SDK
- PostgreSQL (Docker recommended)
- Docker: `docker run --name my-postgres -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d postgres`

### 1. Create the database
```bash
docker exec -it my-postgres psql -U postgres -c "CREATE DATABASE currency_exchange;"
```

### 2. Update connection string
Edit `WCF-Service/CurrencyExchange.Host/appsettings.json` with your PostgreSQL credentials.

### 3. Start the service
```bash
cd WCF-Service/CurrencyExchange.Host
dotnet run
```

### 4. Start the UI
```bash
cd Client-Application/CurrencyExchange.UI
dotnet run
```