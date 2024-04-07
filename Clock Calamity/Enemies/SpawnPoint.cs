using System.Dynamic;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;

public partial class SpawnPoint : Marker2D
{
    [Export] private AStarPathResource _path;

    public AStarPathResource Path
    {
        get {
            return _path;
        }
    }
}
