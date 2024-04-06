using Godot;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;

        [Signal] public delegate void TakeCoverEventHandler();
        [Signal] public delegate void PeakLeftEventHandler();
        [Signal] public delegate void PeakRightEventHandler();

        private Node2D anchor;
        private Marker2D muzzle;
        private Sprite2D crosshairs;

        public override void _Ready()
        {
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            crosshairs = GetNode<Sprite2D>("CrosshairsSprite");

            Input.MouseMode = Input.MouseModeEnum.Hidden;
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

            if (Input.IsActionJustPressed("fire"))
            {
                Fire();
            }
        }

        public override void _ExitTree()
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        private void AimAtMouse()
        {
            anchor.Rotate(anchor.GetAngleTo(GetGlobalMousePosition()));

            crosshairs.GlobalPosition = GetGlobalMousePosition();
        }

        private void Fire()
        {
            Node2D projectileInstance = (Node2D)projectile.Instantiate();
            projectileInstance.Set("speed", projectileSpeed);
            projectileInstance.GlobalPosition = muzzle.GlobalPosition;
            projectileInstance.Rotation = anchor.Rotation;
            Node parent = GetParent();
            parent.AddChild(projectileInstance);
        }
    }
}
