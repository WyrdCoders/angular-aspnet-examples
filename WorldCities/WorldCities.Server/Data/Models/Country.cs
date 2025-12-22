using System.ComponentModel.DataAnnotations;

namespace WorldCities.Server.Data.Models;

public class Country
{
    /// <summary>
    /// The unique id and primary key for this country.
    /// </summary>
    [Key]
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Country code (in ISO 3166-1 ALPHA-2 format)
    /// </summary>
    public required string ISO2 { get; set; }

    /// <summary>
    /// Country code (in ISO 3166-1 ALPHA-3 format)
    /// </summary>
    public required string ISO3 { get; set; }
}
