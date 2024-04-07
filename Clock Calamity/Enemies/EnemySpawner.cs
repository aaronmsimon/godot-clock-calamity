using Godot;
using Godot.Collections;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene enemy;

    private Array<Node> spawnPoints;

    public override void _Ready()
    {
        spawnPoints = GetChildren();

        foreach(Area2D spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(Area2D spawnPoint)
    {
        CollisionShape2D shape = spawnPoint.GetNode<CollisionShape2D>("CollisionShape2D");
        Vector2 size = shape.Shape.GetRect().Size;

        float x = GD.Randf() * size.X;
        float y = GD.Randf() * size.Y;

        Node2D enemyInstance = (Node2D)enemy.Instantiate();
        // projectileInstance.Set("speed", projectileSpeed);
        // projectileInstance.Set("damage", projectileDamage);
        enemyInstance.GlobalPosition = new Vector2(x / 2, y / 2) + spawnPoint.GlobalPosition;
        enemyInstance.Rotation += Mathf.DegToRad(90);
        Node parent = GetNode<Node>("%Enemies");
        parent.AddChild(enemyInstance);
    }
}
