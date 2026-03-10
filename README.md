![CI](https://github.com/konradruta/ECommerceOrdersAPI/actions/workflows/azure-deploy.yml/badge.svg)

# ECommerceOrdersAPI
REST API stworzone w ASP.NET Core Web API, umożliwiające zarządzanie produktami oraz zamówieniami w systemie e-commerce.
Aplikacja obsługuje operacje CRUD dla produktów i zamówień oraz relację wiele-do-wielu pomiędzy zamówieniami i produktami.
Projekt został przygotowany jako zadanie rekrutacyjne.

## Publiczne API
Aplikacja została wdrożona w Azure App Service i jest dostępna publicznie pod adresem dla Swagger UI:
https://ecommerceorders-app-ehdzhbgkbshccbe4.westeurope-01.azurewebsites.net/swagger/index.html

## Wykorzystane usługi Azure
Projekt wykorzystuje następujące usługi w chmurze Microsoft Azure:

- Azure App Service – hosting aplikacji Web API
- Azure SQL Database – baza danych aplikacji
- Azure App Service Deployment Center – integracja z GitHub

## Konfiguracja
Connection string bazy danych nie jest przechowywany w repozytorium.

## Funkcjonalności
- dodawanie, edytowanie, usuwanie i pobieranie produktów
- tworzenie i zarządzanie zamówieniami
- obsługę relacji wiele-do-wielu między zamówieniami i produktami
- asynchroniczne operacje na bazie danych
- globalną obsługę błędów

## Technologie
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- AutoMapper
- SQL Server
- Swagger / OpenAPI
- GitHub Actions (CI/CD)

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
3. Wykonaj migracje bazy danych
4. Uruchom aplikację
5. Otwórz Swagger:
https://localhost:7264/swagger

## Cel projektu
Projekt przygotowany jako zadanie rekrutacyjne na stanowisko Junior .NET Developer.
