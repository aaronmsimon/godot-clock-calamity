using Godot;
using Components.Movement;
using Components.Inputs;
using Components.Game;
using Components.Weapons;

namespace CC.Player
{
    public partial class PlayerController : Node2D, IDamageable
    {
        [ExportCategory("Player Controller")]

        [ExportGroup("Info")]
        [Export] private PlayerResource playerResource;

        [ExportGroup("Peaking")]
        [Export] private Marker2D peakLeftPos;
        [Export] private Marker2D hidePos;
        [Export] private Marker2D peakRightPos;
        // [Export] public WeaponComponent weapon { get; private set; }

        [ExportGroup("Invincibility")]
        [Export] private bool invincible;

        public WeaponComponent weaponComponent { get; private set; }

        private Marker2D muzzle;
        private AnimatedSprite2D playerSprite;
        private FixedMovement2DComponent fixedMovement2DComponent;
        private AxisInputComponent axisInputComponent;
        private ButtonInputComponent fireButtonInputComponent;
        private ButtonInputComponent reloadButtonInputComponent;
        private GameStatComponent shotsFiredStatComponent;

        public override void _Ready()
        {
            // Get nodes
            muzzle = GetNode<Marker2D>("MuzzleMarker");
            playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            fixedMovement2DComponent = GetNode<FixedMovement2DComponent>("FixedMovement2DComponent");
            axisInputComponent = GetNode<AxisInputComponent>("AxisInputComponent");
            fireButtonInputComponent = GetNode<ButtonInputComponent>("FireButtonInputComponent");
            reloadButtonInputComponent = GetNode<ButtonInputComponent>("ReloadButtonInputComponent");
            shotsFiredStatComponent = GetNode<GameStatComponent>("ShotsFiredStatComponent");
            weaponComponent = GetNode<WeaponComponent>("WeaponComponent");

            // Setup listeners
            fireButtonInputComponent.OnButtonPressed += OnFireButtonPressed;
            reloadButtonInputComponent.OnButtonPressed += OnReloadButtonPressed;

            weaponComponent.ReloadStarted += OnReloadStarted;
            weaponComponent.ReloadFinished += OnReloadFinished;

            playerResource.HealthChanged += OnHealthChanged;
            playerResource.Die += OnDie;

            // Setup starting variables
            playerResource.CurrentHealth = playerResource.StartingHealth;
            playerResource.IsAlive = true;
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

        public void TakeDamage(int damage)
        {
            if (!invincible)
            {
                playerResource.CurrentHealth -= damage;
            }
        }

        private void OnFireButtonPressed()
        {
            shotsFiredStatComponent.UpdateStatAddAmount(1);
            weaponComponent.Fire();
        }

        private void OnReloadButtonPressed()
        {
            weaponComponent.Reload();
        }

        private void OnReloadStarted()
        {
            playerSprite.Play("reload");
        }

        private void OnReloadFinished()
        {
            playerSprite.Play("gun");
        }

        private void OnHealthChanged()
        {
            GD.Print($"Player Health: {playerResource.CurrentHealth}");
        }

        private void OnDie()
        {
            GD.Print("Player died");
            QueueFree();
        }
    }
}
