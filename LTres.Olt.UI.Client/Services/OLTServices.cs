using System.Net.Http.Json;
using LTres.Olt.UI.Shared.Models;

namespace LTres.Olt.UI.Client.Services;

public class OLTServices(IHttpClientFactory clientFactory)
{
    private readonly HttpClient _client = clientFactory.CreateClient("api");

    public async Task<IEnumerable<OLT_Host>?> GetHosts()
        => await _client.GetFromJsonAsync<IEnumerable<OLT_Host>>("OLTHost");

    public async Task<OLT_Host?> GetHost(Guid id)
    {
        var result = await _client.GetAsync($"OLTHost/{id}");
        return result.StatusCode == System.Net.HttpStatusCode.OK ?
            await result.Content.ReadFromJsonAsync<OLT_Host>() :
        null;
    }

    public async Task<Guid?> PostHost(OLT_Host host)
    {
        var result = await _client.PostAsJsonAsync($"OLTHost", host);
        return result.IsSuccessStatusCode ?
            await result.Content.ReadFromJsonAsync<Guid?>() :
            null;
    }

    public async Task<Guid?> PutHost(OLT_Host host)
    {
        var result = await _client.PutAsJsonAsync($"OLTHost", host);
        return result.IsSuccessStatusCode ?
            await result.Content.ReadFromJsonAsync<Guid?>() :
            null;
    }

    public async Task<Guid?> DeleteHost(Guid hostId)
    {
        var result = await _client.DeleteAsync($"OLTHost/{hostId}");
        return result.IsSuccessStatusCode ?
            await result.Content.ReadFromJsonAsync<Guid?>() :
            null;
    }

    public async Task<IList<OLT_Host_Item>?> GetHostItems(Guid hostId)
    {
        var result = await _client.GetAsync($@"OLTHostItem/ByOLT/{hostId}");
        return result.StatusCode == System.Net.HttpStatusCode.OK ?
            await result.Content.ReadFromJsonAsync<IList<OLT_Host_Item>>() :
            null;
    }
}