using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prod.Domain.Entities;

// WARNING: This is not how it's should be done in production
// TODO: Create a DTO and use it in the Application layer
public class Country
{
    [Key, Column("id"), JsonIgnore]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("alpha2")]
    public string Alpha2 { get; set; }
    [Column("alpha3")]
    public string Alpha3 { get; set; }
    [Column("region")]
    public string Region { get; set; }
}