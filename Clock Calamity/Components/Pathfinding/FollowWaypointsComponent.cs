using CC.Characters;
using Godot;
using Godot.Collections;

namespace Components.Pathfinding
{
    public partial class FollowWaypointsComponent : Node
    {
        [Export] private Enemy actor;
        [Export] public NPCResource npcResource { private get; set; }

        public AStarGrid2DComponent astarGrid2DComponent { private get; set; }
        public WaypointsResource pathResource { get; set; }

        // Path tracking
        private Array<Vector2I> path;
        private int pathIndex = 0;
        private int cellIndex = 0;

        //other
        private float rotationOffset = 90f;
        private Vector2I lastCellOccupied;

        private bool firing;

        public override void _Ready()
        {
            CallDeferred("SetNextPath");
        }

        public override void _Process(double delta)
        {
            if (npcResource == null)
            {
                GD.PrintErr("An NPC Resource is required to use the Follow Waypoints Component.");
                return;
            }
            if (pathResource == null)
            {
                GD.PrintErr("A Path Resource is required to use the Follow Waypoints Component.");
                return;
            }
            if (path == null) return;

            Vector2I targetCell = path[cellIndex + 1];
            Vector2 targetPos = astarGrid2DComponent.tileMap.MapToLocal(targetCell) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize;

            float targetX = Mathf.MoveToward(actor.GlobalPosition.X, targetPos.X, npcResource.speed * (float)delta);
            float targetY = Mathf.MoveToward(actor.GlobalPosition.Y, targetPos.Y, npcResource.speed * (float)delta);

            if (!firing)
            {
                actor.Rotate(actor.GetAngleTo(targetPos));
                actor.GlobalPosition = new Vector2(targetX, targetY);
            }

            if (actor.GlobalPosition == targetPos)
            {
                NextCell();
            }

            Vector2I currentCell = astarGrid2DComponent.tileMap.LocalToMap(actor.GlobalPosition - astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize);
            if (lastCellOccupied != currentCell)
            {
                // astarGrid2DComponent.astarGrid2D.SetPointSolid(lastCellOccupied, false);
                // astarGrid2DComponent.astarGrid2D.SetPointSolid(currentCell, true);
                lastCellOccupied = currentCell;
            }
        }

        private void SetNextPath()
        {
            if (pathIndex >= pathResource.waypoints.Count - 1)
            {
                path = null;
                return;
            }

            path = astarGrid2DComponent.astarGrid2D.GetIdPath(pathResource.waypoints[pathIndex], pathResource.waypoints[pathIndex + 1]);
            pathIndex++;
        }

        private async void NextCell()
        {
            cellIndex++;
            if (cellIndex >= path.Count - 1)
            {
                cellIndex = 0;
                firing = true;
                for (int i = 0; i < npcResource.shots; i++)
                {
                    actor.Rotate(actor.GetAngleTo(actor.player.GlobalPosition));
                    await actor.Fire();
                }
                firing = false;
                SetNextPath();
            }
        }
    }
}
