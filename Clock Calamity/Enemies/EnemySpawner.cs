using Godot;
using Godot.Collections;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene enemy;
    [Export] private float speed;
    [Export] private int health;
    [Export] private int enemyCount;
    [Export] private float timeBetweenEnemies;

    private Array<Node> spawnPoints;

    public override void _Ready()
    {
        spawnPoints = GetChildren();

        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(SpawnPoint spawnPoint)
    {
        // Instantiate Enemy
        Node2D enemyInstance = (Node2D)enemy.Instantiate();
        // Set Position and Rotation
        enemyInstance.GlobalPosition = spawnPoint.GlobalPosition;
        // Send Tilemap
        TileMap tileMap = GetNode<TileMap>("%TileMap");
        enemyInstance.Set("tileMap", tileMap);
        // Add Paths
        Array<Vector2I> paths = spawnPoint.Path.Cells;
        enemyInstance.Set("paths", paths);
        // Set other Public Variables
        enemyInstance.Set("shots", spawnPoint.Path.Shots);
        enemyInstance.Set("timeBetweenShots", spawnPoint.Path.TimeBetweenShots);
        enemyInstance.Set("speed", speed);
        enemyInstance.Set("health", health);
        // Add to Tree
        Node parent = GetNode<Node>("%Enemies");
        parent.AddChild(enemyInstance);
    }
}
