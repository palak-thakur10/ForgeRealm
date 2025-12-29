using System.ComponentModel.DataAnnotations;

namespace ForgeRealm.Models;

public class Building
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Building Name is required.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Building Name can only contain letters, numbers, and spaces.")]
    public string Name { get; set; } = "Town Hall";

    [Range(1, 100, ErrorMessage = "Level must be between 1 and 100.")]
    public int Level { get; set; } = 1;

    public DateTime? UpgradeCompleteAt { get; set; }

    // Optional: Rarity based on Level
    public string Rarity => Level switch
    {
        >= 10 => "Legendary",
        >= 5 => "Rare",
        _ => "Common"
    };

    // Foreign Key
    public int PlayerId { get; set; }

    // Navigation
    public Player? Player { get; set; }
}
