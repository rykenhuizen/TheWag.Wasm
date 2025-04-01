using Microsoft.JSInterop;

namespace TheWag.Wasm.Util;

public class SessionStorage(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public async Task<T?> GetValueAsync<T>(string key)
    {
        await WaitForReference();
        var sessionValue = await _accessorJsRef.Value.InvokeAsync<string>("get", key);
        if (sessionValue == null)
            return default;

        var objectValue = System.Text.Json.JsonSerializer.Deserialize<T>(sessionValue);

        return objectValue;
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();
        string json = System.Text.Json.JsonSerializer.Serialize<T>(value);
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, json);
    }

    public async Task Clear()
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("clear");
    }

    public async Task RemoveAsync(string key)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("remove", key);
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/SessionStorageAccessor.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }
}