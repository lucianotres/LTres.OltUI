using System.Net.Http.Json;
using LTres.Olt.UI.Shared.Models;

namespace LTres.Olt.UI.Client.Services;

public class OLTServices(IHttpClientFactory clientFactory)
{
	private readonly HttpClient _client = clientFactory.CreateClient("api");

	public async Task<IEnumerable<OLT_Host>?> GetHosts()
	  => await _client.GetFromJsonAsync<IEnumerable<OLT_Host>>("OLTHost");

	public async Task<OLT_Host?> GetHost(Guid id)
	  => await _client.GetFromJsonAsync<OLT_Host>($"OLTHost/{id}");

	public async Task<Guid?> PutHost(OLT_Host host)
	{
		var result = await _client.PutAsJsonAsync($"OLTHost", host);
		return result.IsSuccessStatusCode ?
			await result.Content.ReadFromJsonAsync<Guid?>() :
			null;
	}

}