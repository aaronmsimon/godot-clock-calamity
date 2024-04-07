using Godot;
using Godot.Collections;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene enemy;
    [Export] private float speed;

    private Array<Node> spawnPoints;

    private int enemyRotation = 90;

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
        enemyInstance.Rotation += Mathf.DegToRad(enemyRotation);
        // Add Paths
        Array<Vector2I> paths = spawnPoint.Path.Cells;
        enemyInstance.Set("paths", paths);
        // Set other Public Variables
        enemyInstance.Set("speed", speed);
        // Add to Tree
        Node parent = GetNode<Node>("%Enemies");
        parent.AddChild(enemyInstance);
    }
}
