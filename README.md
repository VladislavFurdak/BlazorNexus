![NuGet Version](https://img.shields.io/nuget/vpre/BlazorNexsus.Navigation?style=flat-square&logoColor=%23512BD4&logoSize=auto&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FBlazorNexsus.Navigation%2F)

# BlazorNexus.Navigation
The new era of Blazor routing 


## Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="BlazorNexsus.Navigation" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package BlazorNexsus.Navigation
```

If you're using Jetbrains Rider or Visual Studio you can also install via the built in NuGet package manager.

## Setup

You will need to register the navigation manager services with the service collection in your _Program.cs_ file in Blazor WebAssembly.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazorNexusNavigation<Routes>();

    await builder.Build().RunAsync();
}
```

## Usage (Blazor WebAssembly)

Create an Enum that keeps your pages (same name as *.razor files):

```c#
public enum Routes
{
    CounterPage,
    HomePage,
    WeatherPage
}
```
for 
CounterPage.razor
HomePage.razor
WeatherPage.razor

But, be sure, that
1. Each Razor Page file has a navigation @page attribute
2. The Name is the same as for Enum
3. The *.razor files have a mandatory "Page" postfix. 

And add INavigationManager<Routes> in your razor views

## Examples of usage

```c#
@using BlazorNexsus.Navigation
@inject INavigationManager<Routes> _navigationManager

 /** Simple use: navigate the page **/
 <button @onclick="() => _navigationManager.Go(Routes.CounterPage)">Go Counter</button>

 /** Complex use, open a special offer page with a bunch of parameters, give the ability to go back with saving of query string params **/
 @{
        Dictionary<string,string> navigationParams =  new()
        {
            ["sectionId"] = "777",
            ["couponId"] = "49CEAE43-2004-45D1-8716-6EA0EFC407AC"
        };

        Dictionary<string,string> queryStringParams =  new()
        {
            ["utm_source"] = "email",
            ["utm_campaign"] = "weekab"
        };
    }

    <button @onclick="() => _navigationManager.Go(
                pageKey: Routes.SpecialOffer,
                newTab: false,
                backPage: Routes.Cart, 
                navigationParams,
                queryStringParams)">Check Special Offer</button>
```

And on the Special Offer page:
```c#

@using BlazorNexsus.Navigation
@inject INavigationManager<Routes> _navigationManager


@if (hasBackPage)
{
    <button class="btn btn-primary" @onclick="() => _navigationManager.Back(fallBackPageKey: Routes.Home, preserveQueryString : true)">Go Back</button>
}

@{
}

@if (utm_campaign is "weekab")
{
    <div>Special offer !</div>
}

@code {
    private bool hasBackPage = false;
    private string utm_campaign = string.empty;

    protected override async Task OnInitializedAsync()
    {
        hasBackPage = await _navigationManager.HasBackPage();
        utm_campaign = _navigationManager.GetQueryStringParam<string>("utm_campaign");
    }
}

```

Another example of creating a menu with an active element:
```c#
@using BlazorNexsus.Navigation
@inject INavigationManager<Routes> _navigitaionManager

<div class="nav-item px-3">
    <a @onclick="() => _navigitaionManager.Go(Routes.HomePage)"
        class="@GetActiveClassFor(Routes.HomePage) nav-link">
        <span class="bi bi-house-door-fill" aria-hidden="true"></span> Home
    </a>
</div>
<div class="nav-item px-3">
    <a @onclick="() => _navigitaionManager.Go(Routes.CounterPage)"
         class="@GetActiveClassFor(Routes.CounterPage) nav-link">
        <span class="bi bi-plus-square-fill" aria-hidden="true"></span> Counter
    </a>
 </div>


@code{
    private string GetActiveClassFor(Routes activePage)
    {
        return _navigitaionManager.CurrentPage == activePage ? "active" : string.Empty;
    }

}

```

## Full API specification

Thanks for dontas:

⭐Mykhailo Rospopchuk
⭐Іnna Terletskaya
