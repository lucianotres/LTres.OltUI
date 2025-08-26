namespace LTres.Olt.UI.Shared.Models;

public class OLT_Host
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
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
}
