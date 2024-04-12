using Godot;

namespace Components.Game
{
    public partial class GameStatsResource : Resource
    {
        [Export] public int score { get; set; } = 0;
        [Export] public int shotsFired { get; set; } = 0;
        [Export] public int shotsHit { get; set; } = 0;
    }
}
