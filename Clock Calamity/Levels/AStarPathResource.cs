using Godot;
using Godot.Collections;

public partial class AStarPathResource : Resource
{
    [Export] private Array<Vector2I> _cells;
    [Export] private int shots;
    [Export] private float timeBetweenShots;

    public Array<Vector2I> Cells
    {
        get
        {
            return _cells;
        }
    }

    public int Shots
    {
        get
        {
            return shots;
        }
    }

    public float TimeBetweenShots
    {
        get
        {
            return timeBetweenShots;
        }
    }
}
