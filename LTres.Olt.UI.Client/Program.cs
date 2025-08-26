using LTres.Olt.UI.Client;
using LTres.Olt.UI.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.TryAddTransient(typeof(AppPageLocalizer<>), typeof(AppPageLocalizer<>));

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

/*builder.Services.AddHttpClient(AuthDefaults.AuthorizedClientName, client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<AuthorizedHandler>();
*/

await builder.Build().RunAsync();
