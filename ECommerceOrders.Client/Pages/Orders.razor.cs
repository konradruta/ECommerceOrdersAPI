using ECommerceOrders.Client.Models;
using ECommerceOrders.Client.Pages.Dialogs;
using MudBlazor;

namespace ECommerceOrders.Client.Pages
{
    public partial class Orders
    {
        private List<Order>? orders;

        private string searchPhase = "";

        protected override async Task OnInitializedAsync()
        {
            orders = await OrderService.GetOrdersAsync();
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
                orders = await OrderService.GetOrdersAsync();

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
                orders = await OrderService.GetOrdersAsync();

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
                orders = await OrderService.GetOrdersAsync();

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
