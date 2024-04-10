using System;
using CC.Characters;
using Godot;
using Godot.Collections;

namespace Components.Pathfinding
{
    public partial class FollowWaypointsComponent : Node
    {
        [Export] private Enemy actor;
        [Export] public NPCResource npcResource { private get; set; }
        [Export] public GridResource occupiedResource { private get; set; }

        public AStarGrid2DComponent astarGrid2DComponent { private get; set; }
        public WaypointsResource pathResource { private get; set; }

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
            if (path == null)
            {
                return;
            }


            Vector2I targetCell = path[cellIndex + 1];
            Vector2 targetPos = astarGrid2DComponent.tileMap.MapToLocal(targetCell) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize;

            float targetX = Mathf.MoveToward(actor.GlobalPosition.X, targetPos.X, npcResource.speed * (float)delta);
            float targetY = Mathf.MoveToward(actor.GlobalPosition.Y, targetPos.Y, npcResource.speed * (float)delta);

            if (!firing)
            {
                if (CheckCellOccupied(GetNextCell()))
                {
                    FireAtTarget(Mathf.Inf);
                    return;
                }
                actor.Rotate(actor.GetAngleTo(targetPos));
                actor.GlobalPosition = new Vector2(targetX, targetY);
            }

            if (actor.GlobalPosition == targetPos)
            {
                MoveNextCell();
            }
        }

        private void SetNextPath()
        {
            if (pathIndex >= pathResource.waypoints.Count - 1)
            {
                path = null;
                FireAtTarget(Mathf.Inf);
                return;
            }

            path = astarGrid2DComponent.astarGrid2D.GetIdPath(pathResource.waypoints[pathIndex], pathResource.waypoints[pathIndex + 1]);
            pathIndex++;
        }

        private void MoveNextCell()
        {
            Vector2I nextCell = GetNextCell();
            occupiedResource.Data[nextCell.X, nextCell.Y] = true;
            occupiedResource.Data[path[cellIndex].X, path[cellIndex].Y] = false;
            
            cellIndex++;
            if (cellIndex >= path.Count - 1)
            {
                cellIndex = 0;
                FireAtTarget(npcResource.shots);
                SetNextPath();
            }
        }

        private async void FireAtTarget(float shotCount)
        {
                firing = true;
                for (int i = 0; i < shotCount; i++)
                {
                    actor.Rotate(actor.GetAngleTo(actor.player.GlobalPosition));
                    await actor.Fire();
                }
                firing = false;
        }

        private Vector2I GetNextCell()
        {
            if (cellIndex < path.Count - 1)
            {
                return path[cellIndex + 1];
            }
            else if (pathIndex < pathResource.waypoints.Count - 1)
            {
                return pathResource.waypoints[pathIndex + 1];
            }
            else
            {
                return new Vector2I(0,0);
            }
        }

        private bool CheckCellOccupied(Vector2I cellCheck)
        {
            return occupiedResource.Data[cellCheck.X, cellCheck.Y];
        }
    }
}
