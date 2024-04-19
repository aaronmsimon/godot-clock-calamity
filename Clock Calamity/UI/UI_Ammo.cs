using Godot;
using Components.Weapons;
using CC.Player;

namespace UI
{
    public partial class UI_Ammo : Node
    {
        [ExportCategory("UI Ammo")]
        [Export] private PlayerController playerController;

        [ExportGroup("Texture")]
        [Export] private Texture2D ammoTexture;
        [Export] private ShaderMaterial shaderAmmoFull;
        [Export] private ShaderMaterial shaderAmmoEmpty;

        private GridContainer ammoContainer;
        private WeaponResource weaponResource;

        public override void _Ready()
        {
            ammoContainer = GetNode<GridContainer>("%AmmoGridContainer");
            weaponResource = playerController.weaponComponent.weaponResource;
            ammoContainer.Columns = weaponResource.ammoPerMag;

            playerController.weaponComponent.AmmoChanged += OnAmmoChanged;

            OnAmmoChanged();
        }

        private void OnAmmoChanged()
        {
            foreach (Node node in ammoContainer.GetChildren())
            {
                node.QueueFree();
            }
            for (int i = 0; i < weaponResource.ammoMagCurrent; i++)
            {
                TextureRect ammoFull = new TextureRect
                {
                    Texture = ammoTexture,
                    Material = shaderAmmoFull
                };
                ammoContainer.AddChild(ammoFull);
            }
            // for (int i = 0; i < weaponResource.ammoPerMag - weaponResource.ammoMagCurrent; i++)
            // {
            //     TextureRect ammoEmpty = new TextureRect
            //     {
            //         Texture = ammoTexture,
            //         Material = shaderAmmoEmpty
            //     };
            //     ammoHBox.AddChild(ammoEmpty);
            // }
        }
    }
}
