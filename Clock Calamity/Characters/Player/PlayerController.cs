using Components.Weapons;
using Godot;

namespace CC.Characters
{
    public partial class PlayerController : Node2D
    {
        [Export] private WeaponComponent weapon;

        [Signal] public delegate void TakeCoverEventHandler();
        [Signal] public delegate void PeakLeftEventHandler();
        [Signal] public delegate void PeakRightEventHandler();
        [Signal] public delegate void AmmoChangedEventHandler();

        private Node2D anchor;
        private Marker2D muzzle;
        private AnimatedSprite2D playerSprite;

        private int ammoCurrent;
        private bool isReloading = false;
        private Timer reloadTimer = new Timer();

        public override void _Ready()
        {
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            playerSprite = GetNode<AnimatedSprite2D>("Anchor/AnimatedSprite2D");

            AmmoChanged += OnAmmoChanged;
            ammoCurrent = weapon.weaponResource.ammoPerMag;
            EmitSignal(SignalName.AmmoChanged);

            AddChild(reloadTimer);
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

        private void AimAtMouse()
        {
            anchor.Rotate(anchor.GetAngleTo(GetGlobalMousePosition()));
        }

        private void Fire()
        {
            if (ammoCurrent > 0 && !isReloading)
            {
                Node2D projectileInstance = (Node2D)weapon.weaponResource.projectile.Instantiate();
                projectileInstance.Set("speed", weapon.weaponResource.projectileSpeed);
                projectileInstance.Set("damage", weapon.weaponResource.projectileDamage);
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
            reloadTimer.Start(weapon.weaponResource.reloadTime);
            playerSprite.Play("reload");
            await ToSignal(reloadTimer, Timer.SignalName.Timeout);
            playerSprite.Play("gun");

            ammoCurrent = weapon.weaponResource.ammoPerMag;
            EmitSignal(SignalName.AmmoChanged);
            isReloading = false;
        }

        private void OnAmmoChanged()
        {
            GD.Print("Ammo: " + ammoCurrent);
        }
    }
}
