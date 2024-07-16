using System.Runtime.InteropServices;
using BlazorNexsus.Navigation;
using BlazorNexsus.Navigation.Repositories;
using FluentAssertions;
using Microsoft.JSInterop;
using NSubstitute;
using UnitTests.Data;
using UnitTests.Dummy;

namespace UnitTests.Tests;

public class NavigationManagerGetQueryStringParam
{
    private string initUri = "http://nexus.com/";
    private INavigationManager<Routes> _navigationManagerExt = null!;
    private IJSRuntime _jsRuntime = null!;
    private NavigationManagerMock _navigationManager = null!;
    private IBackPageRepository<Routes> _backPageRepository = null!;
    
    [SetUp]
    public void Setup()
    {
        _jsRuntime = Substitute.For<IJSRuntime>();
    }
    
    [Test] 
    public async Task GetQueryStringParam_CheckCasting()
    { 
        _backPageRepository = new BackPageRepository<Routes>(new SessionStorageRepositoryMock());
        _navigationManager = new NavigationManagerMock(initUri);
        _navigationManagerExt = new NavigationManagerExt<Routes>(_navigationManager, _jsRuntime, _backPageRepository);

        var queryStringParams = new Dictionary<string, string>()
        {
            {"utm", "1111"},
            {"utm2", "BFG"},
            {"utm3", "30-05-1991"},
            {"utm4", "6A7263BC-7177-4533-A3C8-AC99C24229C1"}
        };
        
        await _navigationManagerExt.Go(
            Routes.AboutPage, 
            false,
            Routes.AboutMePage, 
            null, 
            queryStringParams);

        var qs = _navigationManagerExt.GetQueryStringParam<int>("utm");
        var qs2 = _navigationManagerExt.GetQueryStringParam<string>("utm2");
        var qs3 = _navigationManagerExt.GetQueryStringParam<DateTime>("utm3");
        var qs4 = _navigationManagerExt.GetQueryStringParam<Guid>("utm4");
    }

}