using Godot;
using Godot.Collections;
using Components.Pathfinding;

public partial class SpawnPoint : Marker2D
{
    [ExportCategory("Spawn Point")]
    [Export] private AStarGrid2DComponent astarGrid2DComponent;
    [Export] private WaypointsResource pathResource;
    [Export] private EnemyResource enemyResource;

    [ExportGroup("Behavior")]
    [Export] private int enemiesToSpawn;
    [Export] private float timeBetweenSpawns;

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
        Node2D enemyInstance = (Node2D)enemyResource.enemy.Instantiate();
        // Set Position
        enemyInstance.GlobalPosition = GlobalPosition;
        // Send Resources
        enemyInstance.Set("astarGrid2DComponent", astarGrid2DComponent);
        enemyInstance.Set("pathResource", pathResource);
        enemyInstance.Set("enemyResource", enemyResource);
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
