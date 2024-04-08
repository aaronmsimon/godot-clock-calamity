using Godot;
using Godot.Collections;
using System.Threading.Tasks;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {
        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;
        [Export] private float projectileDamage = 1;

        private float speed { get; set;}
        private int health { get; set; }
        private TileMap tileMap { get; set;}
        private Array<Vector2I> paths { get; set; }
        private int shots { get; set; }
        private float timeBetweenShots { get; set; }

        private PlayerController player;
        private Node2D anchor;
        private Marker2D muzzle;
        private Timer timer = new Timer();

        private AStarGrid2D astarGrid;
        private Vector2 windowSize;
        
        private int tileSize = 64;
        private int tileOffset = 1;
        private int pathIndex = 0;
        private Array<Vector2I> currentPath;
        private int cellIndex = 0;
        private float rotationOffset = 90f;
        private bool firing;

        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        const int TILEMAP_LAYER_BARRIERS = 1;

        public override void _Ready()
        {
            player = GetNode<PlayerController>("../../Player");
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");
            AddChild(timer);

            windowSize = GetViewportRect().Size;

            astarGrid = new AStarGrid2D();
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
                        astarGrid.SetPointSolid(tilePos);
                    }
                }
            }

            SetNextPath();
        }

        public override void _Process(double delta)
        {
            if (currentPath == null) return;
            
            Vector2I targetCell = currentPath[cellIndex + 1];
            Vector2 targetPos = tileMap.MapToLocal(targetCell) - new Vector2(tileOffset * tileSize, tileOffset * tileSize);

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
        }

        private void SetNextPath()
        {
            if (pathIndex >= paths.Count - 1)
            {
                currentPath = null;
                return;
            }

            currentPath = astarGrid.GetIdPath(paths[pathIndex], paths[pathIndex + 1]);
            pathIndex++;
        }

        private async void NextCell()
        {
            cellIndex++;
            if (cellIndex >= currentPath.Count - 1)
            {
                cellIndex = 0;
                firing = true;
                Rotate(GetAngleTo(player.GlobalPosition));
                for (int i = 0; i < shots; i++)
                {
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
