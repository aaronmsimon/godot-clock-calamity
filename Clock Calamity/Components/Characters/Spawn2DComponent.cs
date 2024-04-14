using Components.Pathfinding;
using Godot;

namespace Components.Characters
{
    [GlobalClass]
    public partial class Spawn2DComponent : Node2D
    {
        [ExportCategory("Spawn 2D Component")]
        [ExportGroup("Character")]
        [Export] private PackedScene character;
        [Export] private int charactersToSpawn;
        [Export] private float timeBetweenSpawns;

        [ExportGroup("Pathfinding")]
        [Export] private AStarGrid2DComponent aStarGrid2DComponent;
        [Export] private Waypoints2DResource waypoints2DResource;

        private int characterCount = 0;
        private Timer spawnTimer = new Timer();

        public override void _Ready()
        {
            if (character == null)
            {
                GD.PrintErr($"The Spawn2DComponent { this.Name } requires a character to spawn. Please add one to node { this.GetPath() }");
                return;
            }
            if (aStarGrid2DComponent == null)
            {
                GD.PrintErr($"The Spawn2DComponent { this.Name } requires an AStarGrid2DComponent for pathfinding. Please add one to node { this.GetPath() }");
                return;
            }
            if (waypoints2DResource == null)
            {
                GD.PrintErr($"The Spawn2DComponent { this.Name } requires a Waypoints2DResource for pathfinding. Please add one to node { this.GetPath() }");
                return;
            }

            AddChild(spawnTimer);
            spawnTimer.Timeout += OnSpawnTimerTimeout;

            OnSpawnTimerTimeout();
        }

        private void SpawnCharacter()
        {
            if (character == null || aStarGrid2DComponent == null || waypoints2DResource == null) return;

            // Create instance
            Node2D instance = (Node2D)character.Instantiate();
            // Set pathfinding info
            instance.Set("aStarGrid2DComponent", aStarGrid2DComponent);
            instance.Set("waypoints2DResource", waypoints2DResource);
            // Add to scene as a child of this spawn point
            AddChild(instance);
        }

        private void OnSpawnTimerTimeout()
        {
            if (characterCount < charactersToSpawn)
            {
                SpawnCharacter();
                characterCount++;
                spawnTimer.Start(timeBetweenSpawns);
            }
        }
    }
}