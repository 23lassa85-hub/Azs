using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Связующая таблица «многие ко многим»: какую фирму какое топливо поставляет
public class FirmaToplivo
{
    [Required]
    [StringLength(100)]
    public string NazvanieFirmy { get; set; } = "";

    public int KodTopliva { get; set; }

    [JsonIgnore]
    public Firma? Firma { get; set; }

    [JsonIgnore]
    public Toplivo? Toplivo { get; set; }
}
