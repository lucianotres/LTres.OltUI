using System.ComponentModel.DataAnnotations;
using LTres.Olt.UI.Shared.Validation;

namespace LTres.Olt.UI.Shared.Models;

public class OLT_Host_Item
{
    public Guid Id { get; set; }
    public Guid? IdOltHost { get; set; }
    public Guid? IdRelated { get; set; }
    [Required]
    public string? Action { get; set; }
    [OltItemKeyValidation]
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
    [Required]
    public string? Description { get; set; }
}


public static class OLT_Host_ItemExtensions
{
    public const string ActionPing = "ping";
    public const string ActionSnmpGet = "snmpget";
    public const string ActionSnmpWalk = "snmpwalk";

    public static readonly string[] ValidActions = [ActionPing, ActionSnmpGet, ActionSnmpWalk];

    public const int MinInterval = 1;
    public const int MaxInterval = 86400; //1 day 

    public const int MinHistoryFor = 1;
    public const int MaxHistoryFor = 2628000; //5 years

    public const int MinMaintainFor = 0; //0 is for not maintain at failed read 
    public const int MaxMaintainFor = 2628000; //5 years

    private static string TimeSpanToStringCustom(TimeSpan timeSpan)
    {
        List<string> parts = new();

        if (timeSpan.Days > 0)
            parts.Add($"{timeSpan.Days}d");

        if (timeSpan.Hours > 0)
            parts.Add($"{timeSpan.Hours}h");

        if (timeSpan.Minutes > 0)
            parts.Add($"{timeSpan.Minutes}m");

        if (timeSpan.Seconds > 0)
            parts.Add($"{timeSpan.Seconds}s");

        return string.Join(" ", parts);
    }

    public static string ToStrFromSeconds(this int seconds) => TimeSpanToStringCustom(TimeSpan.FromSeconds(seconds));

    public static string ToStrFromMinutes(this int minutes) => TimeSpanToStringCustom(TimeSpan.FromMinutes(minutes));
}
