using Godot;
using System.Threading.Tasks;
using Components.Pathfinding;
using Components.Game;

namespace CC.Characters
{
    public partial class Enemy : Node2D
    {
        [Export] private PackedScene projectile;
        [Export] private float projectileSpeed = 500f;
        [Export] private float projectileDamage = 1f;
        [Export] private float scoreTiming = 3f;
        [Export] private float baseScore = 1000f;

        // Resources
        public AStarGrid2DComponent astarGrid2DComponent { private get; set; }
        public WaypointsResource pathResource { private get; set; }
        public NPCResource npcResource { private get; set; }
        public GridResource occupiedResource { private get; set; }
        
        public PlayerController player { get; private set; }

        // Pathfinding
        private FollowWaypointsComponent followWaypointsComponent;
        
        private StatsComponent statsComponent;

        // Aiming info
        private Node2D anchor;
        private Marker2D muzzle;
        private Timer shotTimer = new Timer();
        private bool firing = false;

        // Scoring
        private Timer scoreTimer = new Timer();

        // Signals
        [Signal] public delegate void DamagedEventHandler();
        [Signal] public delegate void DiedEventHandler();

        public override void _Ready()
        {
            player = GetNode<PlayerController>("../../Player");
            anchor = GetNode<Node2D>("Anchor");
            muzzle = GetNode<Marker2D>("Anchor/MuzzleMarker");

            followWaypointsComponent = GetNode<FollowWaypointsComponent>("FollowWaypointsComponent");
            followWaypointsComponent.astarGrid2DComponent = astarGrid2DComponent;
            followWaypointsComponent.pathResource = pathResource;
            followWaypointsComponent.npcResource = npcResource;
            followWaypointsComponent.occupiedResource = occupiedResource;

            statsComponent = GetNode<StatsComponent>("StatsComponent");

            AddChild(shotTimer);
            AddChild(scoreTimer);

            scoreTimer.Start(scoreTiming);
        }

        public async Task Fire()
        {
            Node2D projectileInstance = (Node2D)projectile.Instantiate();
            projectileInstance.Set("speed", projectileSpeed);
            projectileInstance.Set("damage", projectileDamage);
            projectileInstance.GlobalPosition = muzzle.GlobalPosition;
            projectileInstance.Rotation = Rotation;
            Node parent = GetParent();
            parent.AddChild(projectileInstance);

            shotTimer.Start(npcResource.timeBetweenShots);
            await ToSignal(shotTimer, Timer.SignalName.Timeout);
        }

        public void TakeDamage(int damage)
        {
            npcResource.health -= damage;
            statsComponent.UpdateShotsHit(1);
            EmitSignal(SignalName.Damaged);

            if (npcResource.health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            statsComponent.UpdateScore((int)((float)scoreTimer.TimeLeft / scoreTiming * baseScore));
            EmitSignal(SignalName.Died);
            QueueFree();
        }
    }
}
