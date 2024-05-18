using System.Reflection;
using EasyBlazor.Navigation.Abstractions;
using EasyBlazor.Navigation.DTOs;
using EasyBlazor.Navigation.Internal;
using EasyBlazor.Navigation.Repositories;
using Microsoft.AspNetCore.Components;

namespace EasyBlazor.Navigation;

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
    
    public void Go(
        T pageKey, 
        Dictionary<string, string>? navigationParams = null,
        Dictionary<string, string>? queryParams = null)
    {
        _navigationManager.NavigateTo(_routes[pageKey].Route);
    }
    
    public void GoWithBack(
        T pageKey, 
        T backPage,
        Dictionary<string, string>? navigationParams = null,
        Dictionary<string, string>? queryParams = null)
    {
        _backpageRepository.SetBackPage(backPage);
        _navigationManager.NavigateTo(_routes[pageKey].Route);
    }
    
    public void Go(
        T pageKey, 
        T backPage
       )
    {
        _navigationManager.NavigateTo(_routes[pageKey].Route);
    }

    public void Back(T fallBackPageKey, bool includePreviousQueryString = true)
    {
        //TODO implement prev qs
        
        T? backPage = _backpageRepository.PopBackPage();

        _navigationManager.NavigateTo(
            backPage.HasValue ? _routes[backPage.Value].Route : _routes[fallBackPageKey].Route);
    }

    public Task Refresh()
    {
        throw new NotImplementedException();
    }

    public bool HasBackPage => _backpageRepository.IsBackPageSet;

    public T CurrentPage => PathMapper.GetPageKeyFromRoute(_routes, _navigationManager.Uri);

    public IReadOnlyDictionary<T, string> Routes { get; }
    public IReadOnlyList<NavigationInfo> History { get; }
    public Task<NavigationInfo> PreviousPage { get; }
}