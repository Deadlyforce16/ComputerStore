# ComputerStore Web API

## Overview
A Internet Services Project made with a ASP.NET Core Web API project template for a computer store.

## Getting Started

### Prerequisites
- .NET 8+ SDK
- Visual Studio 2022+
- SQL Server (localdb or full)

### Setup
1. Clone the repository and open the solution in Visual Studio.
2. Ensure your `appsettings.json` has the correct connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ComputerStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
3. Open **Package Manager Console**:
   - Set Default Project: `ComputerStore.Infrastructure`
   - Run:
     ```
     Add-Migration InitialCreate -StartupProject ComputerStore
     Update-Database -StartupProject ComputerStore
     ```
   - This creates and migrates the database.

### Running the API
- Set `ComputerStore` as startup project.
- Press F5 or run:
  ```
  dotnet run --project ComputerStore/ComputerStore.csproj
  ```
- Swagger UI: [https://localhost:5515/swagger](https://localhost:5515/swagger)

## Project Structure
- `Domain`: Entities, exceptions
- `Application`: DTOs, interfaces, services, mappings
- `Infrastructure`: DbContext, repositories, EF configs, service implementations
- `Controllers`: API endpoints
- `UnitTests`/`IntegrationTests`: Test projects

## Author
Prohor Muchev 5515 
