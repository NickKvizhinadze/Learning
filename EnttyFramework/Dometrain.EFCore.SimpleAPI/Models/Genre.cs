using System.Text.Json.Serialization;

namespace Dometrain.EFCore.SimpleAPI.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}