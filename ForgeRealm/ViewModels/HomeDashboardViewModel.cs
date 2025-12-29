using ForgeRealm.Models;
using System.Collections.Generic;

namespace ForgeRealm.ViewModels
{
    public class HomeDashboardViewModel
    {
        public int TotalPlayers { get; set; }
        public int TotalBuildings { get; set; }
        public int TotalGold { get; set; }
        public int TotalWood { get; set; }
        public int TotalStone { get; set; }
        public int TotalFood { get; set; }
        public List<Player> TopPlayers { get; set; } = new();
    }
}
