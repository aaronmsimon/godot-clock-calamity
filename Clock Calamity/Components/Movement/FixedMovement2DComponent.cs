using Godot;

namespace Components.Movement
{
    [GlobalClass]
    public partial class FixedMovement2DComponent : Node
    {
        [Export] private Node2D actor;

        public override void _Ready()
        {
            if (actor == null)
            {
                GD.PrintErr("The FixedMovement2DComponent " + this.Name + " requires a Node2D to move. Please add one to node " + this.GetPath());
                return;
            }
        }
        
        public void MoveActorToMarker(Marker2D marker2D)
        {
            if (actor == null || marker2D == null) return;
            
            actor.GlobalPosition = marker2D.GlobalPosition;
        }
    }
}
