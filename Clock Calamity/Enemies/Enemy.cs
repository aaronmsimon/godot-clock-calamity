using Godot;
using Godot.Collections;
using System.Threading.Tasks;
using Components.Pathfinding;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {

        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;
        [Export] private float projectileDamage = 1;

        private float speed { get; set;}
        private int health { get; set; }
        private Array<Vector2I> paths { get; set; }
        private int shots { get; set; }
        private float timeBetweenShots { get; set; }
        private AStarGrid2DComponent astarGrid2DComponent { get; set; }

        private PlayerController player;
        private Node2D anchor;
        private Marker2D muzzle;
        private Timer timer = new Timer();

        private int pathIndex = 0;
        private Array<Vector2I> currentPath;
        private int cellIndex = 0;
        private float rotationOffset = 90f;
        private bool firing = false;
        private Vector2I lastCellOccupied;

        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        public override void _Ready()
        {
            player = GetNode<PlayerController>("../../Player");
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            AddChild(timer);

            SetNextPath();
        }

        public override void _Process(double delta)
        {
            if (currentPath == null) return;
            
            Vector2I targetCell = currentPath[cellIndex + 1];
            Vector2 targetPos = astarGrid2DComponent.tileMap.MapToLocal(targetCell) + astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize;

            float targetX = Mathf.MoveToward(GlobalPosition.X, targetPos.X, speed * (float)delta);
            float targetY = Mathf.MoveToward(GlobalPosition.Y, targetPos.Y, speed * (float)delta);
            if (!firing)
            {
                Rotate(GetAngleTo(targetPos));
                GlobalPosition = new Vector2(targetX, targetY);
            }

            if (GlobalPosition == targetPos)
            {
                NextCell();
            }

            Vector2I currentCell = astarGrid2DComponent.tileMap.LocalToMap(GlobalPosition - astarGrid2DComponent.tileOffset * astarGrid2DComponent.tileMap.TileSet.TileSize);
            if (lastCellOccupied != currentCell)
            {
                // astarGrid2DComponent.astarGrid2D.SetPointSolid(lastCellOccupied, false);
                // astarGrid2DComponent.astarGrid2D.SetPointSolid(currentCell, true);
                lastCellOccupied = currentCell;
            }
        }

        private void SetNextPath()
        {
            if (pathIndex >= paths.Count - 1)
            {
                currentPath = null;
                return;
            }

            currentPath = astarGrid2DComponent.astarGrid2D.GetIdPath(paths[pathIndex], paths[pathIndex + 1]);
            pathIndex++;
        }

        private async void NextCell()
        {
            cellIndex++;
            if (cellIndex >= currentPath.Count - 1)
            {
                cellIndex = 0;
                firing = true;
                for (int i = 0; i < shots; i++)
                {
                    Rotate(GetAngleTo(player.GlobalPosition));
                    await Fire();
                }
                firing = false;
                SetNextPath();
            }
        }

        private async Task Fire()
        {
            Node2D projectileInstance = (Node2D)projectile.Instantiate();
            projectileInstance.Set("speed", projectileSpeed);
            projectileInstance.Set("damage", projectileDamage);
            projectileInstance.GlobalPosition = muzzle.GlobalPosition;
            projectileInstance.Rotation = Rotation;
            Node parent = GetParent();
            parent.AddChild(projectileInstance);

            timer.Start(timeBetweenShots);
            await ToSignal(timer, Timer.SignalName.Timeout);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            EmitSignal(SignalName.Damaged);

            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            EmitSignal(SignalName.Died);
            QueueFree();
        }
    }
}
