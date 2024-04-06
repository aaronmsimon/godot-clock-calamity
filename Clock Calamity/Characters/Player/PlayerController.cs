using Godot;
using CC.Level;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        private LevelController map;

        [Signal] public delegate void TakeCoverEventHandler();
        [Signal] public delegate void PeakLeftEventHandler();
        [Signal] public delegate void PeakRightEventHandler();
    
        public override void _Ready()
        {
            map = GetParent<LevelController>();
        }

        public override void _Process(double delta)
        {
            AimAtMouse();

            if (Input.IsActionPressed("peak_left"))
            {
                EmitSignal(SignalName.PeakLeft);
            }
            if (Input.IsActionPressed("peak_right"))
            {
                EmitSignal(SignalName.PeakRight);
            }
            if (Input.IsActionJustReleased("peak_left") || Input.IsActionJustReleased("peak_right"))
            {
                EmitSignal(SignalName.TakeCover);
            }
        }

        private void AimAtMouse()
        {
            Rotate(GetAngleTo(GetGlobalMousePosition()));
        }
    }
}
