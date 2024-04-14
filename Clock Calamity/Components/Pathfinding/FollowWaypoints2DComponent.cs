using Godot;
using Godot.Collections;

namespace Components.Pathfinding
{
    [GlobalClass]
    public partial class FollowWaypoints2DComponent : Node
    {
        public AStarGrid2DComponent AStarGrid2DComponent { get; set; }
        public Waypoints2DResource Waypoints2DResource { get; set; }

        public Vector2I CurrentCell { get; private set; }

        private AStarGrid2D astar;
        private Array<Vector2I> waypoints;
        private int waypointIndex = 0;
        private Array<Vector2I> cells;
        private int cellIndex = 0;
        private bool followingWaypoints = true;

        [Signal] public delegate void EndOfPathEventHandler();
        [Signal] public delegate void EndOfWaypointsEventHandler();

        public override void _Ready()
        {
            CallDeferred("SetupNode");
        }

        private void SetupNode()
        {
            if (AStarGrid2DComponent == null)
            {
                GD.PrintErr("The component FollowWaypoints2DComponent requires an AStarGrid2D. Please add one to node " + this.GetPath());
                return;
            }
            if (Waypoints2DResource == null)
            {
                GD.PrintErr("The component FollowWaypoints2DComponent requires a Waypoints2DResource. Please add one to node " + this.GetPath());
                return;
            }

            astar = AStarGrid2DComponent.astarGrid2D;
            waypoints = Waypoints2DResource.waypoints;

            CurrentCell = waypoints[waypointIndex];
            CalculatePath();
        }

        public void NextCell()
        {
            if (!followingWaypoints || GetNextCell() == null) return;

            if (cellIndex < cells.Count - 1)
            {
                cellIndex++;
                UpdateCurrentCell();
            }
            else
            {
                EmitSignal(SignalName.EndOfPath);
                NextWaypoint();
            }
        }

        public Vector2I? GetNextCell()
        {
            if (cellIndex < cells.Count - 1)
            {
                return cells[cellIndex + 1];
            }
            else if (waypointIndex < waypoints.Count - 1)
            {
                return waypoints[waypointIndex + 1];
            }
            else
            {
                return null;
            }
        }

        public Vector2 GetCellPosition(Vector2I cell)
        {
            return astar.GetPointPosition(cell) + new Vector2(astar.CellSize.X / 2, astar.CellSize.Y / 2);
        }

        private void NextWaypoint()
        {
            if (waypointIndex < waypoints.Count - 2)
            {
                waypointIndex++;
                CalculatePath();
                cellIndex = 0;
                UpdateCurrentCell();
            }
            else
            {
                followingWaypoints = false;
                EmitSignal(SignalName.EndOfWaypoints);
            }
        }

        private void UpdateCurrentCell()
        {
            CurrentCell = cells[cellIndex];
        }

        private void CalculatePath()
        {
            cells = astar.GetIdPath(waypoints[waypointIndex], waypoints[waypointIndex + 1]);
        }
    }
}
