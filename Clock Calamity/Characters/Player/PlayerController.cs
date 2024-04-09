using Godot;
using Components.Weapons;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        [Export] private WeaponComponent weapon;

        [Signal] public delegate void TakeCoverEventHandler();
        [Signal] public delegate void PeakLeftEventHandler();
        [Signal] public delegate void PeakRightEventHandler();

        private Node2D anchor;
        private Marker2D muzzle;
        private AnimatedSprite2D playerSprite;

        public override void _Ready()
        {
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            playerSprite = GetNode<AnimatedSprite2D>("Anchor/AnimatedSprite2D");

            weapon.ReloadStarted += OnReloadStarted;
            weapon.ReloadFinished += OnReloadFinished;
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

            if (Input.IsActionJustPressed("reload"))
            {
                weapon.Reload();
            }
        }

        private void AimAtMouse()
        {
            anchor.Rotate(anchor.GetAngleTo(GetGlobalMousePosition()));
        }

        private void Fire()
        {
            weapon.Fire();
        }

        private void OnReloadStarted()
        {
            playerSprite.Play("reload");
        }

        private void OnReloadFinished()
        {
            playerSprite.Play("gun");
        }
    }
}
