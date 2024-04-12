using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatsComponent : Node
    {
        [Export] private GameStatsResource gamestats;

        public void UpdateStatAddAmount(string statName, float amount)
        {
            if (gamestats == null)
            {
                GD.PrintErr("The component GameStatsComponent requires a GameStatsResource.");
                return;
            }
            float value = (float)gamestats.Get(statName);
            value += amount;
            gamestats.Set(statName, value);
        }
    }
}