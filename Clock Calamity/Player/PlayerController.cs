using Godot;
using Components.Weapons;
using Components.Movement;
using Components.Inputs;

namespace CC.Player
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
        private FixedMovement2DComponent fixedMovement2DComponent;
        private AxisInputComponent axisInputComponent;

        public override void _Ready()
        {
            muzzle = GetNode<Marker2D>("MuzzleMarker");
            playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            fixedMovement2DComponent = GetNode<FixedMovement2DComponent>("FixedMovement2DComponent");
            axisInputComponent = GetNode<AxisInputComponent>("AxisInputComponent");

            // weapon.ReloadStarted += OnReloadStarted;
            // weapon.ReloadFinished += OnReloadFinished;
        }

        public override void _Process(double delta)
        {
            // Aiming
            AimAtMouse();

            // Peaking
            switch (axisInputComponent.InputAxis)
            {
                case < 0:
                    fixedMovement2DComponent.MoveActorToMarker(peakLeftPos);
                    break;
                case > 0:
                    fixedMovement2DComponent.MoveActorToMarker(peakRightPos);
                    break;
                default:
                    fixedMovement2DComponent.MoveActorToMarker(hidePos);
                    break;
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
