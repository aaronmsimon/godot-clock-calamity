using Godot;
using Components.Weapons;

namespace UI
{
    public partial class UI_Ammo : Node
    {
        [ExportCategory("UI Ammo")]
        [Export] private WeaponResource weaponResource;
        [Export] private WeaponComponent weaponComponent;

        [ExportGroup("Texture")]
        [Export] private Texture2D ammoTexture;
        [Export] private ShaderMaterial shaderAmmoFull;
        [Export] private ShaderMaterial shaderAmmoEmpty;


        private HBoxContainer ammoHBox;

        public override void _Ready()
        {
            ammoHBox = GetNode<HBoxContainer>("AmmoHBox");

            weaponComponent.AmmoChanged += OnAmmoChanged;
        }

        private void OnAmmoChanged()
        {
            for (int i = 0; i < weaponResource.ammoMagCurrent; i++)
            {
                TextureRect ammoFull = new TextureRect
                {
                    Texture = ammoTexture,
                    Material = shaderAmmoFull
                };
                ammoHBox.AddChild(ammoFull);
            }
            for (int i = 0; i < weaponResource.ammoPerMag - weaponResource.ammoMagCurrent; i++)
            {
                TextureRect ammoEmpty = new TextureRect
                {
                    Texture = ammoTexture,
                    Material = shaderAmmoEmpty
                };
                ammoHBox.AddChild(ammoEmpty);
            }
        }
    }
}
