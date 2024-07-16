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

1. Create an Enum that keeps your pages (same name as *.razor files):

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

2. add INavigationManager in your razor views

```c#
@using BlazorNexsus.Navigation
@inject INavigationManager<Routes> _navigationManager

 /** Simple use: navigate the page **/
 <button @onclick="() => _navigationManager.Go(Routes.CounterPage)">Go Counter</button>

 /** Complex use **/
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
                newTab: true,
                backPage: Routes.Cart, 
                navigationParams,
                queryStringParams)">Check Special Offer</button>
```

And on the SpecialOffer page:
```c#

@using BlazorNexsus.Navigation
@inject INavigationManager<Routes> _navigationManager


@if (hasBackPage)
{
    <button class="btn btn-primary" @onclick="() => _navigationManager.Back(fallBackPageKey: Routes.Home)">Go Back</button>
}


@code {
      private bool hasBackPage = false;

      protected override async Task OnInitializedAsync()
      {
      hasBackPage = await _navigationManager.HasBackPage();
      }
}

```

Top donators:

⭐Mykhailo Rospopchuk
⭐Іnna Terletskaya
