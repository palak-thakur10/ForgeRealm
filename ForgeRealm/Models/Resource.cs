namespace ForgeRealm.Models;

public class Resource
{
    public int Id { get; set; }

    public int Gold { get; set; } = 100;
    public int Wood { get; set; } = 100;
    public int Stone { get; set; } = 0;
    public int Food { get; set; } = 100;

    // Foreign Key
    public int PlayerId { get; set; }

    // Navigation
    public Player Player { get; set; } = null!;
}
