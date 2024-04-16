using Godot;

namespace Components.Weapons
{
    [GlobalClass]
    public partial class WeaponComponent : Node
    {
        [ExportCategory("Weapon Component")]
        [Export] public WeaponResource weaponResource { get; private set; }

        private bool isReloading;
        private Node2D weaponOwner;
        private Marker2D muzzle;
        private Timer reloadTimer = new Timer();

        [Signal] public delegate void AmmoChangedEventHandler();
        [Signal] public delegate void ReloadStartedEventHandler();
        [Signal] public delegate void ReloadFinishedEventHandler();
        [Signal] public delegate void ShotFiredEventHandler();

        public override void _Ready()
        {
            weaponOwner = GetOwner<Node2D>();
            muzzle = weaponOwner.GetNode<Marker2D>("MuzzleMarker");

            AddChild(reloadTimer);

            AmmoChanged += OnAmmoChanged;

            weaponResource.ammoTotalCurrent = weaponResource.ammoInfinite ? Mathf.Inf : weaponResource.ammoMax;

            SetMaxMagAmmo();
        }

        public void Fire()
        {
            if (weaponResource.ammoMagCurrent > 0 && !isReloading)
            {
                // Create instance
                Node2D instance = (Node2D)weaponResource.projectile.Instantiate();
                // Set position & rotation
                instance.GlobalPosition = muzzle.GlobalPosition;
                instance.Rotation = weaponOwner.Rotation;
                // Set projectile info
                instance.Set("speed", weaponResource.projectileSpeed);
                instance.Set("damage", weaponResource.projectileDamage);
                // Get parent to set child to
                Node parent = GetTree().CurrentScene;
                // Add to scene as a child of this component
                parent.AddChild(instance);
                weaponResource.ammoMagCurrent--;
                EmitSignal(SignalName.ShotFired);
                EmitSignal(SignalName.AmmoChanged);
            }
        }

        public async void Reload()
        {
            isReloading = true;

            reloadTimer.Start(weaponResource.reloadTime);
            EmitSignal(SignalName.ReloadStarted);

            await ToSignal(reloadTimer, Timer.SignalName.Timeout);
            EmitSignal(SignalName.ReloadFinished);

            SetMaxMagAmmo();

            isReloading = false;
        }

        private void SetMaxMagAmmo()
        {
            float ammoNeeded = weaponResource.ammoPerMag - weaponResource.ammoMagCurrent;
            float ammoReloaded = Mathf.Min(weaponResource.ammoTotalCurrent, ammoNeeded);
            weaponResource.ammoTotalCurrent -= ammoReloaded;
            weaponResource.ammoMagCurrent += ammoReloaded;
            EmitSignal(SignalName.AmmoChanged);
        }

        private void OnAmmoChanged()
        {
            // GD.Print("Magazine Ammo: " + weaponResource.ammoMagCurrent + "Total Ammo: " + weaponResource.ammoTotalCurrent);
        }
    }
}
