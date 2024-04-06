using Godot;

namespace CC.Level
{
    public partial class Hide : Node
    {
        [Export] private Marker2D hidePosition;
        [Export] private Marker2D leftPosition;
        [Export] private Marker2D rightPosition;

        public Vector2 HidePosition
        {
            get
            {
                return hidePosition.GlobalPosition;
            }
        }

        public Vector2 LeftPosition
        {
            get
            {
                return leftPosition.GlobalPosition;
            }
        }

        public Vector2 RightPosition
        {
            get
            {
                return rightPosition.GlobalPosition;
            }
        }
    }
}
