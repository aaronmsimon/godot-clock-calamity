using Godot;

public partial class EnemyResource : Resource
{
    [ExportGroup("Enemy")]
    [Export] public PackedScene enemy { get; private set; }
    [ExportSubgroup("Enemy Behavior")]
    [Export] public int health { get; set; }
    [Export] public float speed { get; private set; }
    [Export] public int shots { get; private set; }
    [Export] public float timeBetweenShots { get; private set; }
}
