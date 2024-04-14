using Godot;

namespace Components.Movement
{
    [GlobalClass]
    public partial class FixedMovement2DComponent : Node
    {
        [Export] private Node2D actor;

        public void MoveActorToMarker(Marker2D marker2D)
        {
            actor.GlobalPosition = marker2D.GlobalPosition;
        }
    }
}
