using Godot;

namespace Components.Inputs
{
    [GlobalClass]
    public partial class ButtonInputComponent : Node
    {
        [Export] private StringName buttonAction;

        [Signal] public delegate void OnButtonPressedEventHandler();
        [Signal] public delegate void OnButtonReleasedEventHandler();

        public override void _Ready()
        {
            if (buttonAction == "")
            {
                GD.PrintErr("The ButtonInputComponent " + this.Name + " requires both a Button Action. Please make sure it is configured on node " + this.GetPath());
                return;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (buttonAction == "") return;

            if (@event.IsActionPressed(buttonAction))
            {
                EmitSignal(SignalName.OnButtonPressed);
            }

            if (@event.IsActionReleased(buttonAction))
            {
                EmitSignal(SignalName.OnButtonReleased);
            }
        }
    }
}