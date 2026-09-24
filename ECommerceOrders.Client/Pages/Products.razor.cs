using ECommerceOrders.Client.Models;
using ECommerceOrders.Client.Pages.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;


namespace ECommerceOrders.Client.Pages
{
    public  partial class Products
    {
        [Inject]
        private HttpClient Http { get; set; } = default!;
        private List<Product>? products;
        private int page = 1;
        private int pageSize = 10;
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private int totalCount = 0;
        private int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);
        private int currentPage = 1;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
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
            var product = products?.FirstOrDefault(p => p.Id == productId);

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
                return;

            var success = await ProductService.DeleteProduct(productId);

            if (success)
            {
                await LoadProductsAsync();

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
                    await LoadProductsAsync();

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
                    await LoadProductsAsync();

                    Snackbar.Add("Produkt został dodany.", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Wystąpił błąd podczas dodawania produktu.", Severity.Error);
                }
            }
        }

        private string searchPhase = "";
        private CancellationTokenSource? cancellationTokenSource;

        private async Task OnSearchInput()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            if (string.IsNullOrEmpty(searchPhase) || searchPhase.Trim().Length < 3)
            {
                // jeśli brak zapytania lub zbyt krótki string - załaduj normalną listę
                await LoadProductsAsync();
                return;
            }

            try
            {
                await Task.Delay(300, token); // Debounce for 300ms
                page = 1;
                products = await ProductService.GetProductsAsync(page, pageSize, searchPhase);
                StateHasChanged();
            }
            catch (TaskCanceledException)
            {
                // użytkownik pisze dalej, ignorujemy poprzedni request
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private async Task PrevPage()
        {
            if (page <= 1) return;
            page--;
            await LoadProductsAsync();
        }

        private async Task NextPage()
        {
            page++;
            await LoadProductsAsync();
        }

        private async Task SetPageSize(int size)
        {
            pageSize = size;
            page = 1;
            await LoadProductsAsync();
        }

        private async Task OnPageSizeChanged()
        {
            // @bind-Value:after does not provide the new value, so use the current pageSize
            await SetPageSize(pageSize);
        }
    }
}
