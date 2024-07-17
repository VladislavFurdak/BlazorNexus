[<img src="https://img.shields.io/nuget/v/BlazorNexsus.Navigation">](https://www.nuget.org/packages/BlazorNexsus.Navigation/)

# BlazorNexus.Navigation
![image](https://github.com/user-attachments/assets/34d83f00-ab1f-4a55-89d2-9f5e7cf68a98)

```
<button @onclick="() => _nav.Go(Routes.Aiur)">Teleport on Aiur</button>
```

Why?
1. Routing using Enums is more convenient and does not rely on URI schema changing. That approach will have less error in enterprise-scale applications.
2. Conventions are good. Let's separate "Pages" from "Components". That makes code more readable.
3. Do not reinvent the wheel when implementing "go on the previous page", checking which is the "Current" page, parsing query string, etc.


## Installing

To install the package add the following line to your csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="BlazorNexsus.Navigation" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package BlazorNexsus.Navigation
```

If you're using Jetbrains Rider or Visual Studio you can also install via the built-in NuGet package manager.

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
![image](https://github.com/user-attachments/assets/bb629ce6-e6f4-4510-86b1-0a68f9b9aff9)

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

 /** Complex usage: opens a special offer page with a bunch of parameters,
gives the ability to go back with preserving of query string params **/
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
    <button class="btn btn-primary"
    @onclick="() => _navigationManager.Back(fallBackPageKey: Routes.Home, preserveQueryString : true)">Go Back</button>
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
    <a @onclick="() => _navigitaionManager.Go(Routes.HomePage)" class="@GetActiveClassFor(Routes.HomePage) nav-link">
        <span class="bi bi-house-door-fill" aria-hidden="true"></span> Home
    </a>
</div>
<div class="nav-item px-3">
    <a @onclick="() => _navigitaionManager.Go(Routes.Cart)" class="@GetActiveClassFor(Routes.Cart) nav-link">
        <span class="bi bi-cart-check-fill" aria-hidden="true"></span> Cart
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
INavigationManager<T>:

* Go(pageKey);
* Go(options);
* Go(pageKey, newTab, backPage, navigationParams, queryParams);
* HasBackPage();
* Back(fallBackPageKey, preserveQueryString);
* Refresh(browserReload);
* CurrentPage
* Routes
* event LocationChanged
* GetQueryStringParam<T>(key)

There are 2 optional parameters on registration:

```c#
builder.Services.AddBlazorNexusNavigation<Routes>(
    pagePostfixCheck: true,
    checkUnusedKeys: true);
```

* pagePostfixCheck - on "true" rises an exception if a razor Page with the @page attribute doesn't have a "Page" postfix in the filename.
* checkUnusedKeys - on "true" raise an exception if some of the Enums don't have appropriate pages.
Default values are "true"

Enjoy 🍉🍉🍉

Thanks for dontas:

* ⭐Mykhailo Rospopchuk
* ⭐Іnna Terletskaya
