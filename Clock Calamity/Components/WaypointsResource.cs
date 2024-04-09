using Godot;
using Godot.Collections;

namespace Components.Pathfinding
{
    public partial class WaypointsResource : Resource
    {
        [Export] public Array<Vector2I> waypoints { get; private set; }
    }
}
