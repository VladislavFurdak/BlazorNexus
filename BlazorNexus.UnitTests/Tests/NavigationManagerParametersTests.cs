using BlazorNexsus.Navigation;
using BlazorNexsus.Navigation.Repositories;
using FluentAssertions;
using Microsoft.JSInterop;
using NSubstitute;
using UnitTests.Data;
using UnitTests.Dummy;

namespace UnitTests.Tests;

public class NavigationManagerParametersTests
{
    private string initUri = "http://nexus/";
    private INavigationManager<Routes> _navigationManagerExt = null!;
    private IJSRuntime _jsRuntime = null!;
    private NavigationManagerMock _navigationManager = null!;
    private IBackPageRepository<Routes> _backPageRepository = null!;
    
    [SetUp]
    public void Setup()
    {
        _jsRuntime = Substitute.For<IJSRuntime>();
        _backPageRepository = Substitute.For<IBackPageRepository<Routes>>();
        _navigationManager = new NavigationManagerMock(initUri);
        _navigationManagerExt = new NavigationManagerExt<Routes>(_navigationManager, _jsRuntime, _backPageRepository);
    }

    [Test] public void NoRoute_NoParams()
    {
        _navigationManager.ResultUri += s => s.Should().Be("/");
        _navigationManagerExt.Go(Routes.BasePage);
    }
    
    [Test] public void SingleRouteSegment_NoParams()
    {
        _navigationManager.ResultUri += s => s.Should().Be("/about");
        _navigationManagerExt.Go(Routes.AboutPage);
    }

    [Test] public void MultipleRouteSegment_NoParams()
    {
        _navigationManager.ResultUri += s => s.Should().Be("/about/me");
        _navigationManagerExt.Go(Routes.AboutMePage);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_SingleMandatoryParam()
    {
        var paramKey = "productId";
        var paramValue = "777";
        var navigationParams = new Dictionary<string, string>(){{paramKey, paramValue}};
        
        _navigationManager.ResultUri += s => s.Should().Be("/product/777");
        
        await _navigationManagerExt.Go(Routes.ProductPage, false, null, navigationParams, null);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_SingleOptionalParam()
    {
        var paramKey = "productId";
        var paramValue = "777";
        var navigationParams = new Dictionary<string, string>(){{paramKey, paramValue}};
        
        _navigationManager.ResultUri = s => s.Should().Be("/productext/777");
        await _navigationManagerExt.Go(Routes.ProductExtPage, false, null, navigationParams, null);
        
        _navigationManager.ResultUri = s => s.Should().Be("/productext");
        await _navigationManagerExt.Go(Routes.ProductExtPage, false, null, null, null);
    }

    [Test] 
    public async Task MultipleRouteSegment_SingleOptionalParam_WithQuerySting()
    {
        var paramKey = "productId";
        var paramValue = "777";
        
        var navigationParams = new Dictionary<string, string>(){{paramKey, paramValue}};
        var qsParams = new Dictionary<string, string>(){{"utm", "123123123"}};
        
        _navigationManager.ResultUri = s => s.Should().Be("/productext/777?utm=123123123");
        await _navigationManagerExt.Go(Routes.ProductExtPage, false, null, navigationParams, qsParams);
        
        _navigationManager.ResultUri = s => s.Should().Be("/productext?utm=123123123");
        await _navigationManagerExt.Go(Routes.ProductExtPage, false, null, null, qsParams);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_OptionalParamSet_RequiredParamSet()
    {
        var paramKey1 = "areaId";
        var paramValue1 = "1";
        
        var paramKey2 = "chunkId";
        var paramValue2 = "6A7263BC-7177-4533-A3C8-AC99C24229C1";
        
        var navigationParams = new Dictionary<string, string>()
        {
            {paramKey1, paramValue1},
            {paramKey2, paramValue2},
        };
        
        _navigationManager.ResultUri = s => s.Should().Be("/dashboard/1/personal/6A7263BC-7177-4533-A3C8-AC99C24229C1");
        await _navigationManagerExt.Go(Routes.DashboardPage, false, null, navigationParams, null);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_OptionalParamSet_RequiredParamIsNotSet()
    {
        var paramKey1 = "areaId";
        var paramValue1 = "1";
        
        var navigationParams = new Dictionary<string, string>()
        {
            {paramKey1, paramValue1}
        };
        
        _navigationManager.ResultUri = s => s.Should().Be("/dashboard/1/personal");
        await _navigationManagerExt.Go(Routes.DashboardPage, false, null, navigationParams, null);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_MultipleOptionalParams_AreNotSet()
    {
        _navigationManager.ResultUri = s => s.Should().Be("/levels");
        await _navigationManagerExt.Go(Routes.ManyOptionalParamsPage, false, null, null, null);
    }
    
    [Test] 
    public async Task MultipleRouteSegment_MultipleOptionalParams_AllParamsSet()
    {
       var navigationParams = new Dictionary<string, string>()
            {
                {"active", "true"},
                {"dob", "2016-12-31"},
                {"price", "-1,000.01"},
                {"weight", "-1,001.01e8"}, 
                {"weight2", "-1,001.01e8"}, 
                {"id", "CD2C1638-1638-72D5-1638-DEADBEEF1638"}, 
                {"ids", "-123456789"}, 
                {"ticks", "123456789"}, 
            };

        var resultString = "/levels/true/2016-12-31/-1,000.01/-1,001.01e8/-1,001.01e8/CD2C1638-1638-72D5-1638-DEADBEEF1638/-123456789/123456789";
        _navigationManager.ResultUri = s => s.Should().Be(resultString);
            
        await _navigationManagerExt.Go(Routes.ManyOptionalParamsPage, false, null, navigationParams, null);
    }
    
}