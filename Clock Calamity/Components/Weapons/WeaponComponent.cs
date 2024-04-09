using Godot;

namespace Components.Weapons
{
    public partial class WeaponComponent : Node
    {
        [ExportCategory("Weapon Component")]
        [Export] public WeaponResource weaponResource { get; private set; }
    }
}
