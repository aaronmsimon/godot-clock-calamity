using Godot;
using Godot.Collections;
using Components.Pathfinding;
using Components.Game;
using System.Runtime.CompilerServices;
using Components.Characters;
using CC.Player;

namespace CC.Level
{
    public partial class LevelController : Node2D
    {
        [ExportCategory("Level Controller")]
        [Export] private GridResource occupiedResource;
        
        [ExportGroup("Player Death")]
        [Export] private double deathToGameOverTime;
        [Export(PropertyHint.File, "*.tscn")] private string gameOverScene;

        private Array<Node> playerHides;
        private PlayerController player;
        private AStarGrid2DComponent astarGrid2DComponent;
        private GameStatComponent scoreStatComponent;
        private Label scoreLabel;
        private PlayerResource playerResource;

        private int playerHideIndex = 0;
        private int spawnPointsCompleted = 0;

        public override void _Ready()
        {
            playerHides = GetNode<Node>("PlayerHides").GetChildren();
            player = GetNode<PlayerController>("Player");
            astarGrid2DComponent = GetNode<AStarGrid2DComponent>("AStarGrid2DComponent");
            scoreStatComponent = GetNode<GameStatComponent>("ScoreStatComponent");
            scoreLabel = GetNode<Label>("ScoreLabel");

            if (scoreStatComponent == null)
            {
                GD.PrintErr($"Level { this.Name } does not have a GameStatComponent for the Score stat so scoring will not be tracked.");
                return;
            }

            scoreStatComponent.gamestat.StatChanged += OnScoreChanged;
            foreach (Spawn2DComponent spawnPoint in GetTree().GetNodesInGroup("SpawnPoints"))
            {
                spawnPoint.DoneSpawning += OnSpawnPointCompleted;
            }
            playerResource = player.playerResource;
            playerResource.Die += GameOver;

            ResetGrid();
            OnScoreChanged();
        }

        public override void _Process(double delta)
        {
            if (LevelComplete())
            {
                GameOver();
            }
        }

        public override void _ExitTree()
        {
            scoreStatComponent.gamestat.StatChanged -= OnScoreChanged;
            foreach (Spawn2DComponent spawnPoint in GetTree().GetNodesInGroup("SpawnPoints"))
            {
                spawnPoint.DoneSpawning -= OnSpawnPointCompleted;
            }
            // player.TreeExiting -= GameOver;
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

        private async void GameOver()
        {
            playerResource.Die -= GameOver;
            if (!player.playerResource.IsAlive) player.QueueFree();
            await ToSignal(GetTree().CreateTimer(deathToGameOverTime), Timer.SignalName.Timeout);
            GetTree().ChangeSceneToFile(gameOverScene);
        }

        private void OnSpawnPointCompleted()
        {
            spawnPointsCompleted++;
        }

        private bool LevelComplete()
        {
            int spawnPoints = GetTree().GetNodesInGroup("SpawnPoints").Count;
            int enemies = GetTree().GetNodesInGroup("Enemies").Count;

            if (spawnPointsCompleted >= spawnPoints && enemies == 0)
            {
                return true;
            }

            return false;
        }
    }
}
