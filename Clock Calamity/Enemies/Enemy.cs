using Godot;
using Components.Pathfinding;
using Components.Projectiles2D;
using System.Threading.Tasks;

namespace CC.Enemies
{
    [GlobalClass]
    public partial class Enemy : Node2D
    {
        [Export] private CharacterResource characterResource;

        [ExportGroup("A* Pathfinding Resources")]
        [Export] public AStarGrid2DComponent aStarGrid2DComponent;
        [Export] private Waypoints2DResource waypoints2DResource;

        [ExportGroup("Movement")]
        [Export] private float speed;

        [ExportGroup("Firing")]
        [Export] private Marker2D muzzle;
        [Export] private Node2D target;

        private FollowWaypoints2DComponent followWaypoints2DComponent;
        private Vector2 offset;
        private FireProjectileComponent fireProjectileComponent;
        private Timer shotTimer = new Timer();
        private bool isFiring = false;

        public override void _Ready()
        {
            if (aStarGrid2DComponent == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs the AStarGrid2DComponent set.");
                return;
            }
            if (waypoints2DResource == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs the Waypoints2DResource set.");
                return;
            }

            followWaypoints2DComponent = GetNode<FollowWaypoints2DComponent>("FollowWaypoints2DComponent");
            fireProjectileComponent = GetNode<FireProjectileComponent>("FireProjectileComponent");

            followWaypoints2DComponent.AStarGrid2DComponent = aStarGrid2DComponent;
            followWaypoints2DComponent.Waypoints2DResource = waypoints2DResource;

            followWaypoints2DComponent.EndOfPath += OnEndOfPath;
            followWaypoints2DComponent.EndOfWaypoints += OnEndOfWaypoints;

            offset = aStarGrid2DComponent.tileMap.Position;

            AddChild(shotTimer);

            if (fireProjectileComponent == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a FireProjectileComponent.");
                return;
            }
            if (muzzle == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a Muzzle Marker2D.");
                return;
            }
            if (target == null)
            {
                GD.PrintErr($"Enemy at { this.GetPath() } needs a target.");
                return;
            }

            if (speed == 0)
            {
                GD.PrintErr("Speed is set to zero. Is this right?");
            }
        }

        public override void _Process(double delta)
        {
            if (aStarGrid2DComponent == null || waypoints2DResource == null) return;

            Pathfinding((float)delta);
        }

        private void Pathfinding(float delta)
        {
            if (followWaypoints2DComponent.GetNextCell() == null) return;

            Vector2I targetCell = (Vector2I)followWaypoints2DComponent.GetNextCell();
            Vector2 targetPos = followWaypoints2DComponent.GetCellPosition(targetCell) + offset;

            float targetX = Mathf.MoveToward(GlobalPosition.X, targetPos.X, speed * delta);
            float targetY = Mathf.MoveToward(GlobalPosition.Y, targetPos.Y, speed * delta);

            if (!isFiring)
            {
                Rotate(GetAngleTo(targetPos));
                GlobalPosition = new Vector2(targetX, targetY);
            }

            if (GlobalPosition == targetPos)
            {
                followWaypoints2DComponent.NextCell();
            }
        }

        private float Aim(Node2D _target)
        {
            return GetAngleTo(_target.GlobalPosition);
        }

        private async Task Fire(float shots, float timeBetweenShots)
        {
            if (muzzle == null || target == null) return;

            isFiring = true;

            for (int i = 0; i < shots; i++)
            {
                Rotate(Aim(target));
                fireProjectileComponent.Fire(muzzle.GlobalPosition, Rotation);

                shotTimer.Start(timeBetweenShots);
                await ToSignal(shotTimer, Timer.SignalName.Timeout);
            }

            isFiring = false;
            
        }

        private async void OnEndOfPath()
        {
            await Fire(characterResource.shots, characterResource.timeBetweenShots);
        }

        private async void OnEndOfWaypoints()
        {
            await Fire(Mathf.Inf, characterResource.timeBetweenShots);
        }
    }
}
