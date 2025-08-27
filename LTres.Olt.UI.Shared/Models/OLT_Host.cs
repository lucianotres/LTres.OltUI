using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LTres.Olt.UI.Shared.Models;

public class OLT_Host
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@".+:\d+", ErrorMessage = "Should be a valid DNS name or IP address")]
    public string Host { get; set; } = string.Empty;
    public string? SnmpCommunity { get; set; }
    public bool? SnmpBulk { get; set; }
    public int? SnmpVersion { get; set; }
    public int? GetTimeout { get; set; }
    public int Interface { get; set; }
    public bool? Active { get; set; }


    public IEnumerable<string>? tags { get; set; }
    public OLT_Host_OnuRef? OnuRef { get; set; }
    public OLT_Host_CLIconfig? CLI { get; set; }

    [JsonIgnore]
    public string HostName
    {
        get => string.IsNullOrWhiteSpace(Host) ? string.Empty : Host.Split(':')[0];
        set => Host = $"{value}:{HostPort}";
    }

    [JsonIgnore]
    public int HostPort
    {
        get => string.IsNullOrWhiteSpace(Host) ? 0 :
            int.TryParse(Host.Split(':')[1], out int port) ? port : 0;
        set => Host = $"{HostName}:{value}";
    }
}
