using Godot;
using Godot.Collections;
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
        
        [ExportGroup("A* Pathing")]
        [Export] Array<WaypointsResource> waypointsResource;
        [Export] private bool drawPathing;
        [ExportSubgroup("Line Settings")]
        [Export] private float pathLineWidth = 2;
        [Export] private Color pathLineColor = Colors.DarkGray;
        [Export] private Color pathStartColor = Colors.LightGreen;
        [Export] private Color pathMiddleColor = Colors.LightYellow;
        [Export] private Color pathEndColor = Colors.LightPink;

        public override void _Process(double delta)
        {
            QueueRedraw();
        }

        public override void _Draw()
        {
            DrawAStarGrid2DBarriers();
            DrawAStarGrid2DPathing();
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

        private void DrawAStarGrid2DPathing()
        {
            if (waypointsResource == null) return;
            if (astarGrid2DComponent == null) return;
            if (astarGrid2DComponent.astarGrid2D == null || astarGrid2DComponent.tileMap == null) return;
            if (waypointsResource.Count == 0) return;
            if (!drawPathing) return;

            foreach (WaypointsResource waypoints in waypointsResource)
            {
                for (int i = 0; i < waypoints.waypoints.Count; i++)
                {
                    Vector2I waypoint = waypoints.waypoints[i];
                    Color circleColor = pathMiddleColor;
                    if (i == 0) circleColor = pathStartColor;
                    if (i == waypoints.waypoints.Count - 1) circleColor = pathEndColor;

                    DrawCircle(astarGrid2DComponent.tileMap.MapToLocal(waypoint) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize, astarGrid2DComponent.tileMap.TileSet.TileSize.X / 4, circleColor);

                    if (i > 0)
                    {
                        Vector2 from = astarGrid2DComponent.tileMap.MapToLocal(waypoints.waypoints[i - 1]) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize;
                        Vector2 to = astarGrid2DComponent.tileMap.MapToLocal(waypoints.waypoints[i]) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize;
                        DrawLine(from, to, pathLineColor, pathLineWidth);
                    }
                }
            }
        }
    }
}
