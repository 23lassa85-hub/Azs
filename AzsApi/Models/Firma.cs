using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Фирма-владелец АЗС
public class Firma
{
    [Required]
    [StringLength(100)]
    public string NazvanieFirmy { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string YurAdres { get; set; } = "";

    [Required]
    [StringLength(30)]
    public string Telefon { get; set; } = "";

    [JsonIgnore]
    public ICollection<Azs> AzsList { get; set; } = new List<Azs>();

    [JsonIgnore]
    public ICollection<FirmaToplivo> FirmyToplivo { get; set; } = new List<FirmaToplivo>();
}
