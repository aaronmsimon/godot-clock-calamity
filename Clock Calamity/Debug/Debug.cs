using CC.Characters;
using Godot;

public partial class Debug : Node2D
{
    private TileMap tileMap;
    private AStarGrid2D astarGrid = new AStarGrid2D();
    private int tileSize = 64;
    private int tileOffset = 1;
    const int TILEMAP_LAYER_BARRIERS = 1;

    public override void _Ready()
    {
        tileMap = GetNode<TileMap>("%TileMap");
        astarGrid.Region = tileMap.GetUsedRect();
        astarGrid.CellSize = new Vector2I(tileSize, tileSize);
        astarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles;
        astarGrid.Update();

        for(int x = 0; x < astarGrid.Region.Size.X; x++)
        {
            for(int y = 0; y < astarGrid.Region.Size.Y; y++)
            {
                Vector2I tilePos = new Vector2I(x, y);
                TileData tileData = tileMap.GetCellTileData(TILEMAP_LAYER_BARRIERS, tilePos);

                if (tileData != null)
                {
                    astarGrid.SetPointSolid(tilePos, true);
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (astarGrid != null)
        {
            for(int x = 0; x < astarGrid.Region.Size.X; x++)
            {
                for(int y = 0; y < astarGrid.Region.Size.Y; y++)
                {
                    Vector2I tilePos = new Vector2I(x, y);
                    if (astarGrid.IsPointSolid(tilePos))
                    {
                        Rect2 cell = new Rect2(tileMap.MapToLocal(tilePos) - new Vector2(96, 96), new Vector2(tileSize, tileSize));
                        DrawRect(cell, new Color(1,0,0,.5f));
                    }
                }
            }
        }
    }
}
