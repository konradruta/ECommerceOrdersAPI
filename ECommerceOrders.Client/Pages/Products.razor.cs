using ECommerceOrders.Client.Models;
using ECommerceOrders.Client.Pages.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ECommerceOrders.Client.Pages
{
    public partial class Products
    {
        private PagedResult<Product>? result;
        private string searchPhase = string.Empty;
        private CancellationTokenSource? cancellationTokenSource;
        private int pageSize = 5;
        private readonly int[] pageSizeOptions = [5, 10, 20, 50];
        private int currentPage = 1;

        private List<Product> products => result?.Items ?? [];
        private int totalItems => result?.TotalCount ?? 0;
        private int TotalPages => Math.Max(1, result?.TotalPages ?? 1);
        private bool hasNextPage => result?.HasNextPage ?? false;

        protected override async Task OnInitializedAsync()
        {
            await LoadPageAsync(1);
        }

        private async Task LoadPageAsync(int page)
        {
            try
            {
                currentPage = page;
                result = await ProductService.GetProductsAsync(searchPhase, page, pageSize);

                // Jeśli strona już nie istnieje (np. po usunięciu ostatniego rekordu), cofnij na ostatnią
                if (result.Items.Count == 0 && result.TotalPages > 0 && page > result.TotalPages)
                {
                    currentPage = result.TotalPages;
                    result = await ProductService.GetProductsAsync(searchPhase, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Błąd przy ładowaniu produktów: {ex.Message}");
                result = new PagedResult<Product> { PageNumber = page, PageSize = pageSize };
            }
        }

        private async Task OnPageSizeChanged(int newSize)
        {
            pageSize = newSize;
            await LoadPageAsync(1); 
        }

        private async Task OnPageChanged(int page)
        {
            await GoToPageAsync(page);
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

            var dialogResult = await dialog.Result;

            if (dialogResult.Canceled)
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

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled && dialogResult.Data is Product editedProduct)
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

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled && dialogResult.Data is Product product)
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