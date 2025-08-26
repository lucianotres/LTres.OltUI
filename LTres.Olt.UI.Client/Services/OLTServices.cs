using System.Net.Http.Json;
using LTres.Olt.UI.Shared.Models;

namespace LTres.Olt.UI.Client.Services;

public class OLTServices(IHttpClientFactory clientFactory)
{
    private readonly HttpClient _client = clientFactory.CreateClient("api");

    public async Task<IEnumerable<OLT_Host>?> GetHosts()
      => await _client.GetFromJsonAsync<IEnumerable<OLT_Host>>("OLTHost");
}