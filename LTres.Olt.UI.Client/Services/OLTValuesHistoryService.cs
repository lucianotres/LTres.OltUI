
using LTres.Olt.UI.Shared.Models;

namespace LTres.Olt.UI.Client.Services;

public class OLTValuesHistoryService
{
    private Random random = new();

    public async Task<OLT_Host_Item_Values> GetValues(Guid Id, int quantity, DateTime? startDate, DateTime? endDate)
    {
        await Task.Delay(50); //http get simulation

        if (quantity < 0)
            quantity = 1;
        if (quantity > 9999999)
            quantity = 9999999;

        if (!startDate.HasValue)
            startDate = DateTime.Now;
        
        var values = Enumerable.Range(0, quantity-1)
            .Select(x => new OLT_Host_Item_Value()
            {
                Trend = false,
                At = startDate.Value.AddSeconds(x * 10),
                ValueInt = random.Next(100, 350)
            })
            .ToList();

        return new OLT_Host_Item_Values()
        {
            Id = Id,
            ProbedSuccess = true,
            Values = values,
            LastProbed = values.Last().At,
            ProbedValueInt = values.Last().ValueInt,
        };
    }
}
