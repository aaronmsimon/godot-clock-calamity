using Godot;

namespace Components.CustomCursor
{
    [GlobalClass]
    public partial class CustomMouseComponent : Node2D
    {
        [ExportCategory("Custom Mouse Cursor")]
        [Export] private bool useCustomCursor;
        [Export] private Texture2D cursorTexture;

        public override void _Ready()
        {
            // Set node ahead of all others so cursor renders on top of everything else
            ZIndex = 99;

            if (useCustomCursor)
            {
                if (cursorTexture == null)
                {
                    GD.PrintErr("The component CustomMouseComponent requires a cursor texture. Please add one to node " + this.GetPath());
                    return;
                }
                Input.SetCustomMouseCursor(cursorTexture, hotspot: new Vector2(cursorTexture.GetWidth() / 2, cursorTexture.GetHeight() / 2));
            }
        }
    }
}
