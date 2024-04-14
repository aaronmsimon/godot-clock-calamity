using Godot;

namespace Components.Inputs
{
    [GlobalClass]
    public partial class AxisInputComponent : Node
    {
        [Export] private StringName negativeAction;
        [Export] private StringName positiveAction;

        public float InputAxis { get; private set; }

        public override void _Ready()
        {
            if (negativeAction == "" || positiveAction == "")
            {
                GD.PrintErr("The AxisInputComponent " + this.Name + " requires both a Negative and Positive Action. Please make sure both are configured on node " + this.GetPath());
                return;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (negativeAction == "" || positiveAction == "") return;

            InputAxis = Input.GetAxis(negativeAction, positiveAction);
        }
    }
}