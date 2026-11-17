using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Продажа топлива клиенту на АЗС
public class Prodazha
{
    public int KodProdazhi { get; set; }

    public DateOnly DataProdazhi { get; set; }

    [Required]
    [StringLength(30)]
    public string KartSchet { get; set; } = "";

    public int KodAzs { get; set; }

    public int KodTopliva { get; set; }

    // Количество (в литрах)
    [Range(0.01, 99999999)]
    public decimal Kolichestvo { get; set; }

    [JsonIgnore]
    public Klient? Klient { get; set; }

    [JsonIgnore]
    public Azs? Azs { get; set; }

    [JsonIgnore]
    public Toplivo? Toplivo { get; set; }
}
