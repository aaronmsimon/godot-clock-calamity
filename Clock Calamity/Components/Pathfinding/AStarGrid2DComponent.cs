using Godot;

namespace Components.Pathfinding
{
    [GlobalClass]
    public partial class AStarGrid2DComponent : Node2D
    {
        [ExportCategory("A* Grid2D Component")]

        [ExportGroup("Setup")]
        [Export] public TileMap tileMap { get; private set; }
        [Export] AStarGrid2D.DiagonalModeEnum diagonalMode;
        
        [ExportGroup("Barriers")]
        [Export] private bool hasBarriers;
        [Export] private int barrierLayer;

        public AStarGrid2D astarGrid2D { get; private set; } = new AStarGrid2D();

        public override void _Ready()
        {
            if (tileMap == null)
            {
                GD.PrintErr("A TileMap is required to run the A* Grid2D Component. Please add one to node " + this.GetPath());
                return;
            }

            astarGrid2D.Region = tileMap.GetUsedRect();
            astarGrid2D.CellSize = new Vector2I(tileMap.TileSet.TileSize.X, tileMap.TileSet.TileSize.Y);
            astarGrid2D.DiagonalMode = diagonalMode;
            astarGrid2D.Update();

            SetBarriers();
        }

        private void SetBarriers()
        {
            if (hasBarriers)
            {
                for (int x = 0; x < astarGrid2D.Region.Size.X; x++)
                {
                    for (int y = 0; y < astarGrid2D.Region.Size.Y; y++)
                    {
                        Vector2I tilePos = new Vector2I(x, y);
                        TileData tileData = tileMap.GetCellTileData(barrierLayer, tilePos);

                        if (tileData != null)
                        {
                            astarGrid2D.SetPointSolid(tilePos, true);
                        }
                    }
                }
            }
        }
    }
}
