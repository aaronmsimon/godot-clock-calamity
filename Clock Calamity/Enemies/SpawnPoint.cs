using Godot;
using Godot.Collections;

public partial class SpawnPoint : Marker2D
{
    [Export] private AStarPathResource path;
    [Export] private int enemiesToSpawn;
    [Export] private float timeBetweenSpawns;
    [Export] private PackedScene enemy;
    [Export] private float speed;
    [Export] private int health;
    [Export] private int shots;
    [Export] private float timeBetweenShots;

    private int enemyCount = 0;
    private Timer timer = new Timer();

    public override void _Ready()
    {
        AddChild(timer);
        timer.Timeout += OnTimerTimeout;

        OnTimerTimeout();
    }

    private void SpawnEnemy()
    {
        // Increment enemies spawned counter
        enemyCount++;
        // Instantiate Enemy
        Node2D enemyInstance = (Node2D)enemy.Instantiate();
        // Set Position and Rotation
        enemyInstance.GlobalPosition = GlobalPosition;
        // Send Tilemap
        TileMap tileMap = GetNode<TileMap>("%TileMap");
        enemyInstance.Set("tileMap", tileMap);
        // Add Paths
        Array<Vector2I> paths = path.Cells;
        enemyInstance.Set("paths", paths);
        // Set other Public Variables
        enemyInstance.Set("shots", shots);
        enemyInstance.Set("timeBetweenShots", timeBetweenShots);
        enemyInstance.Set("speed", speed);
        enemyInstance.Set("health", health);
        // Add to Tree
        Node parent = GetNode<Node>("%Enemies");
        parent.AddChild(enemyInstance);
    }

    private void OnTimerTimeout()
    {
        if (enemyCount < enemiesToSpawn)
        {
            SpawnEnemy();
            // Start next timer
            timer.Start(timeBetweenSpawns);
        }
    }
}
