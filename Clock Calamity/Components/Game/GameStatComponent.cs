using Godot;

namespace Components.Game
{
    [GlobalClass]
    public partial class GameStatComponent : Node
    {
        [Export] public GameStatResource gamestat { get; set; }

        [Signal] public delegate void StatChangedEventHandler();

        public void UpdateStatAddAmount(float amount)
        {
            if (gamestat == null)
            {
                GD.PrintErr("The GameStatComponent " + this.Name + " requires a GameStatResource. Please add one to node " + this.GetPath());
                return;
            }
            gamestat.StatValue += amount;
            EmitSignal(SignalName.StatChanged);
        }

        public void UpdateStatSetAmount(float amount)
        {
            if (gamestat == null)
            {
                GD.PrintErr("The GameStatComponent " + this.Name + " requires a GameStatResource. Please add one to node " + this.GetPath());
                return;
            }
            gamestat.StatValue = amount;
            EmitSignal(SignalName.StatChanged);
        }
    }
}