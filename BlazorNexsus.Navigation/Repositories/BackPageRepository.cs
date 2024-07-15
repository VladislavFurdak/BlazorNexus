using BlazorNexsus.Navigation.BrowserSpecific;

namespace BlazorNexsus.Navigation.Repositories;

public class BackPageRepository<T> : IBackPageRepository<T> where T : struct, Enum 
{
    private string backPageKey = "backPageKey";

    private readonly ISessionStorageRepository _sessionStorageRepository;
    
    public BackPageRepository(ISessionStorageRepository sessionStorageRepository)
    {
        _sessionStorageRepository = sessionStorageRepository;
    }
    
    public async Task SetBackPage(T pageKey)
    {
        await _sessionStorageRepository.SetItem(backPageKey, pageKey.ToString());
    }

    public async Task<T?> PopBackPage()
    {
        var obj = await _sessionStorageRepository.GetItem(backPageKey);
        await _sessionStorageRepository.RemoveItem(backPageKey);
            
        if(obj is null)
        {
            return null;
        }
    
        return Enum.Parse<T>(obj);
    }

    public async Task<bool> IsBackPageSet()
    {
        var value = await _sessionStorageRepository.GetItem(backPageKey);
        return !string.IsNullOrEmpty(value);
    }
}