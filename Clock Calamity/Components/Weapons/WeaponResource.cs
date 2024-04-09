using Godot;

namespace Components.Weapons
{
    public partial class WeaponResource : Resource
    {
        [ExportCategory("Weapon Resource")]
        [Export] public string weaponName { get; private set; }
        [Export] public PackedScene projectile { get; private set; }
        [Export] public int projectileSpeed { get; private set; }
        [Export] public int projectileDamage { get; private set; }
        [Export] public int projectilesPerShot { get; private set; }
        [Export] public int ammoPerMag { get; private set; }
        [Export] public float reloadTime { get; private set; }
        
        [ExportGroup("Max Ammo")]
        [Export] public bool ammoInfinite { get; private set; }
        [Export] public int ammoMax { get; private set; }

        public float ammoMagCurrent { get; set; }
        public float ammoTotalCurrent { get; set; }
    }
}
