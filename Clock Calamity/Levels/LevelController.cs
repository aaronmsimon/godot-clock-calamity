using Godot;
using Godot.Collections;
using CC.Characters;

namespace CC.Level
{
    public partial class LevelController : Node2D
    {
        private Array<Node> playerHides;
        private PlayerController player;

        private int playerHideIndex = 0;

        public override void _Ready()
        {
            playerHides = GetNode<Node>("PlayerHides").GetChildren();
            player = GetNode<PlayerController>("Player");

            player.TakeCover += OnTakeCover;
            player.PeakLeft += OnPeakLeft;
            player.PeakRight += OnPeakRight;

            MovePlayer(CurrentHide().HidePosition);
        }

        public Hide CurrentHide()
        {
            return (Hide)playerHides[playerHideIndex];
        }

        private void MovePlayer(Vector2 position)
        {
            player.GlobalPosition = position;
        }

        private void OnTakeCover()
        {
            MovePlayer(CurrentHide().HidePosition);
        }

        private void OnPeakLeft()
        {
            MovePlayer(CurrentHide().LeftPosition);
        }

        private void OnPeakRight()
        {
            MovePlayer(CurrentHide().RightPosition);
        }
    }
}
