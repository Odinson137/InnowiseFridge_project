using System.Text.Json.Serialization;

namespace InnowiseFridgeClient.ViewModels;

public class FridgeViewModel
{
    [JsonIgnore]
    public string Id { get; set; }
    public string Name { get; set; }
    public string FridgeModelName { get; set; }
    public string? OwnerName { get; set; }
}