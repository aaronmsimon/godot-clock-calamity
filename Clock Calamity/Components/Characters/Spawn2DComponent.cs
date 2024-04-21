using CC.Enemies;
using Components.Pathfinding;
using Godot;

namespace Components.Characters
{
    [GlobalClass]
    public partial class Spawn2DComponent : Node2D
    {
        [ExportCategory("Spawn 2D Component")]
        [Export] private CharacterResource characterResource;

        [ExportGroup("Character")]
        [Export] private int charactersToSpawn;
        [Export] private float timeBetweenSpawns;

        [ExportGroup("Pathfinding")]
        [Export] private AStarGrid2DComponent aStarGrid2DComponent;
        [Export] private Waypoints2DResource waypoints2DResource;
        [Export] private GridResource gridResource;

        [ExportGroup("Firing")]
        [Export] private Node2D target;

        [Signal] public delegate void DoneSpawningEventHandler();

        private int characterCount = 0;
        private Timer spawnTimer = new Timer();

        public override void _Ready()
        {
            if (characterResource == null)
            {
                GD.PrintErr($"The Spawn2DComponent { this.Name } requires a CharacterResource. Please add one to node { this.GetPath() }");
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
            if (target == null)
            {
                GD.PrintErr($"The Spawn2DComponent { this.Name } requires a Node2D target for firing. Please add one to node { this.GetPath() }");
                return;
            }

            AddChild(spawnTimer);
            spawnTimer.Timeout += OnSpawnTimerTimeout;

            AddToGroup("SpawnPoints");

            // Deferred to allow LevelController the time to listen for the DoneSpawning signal if only one spawn.
            CallDeferred("OnSpawnTimerTimeout");
        }

        private void SpawnCharacter()
        {
            if (characterResource == null || aStarGrid2DComponent == null || waypoints2DResource == null) return;

            // Create instance
            Node2D instance = (Node2D)characterResource.character.Instantiate();
            // Set pathfinding info
            instance.Set("aStarGrid2DComponent", aStarGrid2DComponent);
            instance.Set("waypoints2DResource", waypoints2DResource);
            instance.Set("gridResource", gridResource);
            instance.Set("target", target);
            instance.Set("characterResource", characterResource);
            Enemy test = (Enemy)instance;
            // Add to scene as a child of this spawn point
            AddChild(instance);
        }

        private void OnSpawnTimerTimeout()
        {
            if (characterCount < charactersToSpawn && IsInstanceValid(target))
            {
                SpawnCharacter();
                characterCount++;
                if (characterCount < charactersToSpawn)
                {
                    spawnTimer.Start(timeBetweenSpawns);
                }
                else
                {
                    EmitSignal(SignalName.DoneSpawning);
                }
            }
        }
    }
}