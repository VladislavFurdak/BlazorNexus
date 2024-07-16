using BlazorNexsus.Navigation;
using BlazorNexsus.Navigation.Repositories;
using FluentAssertions;
using Microsoft.JSInterop;
using NSubstitute;
using UnitTests.Data;
using UnitTests.Dummy;

namespace UnitTests.Tests;

public class NavigationManagerGoBackTests
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
    public async Task GoPage_ThenBackToSame()
    { 
        //TODO fix issue with https://localhostcom/seg name
        
        _backPageRepository = new BackPageRepository<Routes>(new SessionStorageRepositoryMock());
        _navigationManager = new NavigationManagerMock(initUri);
        _navigationManagerExt = new NavigationManagerExt<Routes>(_navigationManager, _jsRuntime, _backPageRepository);
        
        await _navigationManagerExt.Go(
            Routes.AboutPage, 
            false,
            Routes.BasePage, 
            null, 
            null);
        _navigationManagerExt.CurrentPage.Should().Be(Routes.AboutPage);
        
        var hasBackPage = await _navigationManagerExt.HasBackPage();
        hasBackPage.Should().Be(true);
        
        await _navigationManagerExt.Back(fallBackPageKey:Routes.ProductPage);
        
        hasBackPage = await _navigationManagerExt.HasBackPage();

        hasBackPage.Should().Be(false);
        _navigationManagerExt.CurrentPage.Should().Be(Routes.BasePage);
    }

}