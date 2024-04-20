using Godot;

[GlobalClass]
public partial class CharacterResource : Resource
{
    [ExportCategory("Character Resource")]
    [Export] public PackedScene character { get; private set; }

    [ExportGroup("Stats")]
    [Export] public int health { get; private set; }
    [Export] public float speed { get; private set; }

    [ExportGroup("Attack")]
    [Export] public int shots { get; private set; }
    [Export] public float timeBetweenShots { get; private set; }

    [ExportGroup("Death")]
    [Export] public PackedScene Splat { get; private set; }
}
