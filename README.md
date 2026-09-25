![CI](https://github.com/konradruta/ECommerceOrdersAPI/actions/workflows/azure-deploy.yml/badge.svg)

# ECommerceOrders

System e-commerce złożony z aplikacji ASP.NET Core Web API oraz klienta Blazor WebAssembly, umożliwiający zarządzanie produktami oraz zamówieniami.

Projekt pozwala na wykonywanie operacji CRUD, obsługę relacji wiele-do-wielu, wyszukiwanie żywe oraz paginację danych. Został przygotowany jako rozwinięcie zadania rekrutacyjnego na stanowisko Junior .NET Developer.

## Publiczne API
- Swagger UI: https://ecommerceorders-app-ehdzhbgkbshccbe4.westeurope-01.azurewebsites.net/swagger/index.html

## Wykorzystane technologie
- Backend: .NET 8, ASP.NET Core Web API, Entity Framework Core, AutoMapper, SQL Server / Azure SQL, Swagger / OpenAPI
- Frontend: Blazor WebAssembly, MudBlazor, HttpClient / System.Net.Http.Json
- Chmura i CI/CD: Azure App Service, Azure SQL Database, GitHub Actions

## Funkcjonalności

### Produkty
- Pobieranie stronnicowanej listy produktów z API
- Dynamiczne wyszukiwanie z filtrowaniem
- Wybór liczby elementów na stronie za pomocą MudChipSet
- Dodawanie, edycja oraz usuwanie produktów z potwierdzeniem (MudDialog)
- Powiadomienia w czasie rzeczywistym (MudSnackbar)

### Zamówienia
- Pobieranie listy zamówień wraz ze statusami i wartością
- Tworzenie i edycja zamówień (wybór produktów i ich ilości)
- Zmiana statusu zamówienia oraz usuwanie zamówień z potwierdzeniem

## Integracja z API

Aplikacja Blazor WebAssembly komunikuje się z REST API za pomocą dedykowanych serwisów:
```text
Blazor WebAssembly (ECommerceOrders.Client)
    │
    ├── ProductService  -> /api/products
    └── OrderService    -> /api/orders
```

## Endpointy API
### Orders
- GET `/api/orders` – pobranie listy wszystkich zamówień
- GET `/api/orders/{id}` – pobranie zamówienia po ID
- POST `/api/orders` – utworzenie nowego zamówienia
- PUT `/api/orders/{id}` – edycja zamówienia
- DELETE `/api/orders/{id}` – usunięcie zamówienia

### Products
- GET `/api/products` – pobranie listy wszystkich produktów
- GET `/api/products/{id}` – pobranie produktu po ID
- POST `/api/products` – dodanie nowego produktu
- PUT `/api/products/{id}` – edycja produktu
- DELETE `/api/products/{id}` – usunięcie produktu

## CI/CD
Projekt wykorzystuje GitHub Actions do automatycznego budowania
i wdrażania aplikacji do Azure App Service.

Workflow znajduje się w:
.github/workflows/azure-deploy.yml

## Uruchomienie lokalnie
1. Sklonuj repozytorium
2. Skonfiguruj connection string w `appsettings.json`
3. Wykonaj migracje bazy danych: dotnet ef database update
4. Upewnij się, że adres API jest skonfigurowany w Program.cs projektu Blazor.
5. Uruchom projekty ECommerceOrdersAPI oraz ECommerceOrders.Client.
