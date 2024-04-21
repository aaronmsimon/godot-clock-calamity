using Godot;
using Components.Weapons;
using CC.Player;

namespace UI
{
    public partial class UI_Health : Node
    {
        [ExportCategory("UI Ammo")]
        [Export] private PlayerController playerController;

        [ExportGroup("Texture")]
        [Export] private Texture2D healthTextureFull;
        [Export] private Texture2D healthTextureEmpty;

        private GridContainer healthContainer;
        private PlayerResource playerResource;

        public override void _Ready()
        {
            healthContainer = GetNode<GridContainer>("%HealthGridContainer");
            playerResource = playerController.playerResource;
            healthContainer.Columns = playerResource.StartingHealth;

            playerResource.HealthChanged += OnHealthChanged;

            OnHealthChanged();
        }

        public override void _ExitTree()
        {
            playerResource.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            foreach (Node node in healthContainer.GetChildren())
            {
                node.QueueFree();
            }
            for (int i = 0; i < playerResource.CurrentHealth; i++)
            {
                TextureRect healthFull = new TextureRect
                {
                    Texture = healthTextureFull
                };
                healthContainer.AddChild(healthFull);
            }
            for (int i = 0; i < playerResource.StartingHealth - playerResource.CurrentHealth; i++)
            {
                TextureRect healthEmpty = new TextureRect
                {
                    Texture = healthTextureEmpty
                };
                healthContainer.AddChild(healthEmpty);
            }
        }
    }
}
