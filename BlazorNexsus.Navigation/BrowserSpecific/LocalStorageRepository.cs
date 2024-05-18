using Microsoft.JSInterop;

namespace BlazorNexsus.Navigation.BrowserSpecific;

public interface ILocalStorageRepository
{ 
    ValueTask<string> GetItem(string key, CancellationToken cancellationToken = default);
    ValueTask SetItem(string key, string data);
    ValueTask RemoveItem(string key);
}

public class LocalStorageRepository : ILocalStorageRepository
{
    private readonly IJSRuntime _jSRuntime = null!;
    private readonly IJSInProcessRuntime _jSInProcessRuntime = null!;
    
    public LocalStorageRepository(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
        _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
    }
    
    public async ValueTask<string> GetItem(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _jSRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);
        }
        catch (Exception exception)
        {
            if (IsStorageDisabledException(exception))
            {
                throw new Exception("StorageNotAvailableMessage", exception);
            }

            throw;
        }
    }
    
    
    public async ValueTask SetItem(string key, string data)
    {
        CheckForInProcessRuntime();
        try
        {
            await _jSInProcessRuntime.InvokeVoidAsync("localStorage.setItem", key, data);
        }
        catch (Exception exception)
        {
            if (IsStorageDisabledException(exception))
            {
                throw new Exception("StorageNotAvailableMessage", exception);
            }

            throw;
        }
    }

    public async ValueTask RemoveItem(string key)
    {
        CheckForInProcessRuntime();
        try
        {
            _jSInProcessRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (Exception exception)
        {
            if (IsStorageDisabledException(exception))
            {
            }

            throw;
        }
    }
    
    private void CheckForInProcessRuntime()
    {
        if (_jSInProcessRuntime == null)
            throw new InvalidOperationException("IJSInProcessRuntime not available");
    }
    
    private static bool IsStorageDisabledException(Exception exception) 
        => exception.Message.Contains("Failed to read the 'sessionStorage' property from 'Window'");
}