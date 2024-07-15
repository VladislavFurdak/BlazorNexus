using System.Reflection;
using BlazorNexsus.Navigation.Abstractions;
using BlazorNexsus.Navigation.DTOs;
using BlazorNexsus.Navigation.Internal;
using BlazorNexsus.Navigation.Models;
using BlazorNexsus.Navigation.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorNexsus.Navigation;

public class NavigationManagerExt<T> : INavigationManager<T> where T : struct, Enum
{
    private IReadOnlyDictionary<T, RouteInfoDTO> _routes;
    private NavigationManager _navigationManager;
    private IBackpageRepository<T> _backpageRepository;
    private ICurrentPageRepository<T> _currentPageRepository;
    private IJSRuntime _jsRuntime;
    
    public NavigationManagerExt(IEnumerable<Assembly> lookupAssemblies, NavigationManager navigationManager, IJSRuntime jsRuntime)
    {
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
        _routes = RouteMapper.MapRoutes<T>(lookupAssemblies);
        _backpageRepository = DI.Get<IBackpageRepository<T>>()!;
        _currentPageRepository = DI.Get<ICurrentPageRepository<T>>()!;
    }
    
    public void Go(T pageKey)
    {
        _navigationManager.NavigateTo(_routes[pageKey].Route);
    }
    
    public async Task Go(T pageKey, NexusNavigationOptions<T>? options = null)
    {
        var uri = _routes[pageKey].Route;
        if (options is null)
        {
            _navigationManager.NavigateTo(uri);
            return;
        }

        await Go(pageKey, options.NewTab, options.BackPage, options.NavigationParams, options.QueryParams);
    }

    public async Task Go(
        T pageKey,
        bool newTab,
        T? backPage = null,
        IReadOnlyDictionary<string, string>? navigationParams = null,
        IReadOnlyDictionary<string, string>? queryParams = null)
    {
       var uri = UriUtils.GetUri(_routes, _navigationManager, pageKey, newTab, backPage, navigationParams, queryParams);
        
        if (backPage != null)
        {
            _backpageRepository.SetBackPage(backPage.Value);
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


    public void Back(T fallBackPageKey, bool preserveQueryString = true)
    {
        //TODO implement prev qs
        
        T? backPage = _backpageRepository.PopBackPage();

        _navigationManager.NavigateTo(
            backPage.HasValue ? _routes[backPage.Value].Route : _routes[fallBackPageKey].Route);
    }

    public Task Refresh(bool browserReload)
    {
        throw new NotImplementedException();
    }

    public bool HasBackPage => _backpageRepository.IsBackPageSet;

    public T CurrentPage => PathMapper.GetPageKeyFromRoute(_routes, _navigationManager.Uri);

    public IReadOnlyDictionary<T, string> Routes { get; }
    
    public Task<NavigationInfo<T>> PreviousPage { get; }

    public event EventHandler<LocationChangedEventArgs> LocationChanged
    {
        add => _navigationManager.LocationChanged += value;
        remove => _navigationManager.LocationChanged -= value;
    }
}