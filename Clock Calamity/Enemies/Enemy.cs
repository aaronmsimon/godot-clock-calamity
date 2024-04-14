using Godot;
using Components.Pathfinding;

namespace CC.Enemies
{
    [GlobalClass]
    public partial class Enemy : Node2D
    {
        [ExportGroup("A* Pathfinding Resources")]
        [Export] private AStarGrid2DComponent aStarGrid2DComponent;
        [Export] private Waypoints2DResource waypoints2DResource;

        [ExportGroup("Movement")]
        [Export] private float speed;

        private FollowWaypoints2DComponent followWaypoints2DComponent;
        private Vector2 offset;

        public override void _Ready()
        {
            followWaypoints2DComponent = GetNode<FollowWaypoints2DComponent>("FollowWaypoints2DComponent");

            followWaypoints2DComponent.AStarGrid2DComponent = aStarGrid2DComponent;
            followWaypoints2DComponent.Waypoints2DResource = waypoints2DResource;

            offset = aStarGrid2DComponent.tileMap.Position;

            if (speed == 0)
            {
                GD.PrintErr("Speed is set to zero. Is this right?");
            }
        }

        public override void _Process(double delta)
        {
            Pathfinding((float)delta);
        }

        private void Pathfinding(float delta)
        {
            if (followWaypoints2DComponent.GetNextCell() == null) return;

            Vector2I targetCell = (Vector2I)followWaypoints2DComponent.GetNextCell();
            Vector2 targetPos = followWaypoints2DComponent.GetCellPosition(targetCell) + offset;

            float targetX = Mathf.MoveToward(GlobalPosition.X, targetPos.X, speed * delta);
            float targetY = Mathf.MoveToward(GlobalPosition.Y, targetPos.Y, speed * delta);

            Rotate(GetAngleTo(targetPos));
            GlobalPosition = new Vector2(targetX, targetY);

            if (GlobalPosition == targetPos)
            {
                followWaypoints2DComponent.NextCell();
            }
        }
    }
}
