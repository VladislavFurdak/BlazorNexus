using BlazorNexsus.Navigation.Repositories;
using BlazorNexsus.Navigation.URIMapping;
using BlazorNexsus.Navigation.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorNexsus.Navigation;

public class NavigationManagerExt<T> : INavigationManager<T> where T : struct, Enum
{
    private IReadOnlyDictionary<T, RouteInfoDTO> _routes;
    private NavigationManager _navigationManager;
    private IBackPageRepository<T> _backPageRepository;
    private IJSRuntime _jsRuntime;
    
    public NavigationManagerExt(
        NavigationManager navigationManager,
        IJSRuntime jsRuntime,
        IBackPageRepository<T> backPageRepository)
    {
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
        _routes = RouteMapper.MapRoutes<T>(new[] {typeof(T).Assembly});
        _backPageRepository = backPageRepository;
    }
    
    public void Go(T pageKey)
    {
        var route = _routes[pageKey].Route;
        var processedRoute = UriUtils.RemoveOptionalParams(route);
        _navigationManager.NavigateTo(processedRoute);
    }
    
    public async Task Go(T pageKey, NexusNavigationOptions<T> options)
    {
        await Go(pageKey, options.NewTab, options.BackPage, options.NavigationParams, options.QueryParams);
    }

    public async Task Go(
        T pageKey,
        bool newTab,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null)
    {
        await _backPageRepository.PopBackPage();

       var uri = _routes[pageKey].Route;
       uri = UriUtils.ComposeUri(uri, _navigationManager, navigationParams, queryParams);
       uri = UriUtils.RemoveOptionalParams(uri);
        
        if (backPage != null)
        {
            await _backPageRepository.SetBackPage(backPage.Value);
        }

        if (newTab)
        {
            await _jsRuntime.InvokeVoidAsync("open", uri, "_blank");
        }
        else
        {
            _navigationManager.NavigateTo(uri);
        }
    }


    public async Task Back(T fallBackPageKey, bool preserveQueryString = true)
    {
        T? backPage = await _backPageRepository.PopBackPage();
        
        var qs = preserveQueryString ? UriUtils.ExtractQueryString(_navigationManager.Uri) : string.Empty;
       
        var route = backPage.HasValue ? _routes[backPage.Value].Route : _routes[fallBackPageKey].Route;
        route += qs;
       
        _navigationManager.NavigateTo(route);
    }

    public Task Refresh(bool browserReload)
    {
        _navigationManager.NavigateTo(_navigationManager.Uri, true);
        return Task.CompletedTask;
    }

    public async Task<bool> HasBackPage() =>  await _backPageRepository.IsBackPageSet();

    public T CurrentPage => PathMapper.GetPageKeyFromRoute(_routes, _navigationManager.Uri);

    public IReadOnlyDictionary<T, string> Routes { get; }

    public event EventHandler<LocationChangedEventArgs> LocationChanged
    {
        add => _navigationManager.LocationChanged += value;
        remove => _navigationManager.LocationChanged -= value;
    }
}