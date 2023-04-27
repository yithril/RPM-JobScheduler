using System.ComponentModel.DataAnnotations.Schema;

namespace RPM_JobScheduler.DAL.Models;

public class FuelPriceRecord
{
    public int Id { get; set; }
    public DateTime Period { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal Price { get; set; }
}
