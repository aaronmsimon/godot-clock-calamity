using Godot;
using Godot.Collections;
using CC.Characters;
using Components.Game;
using Components.Pathfinding;

namespace CC.Level
{
    public partial class LevelController : Node2D
    {
        [Export] private GridResource occupiedResource;

        private Array<Node> playerHides;
        private PlayerController player;
        private StatsComponent statsComponent;
        private AStarGrid2DComponent astarGrid2DComponent;

        private int playerHideIndex = 0;

        public override void _Ready()
        {
            playerHides = GetNode<Node>("PlayerHides").GetChildren();
            player = GetNode<PlayerController>("Player");
            statsComponent = GetNode<StatsComponent>("StatsComponent");
            astarGrid2DComponent = GetNode<AStarGrid2DComponent>("AStarGrid2DComponent");

            ResetGrid();

            player.TakeCover += OnTakeCover;
            player.PeakLeft += OnPeakLeft;
            player.PeakRight += OnPeakRight;
            player.weapon.ShotFired += OnShotFired;

            MovePlayer(CurrentHide().HidePosition);
        }

        private void ResetGrid()
        {
            occupiedResource.Data = new bool[astarGrid2DComponent.astarGrid2D.Region.Size.X, astarGrid2DComponent.astarGrid2D.Region.Size.Y];
            for (int x = 0; x < astarGrid2DComponent.astarGrid2D.Region.Size.X; x++)
            {
                for (int y = 0; y < astarGrid2DComponent.astarGrid2D.Region.Size.Y; y++)
                {
                    occupiedResource.Data[x, y] = false;
                }
            }
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

        private void OnShotFired()
        {
            statsComponent.UpdateShotsFired(1);
        }
    }
}
