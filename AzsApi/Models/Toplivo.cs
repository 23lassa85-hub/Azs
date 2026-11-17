using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Вид топлива
public class Toplivo
{
    public int KodTopliva { get; set; }

    [Required]
    [StringLength(50)]
    public string VidTopliva { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string EdinicaIzmereniya { get; set; } = "";

    // Цена, руб.
    [Range(0.01, 99999999)]
    public decimal CenaRub { get; set; }

    [JsonIgnore]
    public ICollection<FirmaToplivo> FirmyToplivo { get; set; } = new List<FirmaToplivo>();

    [JsonIgnore]
    public ICollection<Prodazha> Prodazhi { get; set; } = new List<Prodazha>();
}
