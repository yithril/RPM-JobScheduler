using RPM_JobScheduler.DAL.Models;

namespace RPM_JobScheduler.Extensions;

public static class FuelPriceMapping
{
    public static FuelPriceRecord ToFuelPriceRecord(this Datum datum) => new FuelPriceRecord() { Price = (decimal)datum.value, Period = DateTime.Parse(datum.period) };
}
