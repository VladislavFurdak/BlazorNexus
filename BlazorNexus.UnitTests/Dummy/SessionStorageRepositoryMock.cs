using BlazorNexsus.Navigation.BrowserSpecific;

namespace UnitTests.Dummy;

public class SessionStorageRepositoryMock : ISessionStorageRepository
{
    private readonly Dictionary<string, string> _state = new ();
    
    public ValueTask<string> GetItem(string key, CancellationToken cancellationToken = default)
    {
        string result;
        if (!_state.TryGetValue(key, out result))
        {
            return ValueTask.FromResult((string)null!);
        }
        return ValueTask.FromResult(result);
    }

    public ValueTask SetItem(string key, string data)
    {
        _state[key] = data;
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveItem(string key)
    {
        _state.Remove(key);
        return ValueTask.CompletedTask;
    }
}