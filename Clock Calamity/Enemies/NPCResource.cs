using Godot;

public partial class NPCResource : Resource
{
    [ExportCategory("NPC Resource")]
    [Export] public PackedScene npc { get; private set; }
    [ExportGroup("Behavior")]
    [Export] public int health { get; set; }
    [Export] public float speed { get; private set; }
    [Export] public int shots { get; private set; }
    [Export] public float timeBetweenShots { get; private set; }
}
