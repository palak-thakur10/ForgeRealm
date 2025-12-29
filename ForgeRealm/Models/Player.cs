namespace ForgeRealm.Models;

public class Player
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;

    // Navigation properties
    public Resource? Resource { get; set; }
    public ICollection<Building> Buildings { get; set; } = new List<Building>();
}
