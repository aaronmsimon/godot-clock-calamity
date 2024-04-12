using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatResource : Resource
    {
        [ExportCategory("Game Stat Resource")]
        [Export] public string StatName { get; set; }
        [Export] public float StatValue { get; set; }
    }
}
