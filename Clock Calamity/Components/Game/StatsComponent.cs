using Godot;

namespace Components.Game
{
    public partial class StatsComponent : Node
    {
        [Export] public GameStats gameStats { get; private set; }

        [Signal] public delegate void ScoreChangedEventHandler();
        [Signal] public delegate void ShotsFiredChangedEventHandler();
        [Signal] public delegate void ShotsHitChangedEventHandler();

        public void UpdateScore(int amount)
        {
            gameStats.score += amount;
            EmitSignal(SignalName.ScoreChanged);
        }

        public void UpdateShotsFired(int amount)
        {
            gameStats.shotsFired += amount;
            EmitSignal(SignalName.ShotsFiredChanged);
        }

        public void UpdateShotsHit(int amount)
        {
            gameStats.shotsHit += amount;
            EmitSignal(SignalName.ShotsHitChanged);
        }
    }
}
