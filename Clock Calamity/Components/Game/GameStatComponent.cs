using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatComponent : Node
    {
        [Export] public GameStatResource gamestat { get; set; }

        public override void _Ready()
        {
            if (gamestat == null)
            {
                GD.PrintErr("The GameStatComponent " + this.Name + " requires a GameStatResource. Please add one to node " + this.GetPath());
                return;
            }
        }

        public void UpdateStatAddAmount(float amount)
        {
            if (gamestat == null) return;

            gamestat.StatValue += amount;
        }

        public void UpdateStatSetAmount(float amount)
        {
            if (gamestat == null) return;
            
            gamestat.StatValue = amount;
        }
    }
}