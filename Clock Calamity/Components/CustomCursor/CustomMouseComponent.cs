using Godot;

namespace Components.CustomCursor
{
    public partial class CustomMouseComponent : Node2D
    {
        [ExportCategory("Custom Mouse Cursor")]
        [Export] private bool useCustomCursor;
        [Export] private Texture cursorTexture;

        public override void _Ready()
        {
            // Set node ahead of all others so cursor renders on top of everything else
            ZIndex = 99;

            if (useCustomCursor)
            {
                if (cursorTexture == null)
                {
                    GD.PrintErr("Please supply an image if you'd like to use a custom mouse cursor.");
                    return;
                }
                Input.SetCustomMouseCursor(cursorTexture);
            }
        }
    }
}
