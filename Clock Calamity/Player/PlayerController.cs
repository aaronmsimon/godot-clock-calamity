using Godot;
using Components.Weapons;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        [ExportCategory("Player Controller")]
        [ExportGroup("Peaking")]
        [Export] private Marker2D peakLeftPos;
        [Export] private Marker2D hidePos;
        [Export] private Marker2D peakRightPos;
        // [Export] public WeaponComponent weapon { get; private set; }

        private Marker2D muzzle;
        private AnimatedSprite2D playerSprite;

        public override void _Ready()
        {
            muzzle = GetNode<Marker2D>("MuzzleMarker");
            playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

            // weapon.ReloadStarted += OnReloadStarted;
            // weapon.ReloadFinished += OnReloadFinished;
        }

        public override void _Process(double delta)
        {
            // Aiming
            AimAtMouse();

            // Peaking
            if (Input.IsActionPressed("peak_left"))
            {
                GlobalPosition = peakLeftPos.GlobalPosition;
            }
            if (Input.IsActionPressed("peak_right"))
            {
                GlobalPosition = peakRightPos.GlobalPosition;
            }
            if (Input.IsActionJustReleased("peak_left") || Input.IsActionJustReleased("peak_right"))
            {
                GlobalPosition = hidePos.GlobalPosition;
            }

            // if (Input.IsActionJustPressed("fire"))
            // {
            //     Fire();
            // }

            // if (Input.IsActionJustPressed("reload"))
            // {
            //     weapon.Reload();
            // }
        }

        private void AimAtMouse()
        {
            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 mouse2muzzle = muzzle.GlobalPosition - mousePos;
            Vector2 player2muzzle = muzzle.GlobalPosition - GlobalPosition;
            // This mostly reduces stuttering when aiming near the player
            if (mouse2muzzle.Length() < player2muzzle.Length()) return;
            Rotate(muzzle.GetAngleTo(mousePos));
        }

        private void Fire()
        {
            // weapon.Fire();
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
