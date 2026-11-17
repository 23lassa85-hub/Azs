using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Клиент, покупающий топливо
public class Klient
{
    [Required]
    [StringLength(30)]
    public string KartSchet { get; set; } = "";

    [Required]
    [StringLength(150)]
    public string Fio { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string Adres { get; set; } = "";

    [Required]
    [StringLength(30)]
    public string Telefon { get; set; } = "";

    [JsonIgnore]
    public ICollection<Prodazha> Prodazhi { get; set; } = new List<Prodazha>();
}
