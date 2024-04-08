using Godot;
using Godot.Collections;

public partial class AStarPathResource : Resource
{
    [Export] private Array<Vector2I> _cells;

    public Array<Vector2I> Cells
    {
        get
        {
            return _cells;
        }
    }
}
