using System.Text.Json.Serialization;

namespace InnowiseFridgeClient.ViewModels;

public class FridgeViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string FridgeModelName { get; set; }
    public string OwnerName { get; set; }
    public string Description { get; set; }
}