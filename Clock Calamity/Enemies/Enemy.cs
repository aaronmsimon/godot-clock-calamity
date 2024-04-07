using Godot;
using Godot.Collections;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {
        [Export] public float speed { get; private set;} = 100;
        [Export] public int health { get; private set; } = 3;
        [Export] private Array<Vector2I> paths { get; set; }

        private Area2D collider;

        private AStarGrid2D astarGrid;
        private Vector2 windowSize;
        
        private int tileSize = 64;
        private int pathIndex = 0;
        private Array<Vector2I> currentPath;
        private int cellIndex = 0;

        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        public override void _Ready()
        {
            collider = GetNode<Area2D>("Area2D");

            windowSize = GetViewportRect().Size;

            astarGrid = new AStarGrid2D();
            astarGrid.Region = new Rect2I(-1, -1, (int)windowSize.X / tileSize + 2, (int)windowSize.Y / tileSize + 2);
            astarGrid.CellSize = new Vector2I(tileSize, tileSize);
            astarGrid.Update();

            SetNextPath();
        }

        public override void _Process(double delta)
        {
            if (currentPath == null) return;

            Vector2I targetCell = currentPath[cellIndex + 1];
            Vector2 targetPos = new Vector2((targetCell.X - 1) * tileSize + tileSize / 2, (targetCell.Y - 1) * tileSize + tileSize / 2);

            float targetX = Mathf.MoveToward(GlobalPosition.X, targetPos.X, speed * (float)delta);
            float targetY = Mathf.MoveToward(GlobalPosition.Y, targetPos.Y, speed * (float)delta);
            GlobalPosition = new Vector2(targetX, targetY);
            Vector2 vecDiff = GlobalPosition - targetPos;
            float sqrMag = vecDiff.Length() * vecDiff.Length();
            // GD.Print("cell: " + targetCell + " pos: " + targetPos + " global pos: " + GlobalPosition + " mag: " + sqrMag);

            // if (sqrMag < 0.0001f)
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

        private void NextCell()
        {
            cellIndex++;
            if (cellIndex >= currentPath.Count - 1)
            {
                cellIndex = 0;
                SetNextPath();
            }
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
