using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzsApi.Models;

// Автозаправочная станция. Принадлежит ровно одной фирме.
public class Azs
{
    public int KodAzs { get; set; }

    [Required]
    [StringLength(100)]
    public string NazvanieFirmy { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string AdresAzs { get; set; } = "";

    [JsonIgnore]
    public Firma? Firma { get; set; }

    [JsonIgnore]
    public ICollection<Prodazha> Prodazhi { get; set; } = new List<Prodazha>();
}
