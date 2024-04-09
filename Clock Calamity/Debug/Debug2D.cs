using Godot;
using Components.Pathfinding;

namespace Debug.Shapes2D
{
    public partial class Debug2D : Node2D
    {
        [ExportCategory("Debug 2D")]

        [ExportGroup("A* Grid2DComponent")]
        [Export] private AStarGrid2DComponent astarGrid2DComponent;
        [Export] private bool drawGrid2DBarriers;
        [Export] private Color barrierColor = Colors.DarkRed;
        
        // [ExportGroup("A* Pathing")]
        // // [Export] private pathing component
        // [Export] private bool drawPathing;
        // [ExportSubgroup("Line Settings")]
        // [Export] private float lineWidth;
        // [Export] private Color lineColor = Colors.DarkGray;
        // [Export] private Color startColor = Colors.RoyalBlue;
        // [Export] private Color endColor = Colors.LightYellow;

        public override void _Ready()
        {
        }

        public override void _Process(double delta)
        {
            QueueRedraw();
        }

        public override void _Draw()
        {
            DrawAStarGrid2DBarriers();
        }

        private void DrawAStarGrid2DBarriers()
        {
            if (astarGrid2DComponent == null) return;
            if (astarGrid2DComponent.astarGrid2D == null || astarGrid2DComponent.tileMap == null) return;
            if (!drawGrid2DBarriers) return;

            for (int x = 0; x < astarGrid2DComponent.astarGrid2D.Region.Size.X; x++)
            {
                for (int y = 0; y < astarGrid2DComponent.astarGrid2D.Region.Size.Y; y++)
                {
                    Vector2I tilePos = new Vector2I(x, y);
                    if (astarGrid2DComponent.astarGrid2D.IsPointSolid(tilePos))
                    {
                        Rect2 cell = new Rect2(astarGrid2DComponent.tileMap.MapToLocal(tilePos) + (astarGrid2DComponent.tileOffset + astarGrid2DComponent.tileOffset / 2) * astarGrid2DComponent.tileMap.TileSet.TileSize, astarGrid2DComponent.tileMap.TileSet.TileSize);
                        DrawRect(cell, barrierColor);
                    }
                }
            }
        }

        // private void DrawAStarGrid2DPathing()
        // {

        // }
    }
}
