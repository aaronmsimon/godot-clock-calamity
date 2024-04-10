using Godot;

namespace Components.Weapons
{
    public partial class WeaponComponent : Node
    {
        [ExportCategory("Weapon Component")]
        [Export] public WeaponResource weaponResource { get; private set; }

        private bool isReloading;
        private Node2D weaponOwner;
        private Node2D anchor;
        private Marker2D muzzle;
        private Timer reloadTimer = new Timer();

        [Signal] public delegate void AmmoChangedEventHandler();
        [Signal] public delegate void ReloadStartedEventHandler();
        [Signal] public delegate void ReloadFinishedEventHandler();
        [Signal] public delegate void ShotFiredEventHandler();

        public override void _Ready()
        {
            weaponOwner = GetOwner<Node2D>();
            anchor = weaponOwner.GetNode<Node2D>("Anchor");
            muzzle = anchor.GetNode<Marker2D>("MuzzleMarker");

            AddChild(reloadTimer);

            AmmoChanged += OnAmmoChanged;

            weaponResource.ammoTotalCurrent = weaponResource.ammoInfinite ? Mathf.Inf : weaponResource.ammoMax;

            SetMaxMagAmmo();
        }

        public void Fire()
        {
            if (weaponResource.ammoMagCurrent > 0 && !isReloading)
            {
                Node2D projectileInstance = (Node2D)weaponResource.projectile.Instantiate();
                projectileInstance.Set("speed", weaponResource.projectileSpeed);
                projectileInstance.Set("damage", weaponResource.projectileDamage);
                projectileInstance.GlobalPosition = muzzle.GlobalPosition;
                projectileInstance.Rotation = anchor.Rotation;
                Node parent = GetParent().GetParent();
                parent.AddChild(projectileInstance);
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
