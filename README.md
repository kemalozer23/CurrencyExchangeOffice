# Currency Exchange Office

**Course:** Network Application Development  
**Author:** Kemal Ozer  
**Student ID:** 73799

## Description
A network-based currency exchange office system built with CoreWCF on .NET 8.
The system simulates an online currency exchange office: users register, top up a
PLN balance, and buy/sell foreign currencies at live rates from the National Bank
of Poland (NBP) API. It consists of a WCF service, a PostgreSQL database, and an
Avalonia desktop client.

## Components
- **WCF-Service/CurrencyExchange.Service** — WCF service contracts and business logic
- **WCF-Service/CurrencyExchange.Host** — host exposing the CoreWCF service over HTTP
- **WCF-Service/CurrencyExchange.Data** — EF Core data layer (PostgreSQL)
- **WCF-Service/CurrencyExchange.Client** — console client (service consumption demo)
- **Client-Application/CurrencyExchange.UI** — Avalonia desktop client

## Prerequisites
- .NET 8 SDK
- PostgreSQL (Docker recommended)

## Setup

### 1. Start PostgreSQL and create the database
```bash
docker run --name my-postgres -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d postgres
docker exec -it my-postgres psql -U postgres -c "CREATE DATABASE currency_exchange;"
```

### 2. Configure the connection string
Edit `WCF-Service/CurrencyExchange.Host/appsettings.json` and set your PostgreSQL
password in the `DefaultConnection` string (replace `YOUR_PASSWORD`).

The database schema is created automatically on first run via EF Core migrations.

### 3. Start the service (Terminal 1)
```bash
cd WCF-Service/CurrencyExchange.Host
dotnet run
```
The service listens on `http://localhost:5050`.

### 4. Run a client

**Desktop UI (Avalonia):**
```bash
cd Client-Application/CurrencyExchange.UI
dotnet run
```

**Or the console client:**
```bash
cd WCF-Service/CurrencyExchange.Client
dotnet run
```

## Documentation
See [Documentation/README.md](Documentation/README.md) for system architecture,
the full list of service operations, and the database schema.