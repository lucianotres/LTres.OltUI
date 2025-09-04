namespace LTres.Olt.UI.Shared.Models;

public class OLT_Host_Item_Value
{
    public required bool Trend { get; set; }
    public required DateTime At { get; set; }
    public int? ValueInt { get; set; }
    public uint? ValueUInt { get; set; }
    public string? ValueStr { get; set; }
}

public class OLT_Host_Item_Values
{
    public required Guid Id { get; set; }

    public required bool ProbedSuccess { get; set; }
    public string? ProbeFailedMessage { get; set; }
    public DateTime? LastProbed { get; set; }
    public DateTime? NextProbe { get; set; }
    public int? ProbedValueInt { get; set; }
    public uint? ProbedValueUInt { get; set; }
    public string? ProbedValueStr { get; set; }
    public bool? AsHex { get; set; }

    public required IList<OLT_Host_Item_Value> Values { get; set; }
}