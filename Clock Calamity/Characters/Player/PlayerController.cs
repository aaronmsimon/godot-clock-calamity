using Godot;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;
        [Export] private float projectileDamage = 1;
        [Export] private int ammoMax = 6;
        [Export] private float reloadTime = 1;

        [Signal] public delegate void TakeCoverEventHandler();
        [Signal] public delegate void PeakLeftEventHandler();
        [Signal] public delegate void PeakRightEventHandler();
        [Signal] public delegate void AmmoChangedEventHandler();

        private Node2D anchor;
        private Marker2D muzzle;
        private Sprite2D crosshairs;
        private AnimatedSprite2D playerSprite;

        private int ammoCurrent;
        private bool isReloading = false;
        private Timer reloadTimer = new Timer();

        public override void _Ready()
        {
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            crosshairs = GetNode<Sprite2D>("CrosshairsSprite");
            playerSprite = GetNode<AnimatedSprite2D>("Anchor/AnimatedSprite2D");

            AmmoChanged += OnAmmoChanged;
            ammoCurrent = ammoMax;
            EmitSignal(SignalName.AmmoChanged);

            AddChild(reloadTimer);

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

            if (Input.IsActionJustPressed("reload"))
            {
                Reload();
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
            if (ammoCurrent > 0 && !isReloading)
            {
                Node2D projectileInstance = (Node2D)projectile.Instantiate();
                projectileInstance.Set("speed", projectileSpeed);
                projectileInstance.Set("damage", projectileDamage);
                projectileInstance.GlobalPosition = muzzle.GlobalPosition;
                projectileInstance.Rotation = anchor.Rotation;
                Node parent = GetParent();
                parent.AddChild(projectileInstance);
                ammoCurrent--;
                EmitSignal(SignalName.AmmoChanged);
            }
        }

        private async void Reload()
        {
            isReloading = true;
            reloadTimer.Start(reloadTime);
            playerSprite.Play("reload");
            await ToSignal(reloadTimer, Timer.SignalName.Timeout);
            playerSprite.Play("gun");

            ammoCurrent = ammoMax;
            EmitSignal(SignalName.AmmoChanged);
            isReloading = false;
        }

        private void OnAmmoChanged()
        {
            GD.Print("Ammo: " + ammoCurrent);
        }
    }
}
