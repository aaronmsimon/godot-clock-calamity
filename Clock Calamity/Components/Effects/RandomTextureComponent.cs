using Godot;

namespace Components.Effects
{
    [GlobalClass]
    public partial class RandomTextureComponent : Sprite2D
    {
        [ExportCategory("Splat Component")]
        [Export] private Texture2D[] textures;
        [Export] private Color spriteColor;
        [Export] private float scale;
        [Export] private float fadeTime;

        private Timer fadeTimer = new Timer();

        public override void _Ready()
        {
            if (textures == null || textures.Length == 0)
            {
                GD.PrintErr($"RandomTextureComponent on { this.Name } needs at least one texture.");
                return;
            }

            int randomIndex = (int)GD.RandRange(0, textures.Length - 1);
            Texture = textures[randomIndex];
            Modulate = spriteColor;
            Scale = new Vector2(scale, scale);
            
            AddChild(fadeTimer);
            fadeTimer.Start(fadeTime);

            fadeTimer.Timeout += () => QueueFree();
        }

        public override void _Process(double delta)
        {
            float alpha = (float)fadeTimer.TimeLeft / fadeTime;
            Modulate = new Color(spriteColor.R, spriteColor.G, spriteColor.B, alpha);
        }
    }
}
