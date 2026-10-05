using ECommerceOrders.Client.Models;
using ECommerceOrders.Client.Pages.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ECommerceOrders.Client.Pages
{
    public partial class Products
    {
        private List<Product> products = [];
        private string searchPhase = string.Empty;
        private CancellationTokenSource? cancellationTokenSource;
        private const int pageSize = 10;
        private int currentPage = 1;
        private int totalItems;

        private int TotalPages => Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));

        protected override async Task OnInitializedAsync()
        {
            await LoadPageAsync(1);
        }

        private async Task LoadPageAsync(int page)
        {
            currentPage = page;
            totalItems = await ProductService.GetProductsCountAsync(searchPhase);
            products = await ProductService.GetProductsAsync(page, pageSize, searchPhase);
        }

        private async Task GoToPageAsync(int page)
        {
            if (page < 1 || page > TotalPages || page == currentPage)
            {
                return;
            }

            await LoadPageAsync(page);
        }

        private IEnumerable<int> GetPageNumbers()
        {
            return Enumerable.Range(1, TotalPages);
        }

        private async Task LoadProductsAsync()
        {
            products = await ProductService.GetProductsAsync(page, pageSize, searchPhase);
            StateHasChanged();
        }

        // Dodaj tę właściwość do sprawdzania, czy jest kolejna strana:
        private bool hasNextPage => products != null && products.Count == pageSize;

        private async Task DeleteProduct(int productId)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return;
            }

            var parameters = new DialogParameters
            {
                {"Message", $"Czy na pewno chcesz usunąć produkt „{product.Name}”?"}
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>(
                "Potwierdzenie usunięcia",
                parameters,
                options);

            var result = await dialog.Result;

            if (result.Canceled)
            {
                return;
            }

            var success = await ProductService.DeleteProduct(productId);

            if (success)
            {
                await LoadPageAsync(currentPage);
                Snackbar.Add("Produkt został usunięty.", Severity.Info);
            }
            else
            {
                Snackbar.Add("Błąd, produkt nie został usunięty.", Severity.Error);
            }
        }

        private async Task EditProduct(Product product)
        {
            var parameters = new DialogParameters
            {
                { "Product", product }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<EditProductDialog>(
                "Edytuj produkt",
                parameters,
                options);

            var result = await dialog.Result;

            if (!result.Canceled && result.Data is Product editedProduct)
            {
                var success = await ProductService.EditProduct(editedProduct);

                if (success)
                {
                    await LoadPageAsync(currentPage);
                    Snackbar.Add("Produkt został zaktualizowany.", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Wystąpił błąd podczas aktualizacji produktu.", Severity.Error);
                }
            }
        }

        private async Task AddProduct()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<AddProductDialog>(
                "Dodaj produkt",
                options
            );

            var result = await dialog.Result;

            if (!result.Canceled && result.Data is Product product)
            {
                var success = await ProductService.AddProduct(product);

                if (success)
                {
                    await LoadPageAsync(currentPage);
                    Snackbar.Add("Produkt został dodany.", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Wystąpił błąd podczas dodawania produktu.", Severity.Error);
                }
            }
        }

        private async Task OnSearchInput()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            try
            {
                await Task.Delay(300, token);
                await LoadPageAsync(1);
            }
            catch (TaskCanceledException)
            {
                // użytkownik pisze dalej, ignorujemy poprzedni request
            }
        }
    }
}
