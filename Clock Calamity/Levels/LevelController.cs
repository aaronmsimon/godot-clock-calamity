using Godot;
using Godot.Collections;
using Components.Pathfinding;
using Components.Game;

namespace CC.Level
{
    public partial class LevelController : Node2D
    {
        [Export] private GridResource occupiedResource;

        private Array<Node> playerHides;
        // private PlayerController player;
        private AStarGrid2DComponent astarGrid2DComponent;
        private GameStatComponent scoreStatComponent;
        private Label scoreLabel;

        private int playerHideIndex = 0;

        public override void _Ready()
        {
            playerHides = GetNode<Node>("PlayerHides").GetChildren();
            // player = GetNode<PlayerController>("Player");
            astarGrid2DComponent = GetNode<AStarGrid2DComponent>("AStarGrid2DComponent");
            scoreStatComponent = GetNode<GameStatComponent>("ScoreStatComponent");
            scoreLabel = GetNode<Label>("ScoreLabel");

            if (scoreStatComponent == null)
            {
                GD.PrintErr($"Level { this.Name } does not have a GameStatComponent for the Score stat so scoring will not be tracked.");
                return;
            }

            scoreStatComponent.gamestat.StatChanged += OnScoreChanged;

            ResetGrid();
            OnScoreChanged();
        }

        private void ResetGrid()
        {
            occupiedResource.Data = new Node2D[astarGrid2DComponent.astarGrid2D.Region.Size.X, astarGrid2DComponent.astarGrid2D.Region.Size.Y];
            for (int x = 0; x < astarGrid2DComponent.astarGrid2D.Region.Size.X; x++)
            {
                for (int y = 0; y < astarGrid2DComponent.astarGrid2D.Region.Size.Y; y++)
                {
                    occupiedResource.Data[x, y] = null;
                }
            }
        }

        private void OnScoreChanged()
        {
            scoreLabel.Text = $"Score: {scoreStatComponent.gamestat.StatValue:n0}";
        }
    }
}
