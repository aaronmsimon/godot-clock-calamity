using Godot;

namespace Components.Inputs
{
    [GlobalClass]
    public partial class AxisInputComponent : Node
    {
        [Export] private StringName negativeAction;
        [Export] private StringName positiveAction;

        public float InputAxis { get; private set; }

        public override void _Input(InputEvent @event)
        {
            InputAxis = Input.GetAxis(negativeAction, positiveAction);
        }
    }
}