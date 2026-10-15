using ECommerceOrders.Client.Models;
using ECommerceOrders.Client.Pages.Dialogs;
using ECommerceOrders.Client.Services;
using MudBlazor;

namespace ECommerceOrders.Client.Pages
{
    public partial class Orders
    {
        private List<Order>? orders => result?.Items ?? [];
        private PagedResult<Order>? result;
        private string searchPhase = string.Empty;
        private CancellationTokenSource? cancellationTokenSource;
        private int pageSize = 5;
        private readonly int[] pageSizeOptions = [5, 10, 20, 50];
        private int currentPage = 1;
        private int totalItems => result?.TotalCount ?? 0;
        private int TotalPages => Math.Max(1, result?.TotalPages ?? 1);
        private bool hasNextPage => result?.HasNextPage ?? false;

        protected override async Task OnInitializedAsync()
        {
            //orders = await OrderService.GetOrdersAsync();
            await LoadPageAsync(1);
        }

        private async Task LoadPageAsync(int page)
        {
            try
            {
                currentPage = page;
                result = await OrderService.GetOrdersAsync(searchPhase, page, pageSize);

                // Jeśli strona już nie istnieje (np. po usunięciu ostatniego rekordu), cofnij na ostatnią
                if (result.Items.Count == 0 && result.TotalPages > 0 && page > result.TotalPages)
                {
                    currentPage = result.TotalPages;
                    result = await OrderService.GetOrdersAsync(searchPhase, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Błąd przy ładowaniu zamówień: {ex.Message}");
                result = new PagedResult<Order> { PageNumber = page, PageSize = pageSize };
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

        private async Task DleteOrder(int orderId)
        {
            var order = orders?.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                return;

            var parameters = new DialogParameters
        {
            {"Message", $"Czy na pewno chcesz usunąć zamówienie o numerze {order.Id}?"}
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

            var success = await OrderService.DeleteOrder(orderId);
            if (success)
            {
                await LoadPageAsync(currentPage);

                Snackbar.Add("Zamówienie zostało usunięte", Severity.Warning);
            }
            else
            {
                Snackbar.Add("Nie udało usunąć się zamówienia", Severity.Error);
            }
        }

        private async Task EditOrder(Order order)
        {
            var parameters = new DialogParameters
        {
            {"Order", order}
        };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<EditOrderDialog>(
                "Edytuj produkt",
                parameters,
                options);

            var result = await dialog.Result;

            if (result.Canceled)
                return;

            if (result.Data is not Order editedOrder)
                return;

            var success = await OrderService.EditOrder(editedOrder);
            if (success)
            {
                await LoadPageAsync(currentPage);

                Snackbar.Add("Status zamówienia został zmieniony", Severity.Success);
            }
            else
            {
                Snackbar.Add("Nie udało zmienić się zamówienia", Severity.Error);
            }
        }


        private async Task AddOrder()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<AddOrderDialog>(
                "Dodaj zamówienie",
                options);

            var result = await dialog.Result;

            if (result.Canceled)
                return;

            if (result.Data is not CreateOrder order)
                return;

            var success = await OrderService.AddOrder(order);

            if (success)
            {
                await LoadPageAsync(currentPage);

                Snackbar.Add(
                    "Zamówienie zostało utworzone!",
                    Severity.Success);
            }
            else
            {
                Snackbar.Add(
                    "Nie udało się utworzyć zamówienia.",
                    Severity.Error);
            }
        }
    }
}
