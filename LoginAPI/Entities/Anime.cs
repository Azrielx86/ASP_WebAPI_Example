using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Entities;

public class Anime
{
    [Key] public Guid Id { get; set; }
    [Required] [StringLength(30)] public string? Name { get; set; }
    [Required] [StringLength(15)] public string? Origin { get; set; }
    public DateTime ReleaseDate { get; set; }
}