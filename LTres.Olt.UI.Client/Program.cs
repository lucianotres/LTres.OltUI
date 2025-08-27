using LTres.Olt.UI.Client;
using LTres.Olt.UI.Client.Services;
using LTres.Olt.UI.Client.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor;
using MudBlazor.Services;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization();
builder.Services.AddMudServices(cfg =>
{
    cfg.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    cfg.SnackbarConfiguration.ShowCloseIcon = true;
    cfg.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.TryAddTransient(typeof(AppPageLocalizer<>), typeof(AppPageLocalizer<>));
builder.Services.AddTransient<DialogTools>();
builder.Services.AddTransient<ToastTools>();

//authorization scheme services
builder.Services.AddAuthorizationCore();
builder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
builder.Services.AddTransient<AuthorizedHandler>();
builder.Services.AddHttpClient("default", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("default"));

//api services
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/api/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddTransient<OLTServices>();
builder.Services.AddSingleton<AppDataService>();

await builder.Build().RunAsync();
