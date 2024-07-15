using System.Reflection;
using BlazorNexsus.Navigation.Abstractions;
using BlazorNexsus.Navigation.DTOs;
using BlazorNexsus.Navigation.Internal;
using BlazorNexsus.Navigation.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorNexsus.Navigation;

public class NavigationManagerExt<T> : INavigationManager<T> where T : struct, Enum
{
    private IReadOnlyDictionary<T, RouteInfoDTO> _routes;
    private NavigationManager _navigationManager;
    private IBackpageRepository<T> _backpageRepository;
    private ICurrentPageRepository<T> _currentPageRepository;
    public NavigationManagerExt(IEnumerable<Assembly> lookupAssemblies, NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _routes = RouteMapper.MapRoutes<T>(lookupAssemblies);
        _backpageRepository = DI.Get<IBackpageRepository<T>>()!;
        _currentPageRepository = DI.Get<ICurrentPageRepository<T>>()!;
    }
    
    public void Go(T pageKey)
    {
        _navigationManager.NavigateTo(_routes[pageKey].Route);
    }
    
    public void Go(
        T pageKey, 
        T? backPage,
        Dictionary<string, string>? navigationParams = null,
        Dictionary<string, string>? queryParams = null)
    {
        if (backPage != null)
        {
            _backpageRepository.SetBackPage(backPage.Value);
        }
        _navigationManager.NavigateTo(_routes[pageKey].Route);
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