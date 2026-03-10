![CI](https://github.com/konradruta/ECommerceOrdersAPI/actions/workflows/azure-deploy.yml/badge.svg)

# ECommerceOrdersAPI
REST API stworzone w ASP.NET Core Web API, umożliwiające zarządzanie produktami oraz zamówieniami w systemie e-commerce.
Aplikacja obsługuje operacje CRUD dla produktów i zamówień oraz relację wiele-do-wielu pomiędzy zamówieniami i produktami.
Projekt został przygotowany jako zadanie rekrutacyjne.

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

## CI/CD
Projekt wykorzystuje GitHub Actions do automatycznego budowania aplikacji.
Pipeline uruchamia się po każdym pushu do gałęzi main i wykonuje:
- restore zależności
- build projektu
- uruchomienie testów

## Uruchomienie lokalnie
1. Sklonuj repozytorium
2. Skonfiguruj connection string w `appsettings.json`
3. Wykonaj migracje bazy danych
4. Uruchom aplikację
5. Otwórz Swagger:
https://localhost:7264/swagger

## Cel projektu
Projekt przygotowany jako zadanie rekrutacyjne na stanowisko Junior .NET Developer.
