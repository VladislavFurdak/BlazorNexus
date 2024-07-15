using BlazorNexsus.SampleProject;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorNexsus.Navigation;
using BlazorNexsus.SampleProject.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddEasyBlazorNavigationManager<Routes>(
    forcePagePostfixCheck: true,
    forceCheckUnusedKeys: true);

await builder.Build().RunAsync();


//multiple enums
//nested multiple nesting back  