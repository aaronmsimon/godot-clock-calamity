using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatsResource : Resource
    {
        [ExportCategory("Game Stats Resource")]
        [ExportGroup("The stats are customized for every game")]
        [Export] public int Score { get; set; }
        [Export] public int ShotsFired { get; set; }
        [Export] public int ShotsHit { get; set; }
    }
}
