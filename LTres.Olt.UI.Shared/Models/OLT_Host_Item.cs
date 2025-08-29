namespace LTres.Olt.UI.Shared.Models;

public class OLT_Host_Item
{
    public Guid Id { get; set; }
    public Guid? IdOltHost { get; set; }
    public Guid? IdRelated { get; set; }
    public string? Action { get; set; }
    public string? ItemKey { get; set; }
    public DateTime? LastProbed { get; set; }
    public DateTime? NextProbe { get; set; }
    public int? Interval { get; set; }
    public int? MaintainFor { get; set; }
    public int? HistoryFor { get; set; }
    public bool Active { get; set; } = true;
    public bool ProbedSuccess { get; set; }
    public string? ProbeFailedMessage { get; set; }
    public int? ProbedValueInt { get; set; }
    public uint? ProbedValueUInt { get; set; }
    public string? ProbedValueStr { get; set; }
    public bool? Template { get; set; }
    public Guid? From { get; set; }
    public string? Calc { get; set; }
    public bool? AsHex { get; set; }
    public string? Description { get; set; }
}


public static class OLT_Host_ItemExtensions
{
    public static readonly string[] ValidActions = new []{ "ping", "snmpget", "snmpwalk" };

    public const int MinInterval = 1;
    public const int MaxInterval = 86400; //1 day 

    public const int MinHistoryFor = 1;
    public const int MaxHistoryFor = 2628000; //5 years

    public const int MinMaintainFor = 0; //0 is for not maintain at failed read 
    public const int MaxMaintainFor = 2628000; //5 years
}
