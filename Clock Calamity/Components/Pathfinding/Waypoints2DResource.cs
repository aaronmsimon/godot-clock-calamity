using Godot;
using Godot.Collections;

namespace Components.Pathfinding
{
    [GlobalClass]
    public partial class Waypoints2DResource : Resource
    {
        [Export] public Array<Vector2I> waypoints { get; private set; }
    }
}
