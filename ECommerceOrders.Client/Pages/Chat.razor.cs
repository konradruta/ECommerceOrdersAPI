using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
namespace ECommerceOrders.Client.Pages;

public partial class Chat : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private List<ChatMessage> Messages { get; set; } = new();
    private string Input { get; set; } = string.Empty;
    private bool IsTyping { get; set; }

    private ElementReference _messagesContainer;
    private ElementReference _scrollAnchor;
    private IJSObjectReference? _module;

    // Grupuje kolejne wiadomości tego samego nadawcy, żeby nie powtarzać avatara/timestampu przy każdej
    private List<MessageGroup> GroupedMessages { get; set; } = new();

    private void RebuildGroups()
    {
        var groups = new List<MessageGroup>();

        foreach (var msg in Messages)
        {
            var last = groups.Count > 0 ? groups[^1] : null;

            if (last is not null && last.IsUser == msg.IsUser)
            {
                last.Messages.Add(msg);
            }
            else
            {
                groups.Add(new MessageGroup { IsUser = msg.IsUser, Messages = new List<ChatMessage> { msg } });
            }
        }

        GroupedMessages = groups;
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(Input))
        {
            return;
        }

        var text = Input.Trim();
        Input = string.Empty;

        Messages.Add(new ChatMessage { Text = text, IsUser = true });
        RebuildGroups();
        StateHasChanged();
        await ScrollToBottomAsync();

        IsTyping = true;
        StateHasChanged();
        await ScrollToBottomAsync();

        // TODO: podmień na prawdziwe wywołanie Twojego API asystenta
        var reply = await GetAssistantReplyAsync(text);

        IsTyping = false;
        Messages.Add(new ChatMessage { Text = reply, IsUser = false });
        RebuildGroups();
        StateHasChanged();
        await ScrollToBottomAsync();
    }

    private async Task<string> GetAssistantReplyAsync(string userText)
    {
        await Task.Delay(900); // symulacja opóźnienia odpowiedzi - usuń po podłączeniu prawdziwego API
        return $"Otrzymałem: \"{userText}\"";
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !e.ShiftKey)
        {
            await SendMessage();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/chatInterop.js");
        }
    }

    private async Task ScrollToBottomAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("scrollToBottom", _scrollAnchor);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    private class MessageGroup
    {
        public bool IsUser { get; set; }
        public List<ChatMessage> Messages { get; set; } = new();
    }
}

public class ChatMessage
{
    public string Text { get; set; } = string.Empty;
    public bool IsUser { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
